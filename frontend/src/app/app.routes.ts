import { Routes } from '@angular/router';
import { Donate } from './components/donate/donate';
import { APP_NAME } from './globals';
import { Home } from './components/home/home';
import { Dashboard } from './components/dashboard/dashboard';
import { Transactions } from './components/dashboard/transactions/transactions';
import { Overview } from './components/dashboard/overview/overview';
import { Links } from './components/dashboard/links/links';
import { Settings } from './components/dashboard/settings/settings';

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
        title: APP_NAME,

        children: [
            {
                path: '',
                redirectTo: 'overview',
                pathMatch: 'full'
            },
            {
                path: 'overview',
                component: Overview,
                title: APP_NAME
            },
            {
                path: 'links',
                component: Links,
                title: APP_NAME
            },
            {
                path: 'transactions',
                component: Transactions,
                title: APP_NAME
            },
             {
                path: 'settings',
                component: Settings,
                title: APP_NAME
            },
        ]
    },
    {
        path: 'donate/:id',
        component: Donate,
        title: APP_NAME
    }
];
