import { Injectable } from '@angular/core';

import * as signalR from '@microsoft/signalr';
import { AppConsts } from '../../../shared/AppConsts';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  private listeners: Array<(user: string, message: string) => void> = [];

  constructor() {
    this.startConnection();
  }

  private startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(AppConsts.appBaseUrl + AppConsts.chatHubUrl) // '/signalr/chatHub'
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('SignalR Error: ', err));

    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      this.listeners.forEach(callback => callback(user, message));
    });
  }

  sendMessage(user: string, message: string): void {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('SendMessage', user, message)
        .catch(err => console.error(err));
    } else {
      console.warn('SignalR is not connected.');
    }
  }

  onMessageReceived(callback: (user: string, message: string) => void): void {
    this.listeners.push(callback);
  }
}
