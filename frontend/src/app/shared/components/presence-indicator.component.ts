import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { NgFor } from '@angular/common';
import { PresenceService } from '../../core/services/presence.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'presence-indicator',
  standalone: true,
  imports: [NgFor],
  template: `
    <div class="p-4 border-t border-border space-y-3">
      <div class="text-xs uppercase tracking-wide text-foreground/60">Online</div>
      <div class="flex -space-x-2">
        <div *ngFor="let user of onlineUsers()" class="avatar" [title]="user">
          {{ initials(user) }}
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .avatar {
        @apply w-8 h-8 rounded-full bg-accent/10 text-accent flex items-center justify-center text-xs font-semibold border border-accent/40;
      }
    `,
  ],
})
export class PresenceIndicatorComponent implements OnInit {
  readonly onlineUsers = signal<string[]>([]);

  constructor(private readonly presenceService: PresenceService, private readonly destroyRef: DestroyRef) {}

  ngOnInit(): void {
    this.presenceService
      .onlineUsers()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((users) => this.onlineUsers.set(users));
  }

  initials(name: string) {
    return name
      .split(' ')
      .map((part) => part.charAt(0).toUpperCase())
      .join('');
  }
}
