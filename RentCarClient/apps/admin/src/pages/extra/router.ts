import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./extra'),
        canActivate: [() => inject(Common).checkPermissionForRoute('extra:view')]
    },
    {
        path: 'add',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('extra:create')]
    },
    {
        path: 'edit/:id',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('extra:edit')]
    },
    {
        path: 'detail/:id',
        loadComponent: () => import('./detail/detail'),
        canActivate: [() => inject(Common).checkPermissionForRoute('extra:view')]
    }
];

export default router;