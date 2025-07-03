import { Route } from '@angular/router';
import { authGuard } from './guards/auth-guard';

export const appRoutes: Route[] = [
    {
        path: 'login',
        loadComponent: () => import('./pages/login/login')
    },
    {
        path: '',
        loadComponent: () => import('./pages/layouts/layouts'),
        canActivateChild: [authGuard],
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/dashboard/dashboard')
            }
        ]
    }
];
