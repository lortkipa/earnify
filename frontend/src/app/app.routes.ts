import { Routes } from '@angular/router';
import { Donate } from './components/donate/donate';
import { APP_NAME } from './globals';
import { Home } from './components/home/home';
import { Dashboard } from './components/dashboard/dashboard';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: Home,
        title: APP_NAME
    },
    {
        path: 'dashboard',
        component: Dashboard,
        title: APP_NAME
    },
    {
        path: 'donate',
        component: Donate,
        title: APP_NAME
    }
];
