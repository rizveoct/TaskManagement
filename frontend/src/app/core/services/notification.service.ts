import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { SignalRService } from './signalr.service';
import { environment } from '../../../environments/environment';

export interface NotificationPayload {
  id: string;
  message: string;
  createdAt: Date;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly notifications$ = new Subject<NotificationPayload>();

  constructor(private readonly signalR: SignalRService) {
    void this.initialize();
  }

  private async initialize() {
    const connection = await this.signalR.connect(environment.signalR.notificationHub);
    connection.on('NotificationReceived', (id: string, message: string) => {
      this.notifications$.next({ id, message, createdAt: new Date() });
    });
  }

  stream() {
    return this.notifications$.asObservable();
  }
}
