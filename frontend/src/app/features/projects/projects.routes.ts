import { Routes } from '@angular/router';

export const PROJECT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./projects.component').then((m) => m.ProjectsComponent),
    children: [
      {
        path: ':id',
        loadComponent: () => import('./project-detail.component').then((m) => m.ProjectDetailComponent),
      },
    ],
  },
];
