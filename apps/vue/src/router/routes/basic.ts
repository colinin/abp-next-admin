import type { AppRouteRecordRaw } from '/@/router/types';
import { t } from '/@/hooks/web/useI18n';
import {
  REDIRECT_NAME,
  LAYOUT,
  EXCEPTION_COMPONENT,
  PAGE_NOT_FOUND_NAME,
} from '/@/router/constant';

// 404 on a page
export const PAGE_NOT_FOUND_ROUTE: AppRouteRecordRaw = {
  path: '/:path(.*)*',
  name: PAGE_NOT_FOUND_NAME,
  component: LAYOUT,
  meta: {
    title: 'ErrorPage',
    hideBreadcrumb: true,
    hideMenu: true,
  },
  children: [
    {
      path: '/:path(.*)*',
      name: PAGE_NOT_FOUND_NAME,
      component: EXCEPTION_COMPONENT,
      meta: {
        title: 'ErrorPage',
        hideBreadcrumb: true,
        hideMenu: true,
      },
    },
  ],
};

export const REDIRECT_ROUTE: AppRouteRecordRaw = {
  path: '/redirect',
  component: LAYOUT,
  name: 'RedirectTo',
  meta: {
    title: REDIRECT_NAME,
    hideBreadcrumb: true,
    hideMenu: true,
  },
  children: [
    {
      path: '/redirect/:path(.*)',
      name: REDIRECT_NAME,
      component: () => import('/@/views/sys/redirect/index.vue'),
      meta: {
        title: REDIRECT_NAME,
        hideBreadcrumb: true,
      },
    },
  ],
};

export const ERROR_LOG_ROUTE: AppRouteRecordRaw = {
  path: '/error-log',
  name: 'ErrorLog',
  component: LAYOUT,
  redirect: '/error-log/list',
  meta: {
    title: 'ErrorLog',
    hideBreadcrumb: true,
    hideChildrenInMenu: true,
  },
  children: [
    {
      path: 'list',
      name: 'ErrorLogList',
      component: () => import('/@/views/sys/error-log/index.vue'),
      meta: {
        title: t('routes.basic.errorLogList'),
        hideBreadcrumb: true,
        currentActiveMenu: '/error-log',
      },
    },
  ],
};

export const ACCOUNT_CENTER_ROUTE: AppRouteRecordRaw = {
  path: '/account',
  name: 'Account',
  component: LAYOUT,
  redirect: '/account/center',
  meta: {
    title: 'Account',
    hideMenu: true,
    ignoreAuth: true,
  },
  children: [
    {
      path: 'settings',
      name: 'ASettings',
      component: () => import('/@/views/account/setting/index.vue'),
      meta: {
        title: t('routes.basic.accountSetting'),
        hideMenu: true,
        ignoreAuth: true,
      },
    },
    {
      path: 'center',
      name: 'ACenter',
      component: () => import('/@/views/account/center/index.vue'),
      meta: {
        title: t('routes.basic.accountCenter'),
        hideMenu: true,
        ignoreAuth: true,
      },
    },
    {
      path: 'email-confirm',
      name: 'email-confirm',
      component: () => import('/@/views/account/email-confirm/index.vue'),
      meta: {
        title: t('routes.basic.accountEmailConfirm'),
        hideMenu: true,
        ignoreAuth: true,
      },
    },
  ],
};

