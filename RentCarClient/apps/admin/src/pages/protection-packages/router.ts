import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./protection-packages'),
        canActivate: [() => inject(Common).checkPermissionForRoute('protection_package:view')]
    },
    {
        path: 'add',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('protection_package:create')]
    },
    {
        path: 'edit/:id',
        loadComponent: () => import('./create/create'),
        canActivate: [() => inject(Common).checkPermissionForRoute('protection_package:edit')]
    },
    {
        path: 'detail/:id',
        loadComponent: () => import('./detail/detail'),
        canActivate: [() => inject(Common).checkPermissionForRoute('protection_package:view')]
    }
];

export default router;