import { Route } from '@angular/router';
import { authGuard } from './guards/auth-guard';

export const appRoutes: Route[] = [
    {
        path: 'login',
        loadComponent: () => import('./pages/auth/login/login')
    },
    {
        path: 'reset-password/:id',
        loadComponent: () => import('./pages/auth/reset-password/reset-password')
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
