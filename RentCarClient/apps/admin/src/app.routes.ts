import { Route } from '@angular/router';
import { authGuard } from './guards/auth-guard';
import { inject } from '@angular/core';
import { Common } from './services/common';

export const appRoutes: Route[] = [
    {
        path: 'unauthorize',
        loadComponent: () => import('./pages/unauthorize/unauthorize')
    },
    {
        path: 'unavailable',
        loadComponent: () => import('./pages/unavailable/unavailable')
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
        path: 'form/:type/:reservationId',
        loadComponent: () => import('./pages/reservations/form/form'),
        //canActivate: [() => inject(Common).checkPermissionForRoute('form:view')]
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
            },
            {
                path: 'categories',
                loadChildren: () => import('./pages/categories/router')
            },
            {
                path: 'protection-packages',
                loadChildren: () => import('./pages/protection-packages/router')
            },
            {
                path: 'users',
                loadChildren: () => import('./pages/users/router')
            },
            {
                path: 'extra',
                loadChildren: () => import('./pages/extra/router')
            },
            {
                path: 'vehicles',
                loadChildren: () => import('./pages/vehicles/router')
            },
            {
                path: 'customers',
                loadChildren: () => import('./pages/customers/router')
            },
             {
                path: 'reservations',
                loadChildren: () => import('./pages/reservations/router')
            },
        ]
    }
];
