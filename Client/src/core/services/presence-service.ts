import { inject, Injectable, NgZone, signal, Signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../../types/user';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Message } from '../../types/message';
import { ToastService } from './toast-service';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  private hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  public onlineUsers = signal<string[]>([]);
  private toastService = inject(ToastService);

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token 
      })
      .withAutomaticReconnect().build();
      
    this.hubConnection.start().catch(error => console.log(error));
    this.hubConnection.on('UserOnline', userId => this.onlineUsers.update(users => [...users, userId]));
    this.hubConnection.on('UserOffline', userId => this.onlineUsers.update(users => users.filter(x => x !== userId)));
    this.hubConnection.on('GetOnlineUsers', userIds => this.onlineUsers.set(userIds));
    this.hubConnection.on('NewMessageReceived', (msg: Message) => {
      this.toastService.info(msg.senderUserName + ' has sent you a new message.',
        10 * 1000, msg.senderImageUrl, `/members/${msg.senderId}/messages`);
    });
  }

  stopHubConnection() {
    if (this.isConnected()) {
      this.hubConnection?.stop().catch(error => console.log(error));
    }
  }

  isConnected() {
    return this.hubConnection?.state === HubConnectionState.Connected;
  }
}
