import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./categories'),
        canActivate: [() => inject(Common).checkPermissionForRoute('category:view')]
    },
    {
        path: 'add',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('category:create')]
    },
    {
        path: 'edit/:id',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('category:edit')]
    },
    {
        path: 'detail/:id',
        loadComponent: () => import('./detail/detail'),
        canActivate: [() => inject(Common).checkPermissionForRoute('category:view')]
    }
];

export default router;