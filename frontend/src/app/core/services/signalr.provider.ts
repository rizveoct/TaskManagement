import { makeEnvironmentProviders } from '@angular/core';
import { SignalRService } from './signalr.service';
import { NotificationService } from './notification.service';
import { PresenceService } from './presence.service';

export function provideSignalR() {
  return makeEnvironmentProviders([
    SignalRService,
    NotificationService,
    PresenceService,
  ]);
}
