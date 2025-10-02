import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SignalRService } from './signalr.service';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PresenceService {
  private readonly users$ = new BehaviorSubject<string[]>([]);

  constructor(private readonly signalR: SignalRService) {
    void this.initialize();
  }

  private async initialize() {
    const connection = await this.signalR.connect(environment.signalR.presenceHub);
    connection.on('UserOnline', (userId: string) => this.addUser(userId));
    connection.on('UserOffline', (userId: string) => this.removeUser(userId));

    try {
      const users = await connection.invoke<string[]>('GetOnlineUsers');
      this.users$.next(users);
    } catch (error) {
      console.error('Failed to load online users', error);
    }
  }

  onlineUsers() {
    return this.users$.asObservable();
  }

  private addUser(userId: string) {
    const users = new Set(this.users$.value);
    users.add(userId);
    this.users$.next(Array.from(users));
  }

  private removeUser(userId: string) {
    const users = this.users$.value.filter((user) => user !== userId);
    this.users$.next(users);
  }
}
