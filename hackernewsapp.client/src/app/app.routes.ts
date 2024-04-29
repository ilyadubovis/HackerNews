import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'news',
    children: [
      {
        path: ':category',
        loadComponent: () => import('./components/news-list/news-list.component').then(m => m.NewsListComponent)
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'news/topstories',
    pathMatch: 'full'
  }
];
