import { Component, signal } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NgFor } from '@angular/common';

interface ProjectListItem {
  id: string;
  name: string;
  owner: string;
  status: 'On Track' | 'At Risk' | 'Blocked';
  members: number;
}

@Component({
  selector: 'tm-projects',
  standalone: true,
  imports: [RouterOutlet, RouterLink, NgFor],
  template: `
    <div class="grid gap-6" [class.grid-cols-2]="projects().length > 0">
      <section class="space-y-4">
        <header class="flex items-center justify-between">
          <div>
            <h2 class="text-xl font-semibold">Projects</h2>
            <p class="text-sm text-foreground/60">Manage workspaces and monitor their health.</p>
          </div>
          <button class="btn-primary">New Project</button>
        </header>
        <ul class="space-y-3">
          <li *ngFor="let project of projects()" class="card flex items-center justify-between">
            <div>
              <a [routerLink]="[project.id]" class="font-semibold text-foreground hover:text-accent">{{ project.name }}</a>
              <div class="text-xs text-foreground/60">Owner: {{ project.owner }}</div>
            </div>
            <div class="flex items-center space-x-3">
              <span class="badge" [class.bg-success]="project.status === 'On Track'" [class.bg-warning]="project.status === 'At Risk'" [class.bg-danger]="project.status === 'Blocked'">
                {{ project.status }}
              </span>
              <span class="text-sm text-foreground/60">{{ project.members }} members</span>
            </div>
          </li>
        </ul>
      </section>
      <router-outlet></router-outlet>
    </div>
  `,
  styles: [
    `
      .card {
        @apply bg-layer-1 border border-border rounded-xl p-4 shadow-sm;
      }
      .btn-primary {
        @apply inline-flex items-center px-4 py-2 rounded-lg bg-accent text-white text-sm font-medium shadow hover:bg-accent/90 transition;
      }
      .badge {
        @apply px-2 py-1 rounded-full text-xs font-semibold text-white;
      }
      .bg-success {
        @apply bg-emerald-500;
      }
      .bg-warning {
        @apply bg-amber-500;
      }
      .bg-danger {
        @apply bg-rose-500;
      }
    `,
  ],
})
export class ProjectsComponent {
  readonly projects = signal<ProjectListItem[]>(
    Array.from({ length: 8 }).map((_, index) => ({
      id: `${index + 1}`,
      name: `Next Gen Platform ${index + 1}`,
      owner: 'Alex Johnson',
      status: index % 3 === 0 ? 'On Track' : index % 3 === 1 ? 'At Risk' : 'Blocked',
      members: 8 + index,
    }))
  );
}
