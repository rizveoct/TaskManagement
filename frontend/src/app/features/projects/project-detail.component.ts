import { Component, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'tm-project-detail',
  standalone: true,
  imports: [NgFor, NgIf],
  template: `
    <section class="card h-full">
      <header class="flex items-start justify-between">
        <div>
          <h2 class="text-xl font-semibold">{{ projectName() }}</h2>
          <p class="text-sm text-foreground/60">{{ description() }}</p>
        </div>
        <button class="btn-secondary">Edit</button>
      </header>
      <div class="mt-4 space-y-4">
        <div>
          <h3 class="text-sm font-semibold uppercase text-foreground/60">Team</h3>
          <div class="flex -space-x-2 mt-2">
            <div *ngFor="let member of members()" class="avatar">{{ member }}</div>
          </div>
        </div>
        <div>
          <h3 class="text-sm font-semibold uppercase text-foreground/60">Active Boards</h3>
          <ul class="space-y-2">
            <li *ngFor="let board of boards()" class="flex items-center justify-between">
              <span>{{ board.name }}</span>
              <span class="text-xs text-foreground/60">{{ board.statuses.length }} statuses • {{ board.tasks }} tasks</span>
            </li>
          </ul>
        </div>
      </div>
    </section>
  `,
  styles: [
    `
      .card {
        @apply bg-layer-1 border border-border rounded-xl p-6 shadow-sm;
      }
      .btn-secondary {
        @apply inline-flex items-center px-4 py-2 rounded-lg border border-border text-sm font-medium hover:border-accent transition;
      }
      .avatar {
        @apply w-10 h-10 rounded-full bg-layer-2 text-foreground flex items-center justify-center text-sm font-semibold border border-border;
      }
    `,
  ],
})
export class ProjectDetailComponent {
  readonly projectName = signal('Project Overview');
  readonly description = signal('Real-time collaboration workspace with Kanban, calendar, and Gantt views.');
  readonly members = signal(['AJ', 'MS', 'TD', 'JL']);
  readonly boards = signal(
    Array.from({ length: 3 }).map((_, index) => ({
      name: `Board ${index + 1}`,
      statuses: ['To Do', 'In Progress', 'Review', 'Done'],
      tasks: 12 + index * 4,
    }))
  );

  constructor(private readonly route: ActivatedRoute) {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.projectName.set(`Project #${params['id']}`);
      }
    });
  }
}
