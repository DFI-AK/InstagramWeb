import { inject, Injectable, signal } from '@angular/core';
import { HttpError, HttpTransportType, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { environment } from 'src/environments/environment.prod';
import { ChatDto, Message } from '../models/interfaces';
import { ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  public chats = signal<Partial<Message>[]>([]);

  private readonly activatedRoute = inject(ActivatedRoute);

  private hubConnection = new HubConnectionBuilder()
    .configureLogging(environment.production ? LogLevel.Warning : LogLevel.Debug)
    .withUrl(environment.endpoint + '/chathub', { transport: HttpTransportType.WebSockets | HttpTransportType.LongPolling })
    .withAutomaticReconnect([36000])
    .withKeepAliveInterval(60 * 60 * 1000)
    .build();

  constructor() {
    if (this.hubConnection.state === HubConnectionState.Disconnected) {
      this.hubConnection.start()
        .then(() => {
          if (this.hubConnection.state === HubConnectionState.Connected) {
            // ===========Receive chats========

            this.hubConnection.on('SendMessage', (receiverId, message: Message[]) => {
              console.log(message);
              this.chats.update((prev) => [...prev, ...message]);
            });

            this.hubConnection.on('ReceiveMessage', (receiverId, message: Message) => {
              this.chats.update((prev) => [...prev, message]);
            });

            this.activatedRoute.queryParams.subscribe({
              next: param => {
                const { userId } = param;
                this.hubConnection.invoke('InvokeReceiveMessage', userId)
                  .catch(err => console.log(err));
              }
            });
          }
        })
        .catch(error => {
          if (error instanceof HttpError) {
            console.log(error.statusCode);
          }
        });
    }
  }

  public sendMessage(receiverId: string, message: string) {
    this.hubConnection.invoke('InvokeSendMessage', receiverId, message)
      .catch(err => console.log(err));
  }

}
