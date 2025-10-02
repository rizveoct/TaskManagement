import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../core/services/notification.service';

interface NotificationItem {
  id: string;
  message: string;
  createdAt: Date;
  read: boolean;
}

@Component({
  selector: 'notification-center',
  standalone: true,
  imports: [NgFor, NgIf],
  template: `
    <div class="relative">
      <button class="relative btn-icon" (click)="toggle()">
        <span class="material-symbols-outlined">notifications</span>
        <span *ngIf="unreadCount() > 0" class="badge">{{ unreadCount() }}</span>
      </button>
      <div *ngIf="isOpen()" class="dropdown">
        <div class="dropdown-header">Notifications</div>
        <ul class="max-h-80 overflow-y-auto">
          <li *ngFor="let item of notifications()" class="dropdown-item" [class.unread]="!item.read">
            <div class="text-sm">{{ item.message }}</div>
            <div class="text-xs text-foreground/60">{{ item.createdAt | date: 'short' }}</div>
          </li>
        </ul>
      </div>
    </div>
  `,
  styles: [
    `
      .btn-icon {
        @apply w-10 h-10 rounded-full flex items-center justify-center bg-layer-2 text-foreground hover:bg-layer-3 transition;
      }
      .badge {
        @apply absolute -top-1 -right-1 bg-accent text-white text-xs px-1.5 py-0.5 rounded-full;
      }
      .dropdown {
        @apply absolute right-0 mt-2 w-72 bg-layer-1 border border-border rounded-lg shadow-lg z-50;
      }
      .dropdown-header {
        @apply px-4 py-2 border-b border-border font-medium;
      }
      .dropdown-item {
        @apply px-4 py-2 border-b border-border last:border-b-0;
      }
      .dropdown-item.unread {
        @apply bg-layer-2;
      }
    `,
  ],
})
export class NotificationCenterComponent implements OnInit, OnDestroy {
  readonly isOpen = signal(false);
  readonly notifications = signal<NotificationItem[]>([
    { id: 'seed-1', message: 'Standup starts in 5 minutes', createdAt: new Date(), read: false },
    { id: 'seed-2', message: 'Design board synced successfully', createdAt: new Date(), read: true },
  ]);
  private subscription?: Subscription;

  constructor(private readonly notificationService: NotificationService) {}

  ngOnInit(): void {
    this.subscription = this.notificationService.stream().subscribe((notification) => {
      this.notifications.update((current) => [
        { id: notification.id, message: notification.message, createdAt: notification.createdAt, read: false },
        ...current,
      ]);
    });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  toggle() {
    this.isOpen.update((value) => !value);
  }

  unreadCount() {
    return this.notifications().filter((notification) => !notification.read).length;
  }
}
