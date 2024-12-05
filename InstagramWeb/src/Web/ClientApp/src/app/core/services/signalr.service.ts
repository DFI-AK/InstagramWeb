import { Injectable, signal } from '@angular/core';
import { HttpError, HttpTransportType, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { environment } from 'src/environments/environment.prod';
import { ChatDto } from '../models/interfaces';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  public chats = signal<ChatDto | null>(null);

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
            this.hubConnection.on('ReceiveMessage', (receiverId, message: ChatDto) => {
              this.chats.set(message);
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
