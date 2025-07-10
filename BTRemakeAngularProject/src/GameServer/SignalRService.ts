import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  private connectionStatus = new BehaviorSubject<boolean>(false);
  private messageSubject = new BehaviorSubject<any>(null);
  private fullStateSubject = new BehaviorSubject<any>(null);

  public connectionStatus$ = this.connectionStatus.asObservable();
  public message$ = this.messageSubject.asObservable();
  public fullState$ = this.fullStateSubject.asObservable();

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  private createConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:8080/GameHub')
      .withAutomaticReconnect()
      .build();
  }

  private registerOnServerEvents(): void {
    // Écouter les messages du serveur
    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      this.messageSubject.next({ user, message, timestamp: new Date() });
    });

    this.hubConnection.on('FullGameState', (parameter: any) => {
      this.fullStateSubject.next({ timestamp: new Date(), parameter });
    });

    this.hubConnection.on('UserConnected', (user: string) => {
      console.log(`${user} s'est connecté`);
    });

    this.hubConnection.on('UserDisconnected', (user: string) => {
      console.log(`${user} s'est déconnecté`);
    });
  }

  private async startConnection(): Promise<void> {
    try {
      await this.hubConnection.start();
      console.log('Connexion SignalR établie');
      this.connectionStatus.next(true);
    } catch (error) {
      console.error('Erreur de connexion SignalR:', error);
      this.connectionStatus.next(false);
      // Retry après 5 secondes
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  // Méthodes publiques pour envoyer des messages
  public async sendMessage(user: string, message: string): Promise<void> {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      await this.hubConnection.invoke('SendMessage', user, message);
    }
  }

  public async joinGroup(groupName: string): Promise<void> {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      await this.hubConnection.invoke('JoinGroup', groupName);
    }
  }

  public async leaveGroup(groupName: string): Promise<void> {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      await this.hubConnection.invoke('LeaveGroup', groupName);
    }
  }

  public async disconnect(): Promise<void> {
    if (this.hubConnection) {
      await this.hubConnection.stop();
      this.connectionStatus.next(false);
    }
  }
}
