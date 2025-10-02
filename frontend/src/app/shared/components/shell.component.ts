import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AsyncPipe, NgIf } from '@angular/common';
import { NotificationCenterComponent } from './notification-center.component';
import { PresenceIndicatorComponent } from './presence-indicator.component';

@Component({
  selector: 'tm-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, NotificationCenterComponent, PresenceIndicatorComponent, AsyncPipe, NgIf],
  template: `
    <div class="flex min-h-screen">
      <aside class="w-64 bg-layer-1 border-r border-border hidden md:flex flex-col">
        <div class="p-6 text-lg font-semibold">Task Management</div>
        <nav class="flex-1 px-4 space-y-2">
          <a routerLink="/dashboard" routerLinkActive="bg-layer-2" class="nav-link">Dashboard</a>
          <a routerLink="/projects" routerLinkActive="bg-layer-2" class="nav-link">Projects</a>
          <a routerLink="/tasks" routerLinkActive="bg-layer-2" class="nav-link">Tasks</a>
          <a routerLink="/calendar" routerLinkActive="bg-layer-2" class="nav-link">Calendar</a>
        </nav>
        <presence-indicator></presence-indicator>
      </aside>
      <div class="flex-1 flex flex-col">
        <header class="h-16 bg-layer-1 border-b border-border flex items-center justify-between px-4">
          <div class="font-semibold">Real-Time Collaboration</div>
          <notification-center></notification-center>
        </header>
        <main class="flex-1 overflow-y-auto p-4 bg-layer-0">
          <router-outlet></router-outlet>
        </main>
      </div>
    </div>
  `,
  styles: [
    `
      .nav-link {
        @apply block rounded-md px-3 py-2 text-sm font-medium text-foreground/80 hover:text-foreground hover:bg-layer-2 transition;
      }
    `,
  ],
})
export class ShellComponent {}
