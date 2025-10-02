import { Component, computed, signal } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';

interface MetricCard {
  title: string;
  value: string;
  delta: string;
  trend: 'up' | 'down' | 'neutral';
}

@Component({
  selector: 'tm-dashboard',
  standalone: true,
  imports: [NgFor, NgIf],
  template: `
    <section class="space-y-6">
      <header class="flex items-center justify-between">
        <div>
          <h1 class="text-2xl font-semibold">Team Performance</h1>
          <p class="text-sm text-foreground/60">Live metrics update in real-time as your team collaborates.</p>
        </div>
        <button class="btn-primary">Create Project</button>
      </header>
      <div class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-4">
        <div *ngFor="let card of metricCards()" class="card">
          <div class="text-sm text-foreground/60">{{ card.title }}</div>
          <div class="text-3xl font-semibold mt-2">{{ card.value }}</div>
          <div class="text-xs mt-1" [class.text-success]="card.trend === 'up'" [class.text-danger]="card.trend === 'down'">
            {{ card.delta }} vs last week
          </div>
        </div>
      </div>
      <div class="grid grid-cols-1 xl:grid-cols-3 gap-4">
        <div class="card xl:col-span-2">
          <h2 class="text-lg font-semibold mb-4">Active Projects</h2>
          <ul class="space-y-3">
            <li *ngFor="let project of projects()" class="flex items-center justify-between">
              <div>
                <div class="font-medium">{{ project.name }}</div>
                <div class="text-xs text-foreground/60">{{ project.team }} • Due {{ project.due | date }}</div>
              </div>
              <div class="text-sm font-semibold">{{ project.progress }}%</div>
            </li>
          </ul>
        </div>
        <div class="card">
          <h2 class="text-lg font-semibold mb-4">Live Activity</h2>
          <ul class="space-y-2">
            <li *ngFor="let activity of activityFeed()" class="text-sm">
              <span class="font-medium">{{ activity.user }}</span>
              {{ activity.action }}
              <span class="text-foreground/60">{{ activity.timestamp | date: 'shortTime' }}</span>
            </li>
          </ul>
        </div>
      </div>
    </section>
  `,
  styles: [
    `
      .card {
        @apply bg-layer-1 border border-border rounded-xl p-5 shadow-sm;
      }
      .btn-primary {
        @apply inline-flex items-center px-4 py-2 rounded-lg bg-accent text-white text-sm font-medium shadow hover:bg-accent/90 transition;
      }
      .text-success {
        @apply text-emerald-500;
      }
      .text-danger {
        @apply text-rose-500;
      }
    `,
  ],
})
export class DashboardComponent {
  readonly metricCards = signal<MetricCard[]>([
    { title: 'Active Tasks', value: '128', delta: '+12%', trend: 'up' },
    { title: 'Completed Today', value: '34', delta: '+4%', trend: 'up' },
    { title: 'Overdue', value: '5', delta: '-2%', trend: 'down' },
    { title: 'Team Online', value: '18', delta: '+3', trend: 'neutral' },
  ]);

  readonly projects = signal(
    Array.from({ length: 4 }).map((_, index) => ({
      name: `Project ${index + 1}`,
      team: 'Product Squad',
      due: new Date(Date.now() + index * 86400000),
      progress: 40 + index * 10,
    }))
  );

  readonly activityFeed = computed(() =>
    Array.from({ length: 6 }).map((_, index) => ({
      user: `User ${index + 1}`,
      action: 'updated a task',
      timestamp: new Date(Date.now() - index * 3600000),
    }))
  );
}
