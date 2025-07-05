import { Routes } from "@angular/router";

const router: Routes = [
    {
        path: '',
        loadComponent: () => import('./branches')
    },
    {
        path: 'add',
        loadComponent: () => import('./create/create')
    },
    {
        path: 'edit/:id',
        loadComponent: () => import('./create/create')
    }
]

export default router;