import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./branches'),
        canActivate: [() => inject(Common).checkPermissionForRoute('branch:view')]
    },
    {
        path: 'add',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('branch:create')]
    },
    {
        path: 'edit/:id',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('branch:edit')]
    },
    {
        path: 'detail/:id',
        loadComponent: () => import('./detail/detail'),
        canActivate: [() => inject(Common).checkPermissionForRoute('branch:view')]
    },
]

export default router;