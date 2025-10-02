import { Injectable, NgZone } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private readonly connections = new Map<string, HubConnection>();
  private readonly startPromises = new Map<string, Promise<void>>();

  constructor(private readonly zone: NgZone) {}

  async connect(hubUrl: string): Promise<HubConnection> {
    const absoluteUrl = `${environment.apiUrl}${hubUrl}`;

    if (!this.connections.has(absoluteUrl)) {
      const connection = new HubConnectionBuilder()
        .withUrl(absoluteUrl, {
          accessTokenFactory: () => localStorage.getItem('access_token') ?? '',
        })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

      this.connections.set(absoluteUrl, connection);
    }

    const connection = this.connections.get(absoluteUrl)!;

    if (connection.state === HubConnectionState.Disconnected && !this.startPromises.has(absoluteUrl)) {
      const startPromise = this.zone.runOutsideAngular(() => connection.start());
      this.startPromises.set(absoluteUrl, startPromise);

      try {
        await startPromise;
      } catch (error) {
        console.error('SignalR connection failed', error);
        this.startPromises.delete(absoluteUrl);
        throw error;
      }

      this.startPromises.delete(absoluteUrl);
    } else if (this.startPromises.has(absoluteUrl)) {
      await this.startPromises.get(absoluteUrl);
    }

    return connection;
  }

  async stopAll(): Promise<void> {
    await Promise.all(
      Array.from(this.connections.values()).map(async (connection) => {
        if (connection.state === HubConnectionState.Connected) {
          await connection.stop();
        }
      })
    );

    this.connections.clear();
    this.startPromises.clear();
  }
}
