import { Route } from '@angular/router';
import { authGuard } from './guards/auth-guard';
import { inject } from '@angular/core';
import { Common } from './services/common';

export const appRoutes: Route[] = [
    {
        path: 'unauthorize',
        loadComponent: () => import('./pages/unatuhorize/unatuhorize')
    },
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
                loadComponent: () => import('./pages/dashboard/dashboard'),
                canActivate: [() => inject(Common).checkPermissionForRoute('dashboard:view')]
            },
            {
                path: 'branches',
                loadChildren: () => import('./pages/branches/router')
            },
            {
                path: 'roles',
                loadChildren: () => import('./pages/roles/router')
            }
        ]
    }
];
