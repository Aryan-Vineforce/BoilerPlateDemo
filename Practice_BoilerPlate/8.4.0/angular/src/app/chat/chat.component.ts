import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AppConsts } from '@shared/AppConsts';
import { UserDto } from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit {
  user: string = '';
  message: string = '';
  messages: { user: string; message: string; status: 'sent' | 'delivered' | 'seen' }[] = [];
  selectedUser: any = null;
userList: UserDto[] = [];
optionsVisible = false;
  private hubConnection: signalR.HubConnection;

  constructor(private appSession: AppSessionService) {}

  ngOnInit(): void {
    this.user = this.appSession.user?.userName || 'Guest';
    this.loadMessagesFromLocalStorage();

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(AppConsts.remoteServiceBaseUrl + '/signalr/chatHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().then(() => console.log('Connected')).catch(console.error);

    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      const isMine = user === this.user;
      this.messages.push({
        user,
        message,
        status: isMine ? 'delivered' : 'seen',
      });
      this.saveMessagesToLocalStorage();
    });
  }

  sendMessage(): void {
  if (!this.message.trim()) return;

  this.hubConnection.invoke('SendMessage', this.user, this.message)
    .then(() => {
      this.message = ''; // Only clear the input
      // Don't push to messages[] here — wait for SignalR callback
    })
  
    .catch(err => console.error(err));
}

  loadMessagesFromLocalStorage(): void {
    const saved = localStorage.getItem('chatMessages');
    if (saved) {
      this.messages = JSON.parse(saved);
    }
  }

  saveMessagesToLocalStorage(): void {
    localStorage.setItem('chatMessages', JSON.stringify(this.messages));
  }
  selectUser(user: any) {
  this.selectedUser = user;
  // Optionally: load messages for selected user here
}


toggleOptions() {
  this.optionsVisible = !this.optionsVisible;
}

viewContact() {
  this.optionsVisible = false;
  alert('👤 Contact Info\nName: ' + this.user);
}

selectMessages() {
  console.log('Select messages clicked');
  this.optionsVisible = false;
}

closeChat() {
  this.optionsVisible = false;
  this.messages = [];
  this.user = '';
  alert('❌ Chat closed');
}

mute() {
  console.log('Mute notifications clicked');
  this.optionsVisible = false;
}

disappearingMessages() {
  console.log('Disappearing messages clicked');
  this.optionsVisible = false;
}

clearMessages() {
  this.optionsVisible = false;
  if (confirm('Are you sure you want to clear all messages?')) {
    this.messages = [];
  }
}

deleteChat() {
  this.optionsVisible = false;
  if (confirm('⚠️ Delete entire chat? This cannot be undone.')) {
    this.messages = [];
    this.user = '';
  }

}
}
