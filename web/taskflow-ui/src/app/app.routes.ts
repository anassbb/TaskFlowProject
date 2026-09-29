import { Routes } from '@angular/router';

// Chaque feature est chargée à la demande (lazy loading) : le bundle initial reste léger.
export const routes: Routes = [
  {
    path: '',
    title: 'TaskFlow',
    loadComponent: () => import('./features/home/home').then((m) => m.Home),
  },
  // TODO (toi) : { path: 'projects', loadChildren: () => import('./features/projects/projects.routes') },
  { path: '**', redirectTo: '' },
];
