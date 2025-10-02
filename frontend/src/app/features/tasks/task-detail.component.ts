import { Component, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'tm-task-detail',
  standalone: true,
  imports: [NgFor, NgIf],
  template: `
    <section class="card space-y-4">
      <header class="space-y-2">
        <div class="flex items-center justify-between">
          <h2 class="text-xl font-semibold">{{ title() }}</h2>
          <span class="badge">{{ priority() }}</span>
        </div>
        <p class="text-sm text-foreground/60">Status: {{ status() }} • Due {{ dueDate() | date }}</p>
      </header>
      <article>
        <h3 class="text-sm font-semibold uppercase text-foreground/60">Description</h3>
        <p class="mt-2 text-sm leading-relaxed">{{ description() }}</p>
      </article>
      <section>
        <h3 class="text-sm font-semibold uppercase text-foreground/60 mb-2">Subtasks</h3>
        <ul class="space-y-2">
          <li *ngFor="let subtask of subtasks()" class="flex items-center justify-between">
            <span>{{ subtask.title }}</span>
            <input type="checkbox" [checked]="subtask.completed" class="form-checkbox" />
          </li>
        </ul>
      </section>
      <section>
        <h3 class="text-sm font-semibold uppercase text-foreground/60 mb-2">Activity</h3>
        <ul class="space-y-2">
          <li *ngFor="let comment of comments()" class="text-sm">
            <span class="font-medium">{{ comment.author }}</span>
            {{ comment.message }}
            <span class="text-foreground/60">{{ comment.timestamp | date: 'short' }}</span>
          </li>
        </ul>
      </section>
    </section>
  `,
  styles: [
    `
      .card {
        @apply bg-layer-1 border border-border rounded-xl p-6 shadow-sm;
      }
      .badge {
        @apply inline-flex px-3 py-1 rounded-full bg-accent text-white text-xs font-semibold;
      }
      .form-checkbox {
        @apply h-4 w-4 rounded border-border text-accent focus:ring-accent;
      }
    `,
  ],
})
export class TaskDetailComponent {
  readonly title = signal('Collaborative Design Review');
  readonly priority = signal('High');
  readonly status = signal('In Progress');
  readonly dueDate = signal(new Date());
  readonly description = signal(
    'Coordinate with the design and engineering teams to finalize the real-time collaboration experience. Ensure accessibility and responsiveness across devices.'
  );
  readonly subtasks = signal(
    Array.from({ length: 5 }).map((_, index) => ({
      title: `Checklist item ${index + 1}`,
      completed: index % 2 === 0,
    }))
  );
  readonly comments = signal(
    Array.from({ length: 3 }).map((_, index) => ({
      author: `User ${index + 1}`,
      message: ' left a comment.',
      timestamp: new Date(Date.now() - index * 7200000),
    }))
  );

  constructor(private readonly route: ActivatedRoute) {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.title.set(`Task ${params['id']}`);
      }
    });
  }
}
