import { inject } from "@angular/core";
import { Routes } from "@angular/router";
import { Common } from "../../services/common";

const router: Routes = [
  {
    path: '',
    loadComponent: () => import('./customers'),
    canActivate: [() => inject(Common).checkPermissionForRoute('customer:view')]
  },
  {
    path: 'add',
    loadComponent: () => import('./create/create'),
    canActivate: [() => inject(Common).checkPermissionForRoute('customer:create')]
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./create/create'),
    canActivate: [() => inject(Common).checkPermissionForRoute('customer:edit')]
  },
  {
    path: 'detail/:id',
    loadComponent: () => import('./detail/detail'),
    canActivate: [() => inject(Common).checkPermissionForRoute('customer:view')]
  }
];

export default router;