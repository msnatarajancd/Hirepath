import { Routes } from '@angular/router';

/**
 * Chat is the front door. The other routes are the escape-hatch navigation;
 * their features arrive in later phases, so they show honest placeholders now.
 */
export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'chat' },
  {
    path: 'chat',
    title: 'Hirepath',
    loadComponent: () => import('./features/chat/chat').then((m) => m.Chat),
  },
  {
    path: 'analyses',
    title: 'Analyses · Hirepath',
    loadComponent: () => import('./shared/ui/page-placeholder').then((m) => m.PagePlaceholder),
    data: {
      icon: 'insights',
      title: 'Your analyses',
      subtitle: 'Every resume-to-role match you run will be listed here.',
    },
  },
  {
    path: 'resumes',
    title: 'Resumes · Hirepath',
    loadComponent: () => import('./features/resume/resumes').then((m) => m.Resumes),
  },
  {
    path: 'jobs',
    title: 'Job descriptions · Hirepath',
    loadComponent: () => import('./features/jobs/jobs').then((m) => m.Jobs),
  },
  {
    path: 'settings',
    title: 'Settings · Hirepath',
    loadComponent: () => import('./features/settings/settings').then((m) => m.Settings),
  },
  { path: '**', redirectTo: 'chat' },
];
