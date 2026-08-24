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
    loadComponent: () => import('./shared/ui/page-placeholder').then((m) => m.PagePlaceholder),
    data: {
      icon: 'description',
      title: 'Your resumes',
      subtitle: 'Upload and manage the resumes Hirepath reasons about.',
    },
  },
  {
    path: 'settings',
    title: 'Settings · Hirepath',
    loadComponent: () => import('./shared/ui/page-placeholder').then((m) => m.PagePlaceholder),
    data: {
      icon: 'settings',
      title: 'Settings',
      subtitle: 'Choose your AI provider, model, and theme.',
    },
  },
  { path: '**', redirectTo: 'chat' },
];
