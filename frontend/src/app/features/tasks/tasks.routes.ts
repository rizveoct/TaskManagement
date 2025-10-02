import { Routes } from '@angular/router';

export const TASK_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./tasks-board.component').then((m) => m.TasksBoardComponent),
  },
  {
    path: ':id',
    loadComponent: () => import('./task-detail.component').then((m) => m.TaskDetailComponent),
  },
];
