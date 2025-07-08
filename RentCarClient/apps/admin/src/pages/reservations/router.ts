import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./reservations'),
        canActivate: [() => inject(Common).checkPermissionForRoute('reservation:view')]
    },
    {
        path: 'add',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('reservation:create')]
    },
    {
        path: 'edit/:id',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('reservation:edit')]
    },
    {
        path: 'detail/:id',
        loadComponent: () => import('./detail/detail'),
        canActivate: [() => inject(Common).checkPermissionForRoute('reservation:view')]
    }
];

export default router;