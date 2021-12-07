/**
The routing of this file will not show the layout.
It is an independent new page.
the contents of the file still need to log in to access
 */
import type { AppRouteModule } from '/@/router/types';

// test
// http:ip:port/main-out
export const mainOutRoutes: AppRouteModule[] = [
  {
    path: '/signin-callback',
    name: 'signin-callback',
    component: () => import('/@/views/sys/login/OidcSignInCallback.vue'),
    meta: {
      title: 'signin-callbackk',
      ignoreAuth: true,
      hideMenu: true,
      hideTab: true,
      hideBreadcrumb: true,
    },
  },
  {
    path: '/signout-callback',
    name: 'signout-callback',
    component: () => import('../../views/sys/login/OidcSignOutCallback.vue'),
    meta: {
      title: 'signout-callback',
      ignoreAuth: true,
      hideMenu: true,
      hideTab: true,
      hideBreadcrumb: true,
    },
  },
];

export const mainOutRouteNames = mainOutRoutes.map((item) => item.name);
