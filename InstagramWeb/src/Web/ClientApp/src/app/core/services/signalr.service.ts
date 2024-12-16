import { inject, Injectable, signal } from '@angular/core';
import { HttpError, HttpTransportType, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { environment } from 'src/environments/environment.prod';
import { ChatDto, Message } from '../models/interfaces';
import { ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  public chats = signal<ChatDto>({ user: null, messages: [] });

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
            this.activatedRoute.queryParams.subscribe({
              next: (param) => {
                const { userId } = param;
                console.log("User ID:", userId); // Debugging log
                if (userId) {
                  // Clear the chats state before fetching messages for the new user
                  this.chats.set({ user: null, messages: [] });
            
                  // Invoke the hub method to receive messages for the new user
                  this.hubConnection.invoke('InvokeReceiveMessage', userId)
                    .catch(err => console.log('Error invoking InvokeReceiveMessage:', err));
                } else {
                  console.log('UserId is missing from query parameters.');
                }
              }
            });
            

            this.hubConnection.on('SendMessage', (receiverId, chat: ChatDto) => {
              const message = [...this.chats().messages, ...chat.messages]
              const filtered = message.filter((va, idx, _self) => idx === _self.findIndex(x => x.messageId === va.messageId))
              this.chats.update(prev => ({
                messages: filtered,
                user: prev.user
              }));

            });

            this.hubConnection.on('ReceiveMessage', (receiverId, chat: ChatDto) => {
              const filtered = chat.messages.filter((va, idx, _self) => 
                idx === _self.findIndex(x => x.messageId === va.messageId)
              );
              this.chats.set({
                messages: filtered,
                user: chat.user
              });
            });
            

            this.hubConnection.onreconnecting(() => {
              console.log("Reconnecting to SignalR hub...");
            });
            
            this.hubConnection.onreconnected(() => {
              console.log("Reconnected to SignalR hub.");
            });
            


            this.hubConnection.on('sentMessageSuccessfully', (chats: ChatDto) => {
              console.log('chats', chats)
              this.chats.set(chats)
            })
            this.hubConnection.onclose((error) => {
              console.error("Connection closed with error:", error);
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
