import type { RouteRecordStringComponent } from '@vben/types';

import type { MenuDto } from '../types';

import { listToTree } from '@abp/core';

export function useMenuTransform() {
  function mapMetaString(meta: Record<string, any>, key: string) {
    if (!meta[key]) {
      return undefined;
    }
    return typeof meta[key] === 'string' ? meta[key] : String(meta[key]);
  }
  function mapMetaNumber(meta: Record<string, any>, key: string) {
    if (!meta[key]) {
      return undefined;
    }
    return typeof meta[key] === 'number' ? meta[key] : Number(meta[key]);
  }
  function mapMetaBoolean(meta: Record<string, any>, key: string) {
    if (!meta[key]) {
      return undefined;
    }
    return typeof meta[key] === 'boolean' ? meta[key] : meta[key] === 'true';
  }
  function mapMetaArray(meta: Record<string, any>, key: string) {
    if (!meta[key]) {
      return undefined;
    }
    return Array.isArray(meta[key]) ? meta[key] : String(meta[key]).split(',');
  }
  function transformRoutes(menus: MenuDto[]): RouteRecordStringComponent[] {
    const combMenus = menus.map((item) => {
      return {
        component: item.component.includes('BasicLayout')
          ? undefined
          : item.component,
        id: item.id,
        meta: {
          activeIcon: mapMetaString(item.meta, 'activeIcon'),
          activePath: mapMetaString(item.meta, 'activePath'),
          affixTab: mapMetaBoolean(item.meta, 'affixTab'),
          affixTabOrder: mapMetaNumber(item.meta, 'affixTabOrder'),
          authority: mapMetaArray(item.meta, 'authority'),
          badge: mapMetaString(item.meta, 'badge'),
          badgeType: mapMetaString(item.meta, 'badgeType'),
          badgeVariants: mapMetaString(item.meta, 'badgeVariants'),
          hideChildrenInMenu: mapMetaBoolean(item.meta, 'hideChildrenInMenu'),
          hideInBreadcrumb: mapMetaBoolean(item.meta, 'hideInBreadcrumb'),
          hideInMenu: mapMetaBoolean(item.meta, 'hideInMenu'),
          hideInTab: mapMetaBoolean(item.meta, 'hideInTab'),
          icon: mapMetaString(item.meta, 'icon'),
          iframeSrc: mapMetaString(item.meta, 'iframeSrc'),
          ignoreAccess: mapMetaBoolean(item.meta, 'ignoreAccess'),
          keepAlive: mapMetaBoolean(item.meta, 'keepAlive'),
          link: mapMetaString(item.meta, 'link'),
          maxNumOfOpenTab: mapMetaNumber(item.meta, 'maxNumOfOpenTab'),
          menuVisibleWithForbidden: mapMetaBoolean(
            item.meta,
            'menuVisibleWithForbidden',
          ),
          noBasicLayout: mapMetaBoolean(item.meta, 'noBasicLayout'),
          openInNewWindow: mapMetaBoolean(item.meta, 'openInNewWindow'),
          order: mapMetaNumber(item.meta, 'order'),
          title: item.meta.title ?? item.displayName,
        },
        name: item.name,
        parentId: item.parentId,
        path: item.path,
        redirect: item.redirect,
      };
    });
    const routes = listToTree(combMenus, {
      id: 'id',
      pid: 'parentId',
      children: 'children',
    });

    return routes;
  }

  return {
    transformRoutes,
  };
}
