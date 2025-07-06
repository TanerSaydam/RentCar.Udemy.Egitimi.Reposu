import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./users'),
        canActivate: [() => inject(Common).checkPermissionForRoute('user:view')]
    },
    // {
    //     path: 'add',
    //     loadComponent: () => import('./create/create'),
    //     canActivate: [() => inject(Common).checkPermissionForRoute('user:create')]
    // },
    // {
    //     path: 'edit/:id',
    //     loadComponent: () => import('./create/create'),
    //     canActivate: [() => inject(Common).checkPermissionForRoute('user:edit')]
    // },
    // {
    //     path: 'detail/:id',
    //     loadComponent: () => import('./detail/detail'),
    //     canActivate: [() => inject(Common).checkPermissionForRoute('user:view')]
    // }
]

export default router;