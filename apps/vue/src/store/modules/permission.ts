import type { AppRouteRecordRaw, Menu, RouteMeta } from '/@/router/types';

import { defineStore } from 'pinia';
import { store } from '/@/store';
import { useI18n } from '/@/hooks/web/useI18n';
import { useUserStore } from './user';
import { useAbpStoreWithOut } from './abp';
import { useAppStoreWithOut } from './app';
import { toRaw } from 'vue';
import { transformObjToRoute, flatMultiLevelRoutes } from '/@/router/helper/routeHelper';
import { transformRouteToMenu, generateTree, mapMetaBoolean } from '/@/router/helper/menuHelper';

import projectSetting from '/@/settings/projectSetting';

import { PermissionModeEnum } from '/@/enums/appEnum';

import { asyncRoutes } from '/@/router/routes';
import { ERROR_LOG_ROUTE, PAGE_NOT_FOUND_ROUTE } from '/@/router/routes/basic';

import { filter } from '/@/utils/helper/treeHelper';

import { getMenuList } from '/@/api/sys/menu';

import { useMessage } from '/@/hooks/web/useMessage';
import { RouteItem } from '/@/api/sys/model/menuModel';
import { PageEnum } from '/@/enums/pageEnum';

interface PermissionState {
  // Permission code list
  permCodeList: string[] | number[];
  // Whether the route has been dynamically added
  isDynamicAddedRoute: boolean;
  // To trigger a menu update
  lastBuildMenuTime: number;
  // Backstage menu list
  backMenuList: Menu[];
  frontMenuList: Menu[];
}
export const usePermissionStore = defineStore({
  id: 'app-permission',
  state: (): PermissionState => ({
    permCodeList: [],
    // Whether the route has been dynamically added
    isDynamicAddedRoute: false,
    // To trigger a menu update
    lastBuildMenuTime: 0,
    // Backstage menu list
    backMenuList: [],
    // menu List
    frontMenuList: [],
  }),
  getters: {
    getPermCodeList(): string[] | number[] {
      return this.permCodeList;
    },
    getBackMenuList(): Menu[] {
      return this.backMenuList;
    },
    getFrontMenuList(): Menu[] {
      return this.frontMenuList;
    },
    getLastBuildMenuTime(): number {
      return this.lastBuildMenuTime;
    },
    getIsDynamicAddedRoute(): boolean {
      return this.isDynamicAddedRoute;
    },
  },
  actions: {
    setPermCodeList(codeList: string[]) {
      this.permCodeList = codeList;
    },

    setBackMenuList(list: Menu[]) {
      this.backMenuList = list;
      list?.length > 0 && this.setLastBuildMenuTime();
    },

    setFrontMenuList(list: Menu[]) {
      this.frontMenuList = list;
    },

    setLastBuildMenuTime() {
      this.lastBuildMenuTime = new Date().getTime();
    },

    setDynamicAddedRoute(added: boolean) {
      this.isDynamicAddedRoute = added;
    },
    resetState(): void {
      this.isDynamicAddedRoute = false;
      this.permCodeList = [];
      this.backMenuList = [];
      this.lastBuildMenuTime = 0;
    },
    async changePermissionCode() {
      const abpStore = useAbpStoreWithOut();
      const grantedPolicies = abpStore.getApplication.auth.grantedPolicies;
      const authPermissions = new Array<string>();
      if (grantedPolicies) {
        Object.keys(grantedPolicies).forEach((key) => {
          if (grantedPolicies[key]) {
            authPermissions.push(key);
          }
        });
      }
      this.setPermCodeList(authPermissions);
    },
    async buildRoutesAction(): Promise<AppRouteRecordRaw[]> {
      const { t } = useI18n();
      const userStore = useUserStore();
      const appStore = useAppStoreWithOut();

      let routes: AppRouteRecordRaw[] = [];
      const roleList = toRaw(userStore.getRoleList) || [];
      const { permissionMode = projectSetting.permissionMode } = appStore.getProjectConfig;

      const routeFilter = (route: AppRouteRecordRaw) => {
        const { meta } = route;
        const { roles } = meta || {};
        if (!roles) return true;
        return roleList.some((role) => roles.includes(role));
      };

      const routeRemoveIgnoreFilter = (route: AppRouteRecordRaw) => {
        const { meta } = route;
        const { ignoreRoute } = meta || {};
        return !ignoreRoute;
      };

      /**
       * @description 根据设置的首页path，修正routes中的affix标记（固定首页）
       * */
      const patchHomeAffix = (routes: AppRouteRecordRaw[]) => {
        if (!routes || routes.length === 0) return;
        let homePath: string = userStore.getUserInfo.homePath || PageEnum.BASE_HOME;
        function patcher(routes: AppRouteRecordRaw[], parentPath = '') {
          if (parentPath) parentPath = parentPath + '/';
          routes.forEach((route: AppRouteRecordRaw) => {
            const { path, children, redirect } = route;
            const currentPath = path.startsWith('/') ? path : parentPath + path;
            if (currentPath === homePath) {
              if (redirect) {
                homePath = route.redirect! as string;
              } else {
                route.meta = Object.assign({}, route.meta, { affix: true });
                throw new Error('end');
              }
            }
            children && children.length > 0 && patcher(children, currentPath);
          });
        }
        try {
          patcher(routes);
        } catch (e) {
          // 已处理完毕跳出循环
        }
        return;
      };

      switch (permissionMode) {
        case PermissionModeEnum.ROLE:
          routes = filter(asyncRoutes, routeFilter);
          routes = routes.filter(routeFilter);
          // Convert multi-level routing to level 2 routing
          routes = flatMultiLevelRoutes(routes);
          break;

        case PermissionModeEnum.ROUTE_MAPPING:
          routes = filter(asyncRoutes, routeFilter);
          routes = routes.filter(routeFilter);
          const menuList = transformRouteToMenu(routes, true);
          routes = filter(routes, routeRemoveIgnoreFilter);
          routes = routes.filter(routeRemoveIgnoreFilter);
          menuList.sort((a, b) => {
            return (a.meta?.orderNo || 0) - (b.meta?.orderNo || 0);
          });

          this.setFrontMenuList(menuList);
          // Convert multi-level routing to level 2 routing
          routes = flatMultiLevelRoutes(routes);
          break;

        //  If you are sure that you do not need to do background dynamic permissions, please comment the entire judgment below
        case PermissionModeEnum.BACK:
          const { createMessage } = useMessage();

          createMessage.loading({
            content: t('sys.app.menuLoading'),
            duration: 1,
          });

          // !Simulate to obtain permission codes from the background,
          // this function may only need to be executed once, and the actual project can be put at the right time by itself
          let routeList: AppRouteRecordRaw[] = [];
          try {
            this.changePermissionCode();
            const menuResult = await getMenuList();
            const menuList = generateTree(menuResult.items) as RouteItem[];
            routeList = this.filterDynamicRoutes(menuList);
          } catch (error) {
            console.error(error);
          }

          // Dynamically introduce components
          routeList = transformObjToRoute(routeList);

          //  Background routing to menu structure
          const backMenuList = transformRouteToMenu(routeList);
          this.setBackMenuList(backMenuList);

          // remove meta.ignoreRoute item
          routeList = filter(routeList, routeRemoveIgnoreFilter);
          routeList = routeList.filter(routeRemoveIgnoreFilter);

          routeList = flatMultiLevelRoutes(routeList);
          routes = [PAGE_NOT_FOUND_ROUTE, ...routeList];
          break;
      }

      routes.push(ERROR_LOG_ROUTE);
      patchHomeAffix(routes);
      return routes;
    },
    filterDynamicRoutes(menus: RouteItem[]) {
      const routeList: AppRouteRecordRaw[] = [];
      menus.forEach((menu) => {
        if (!this.validationFeatures(menu.meta)) {
          return;
        }
        const r: AppRouteRecordRaw = {
          path: menu.path,
          name: menu.name!,
          redirect: menu.redirect,
          component: menu.component,
          meta: {
            affix: mapMetaBoolean('affix', menu.meta),
            title: menu.meta.title,
            icon: menu.meta.icon,
            ignoreAuth: mapMetaBoolean('ignoreAuth', menu.meta),
            ignoreKeepAlive: mapMetaBoolean('ignoreKeepAlive', menu.meta),
            frameSrc: menu.meta.frameSrc,
            frameFormat: menu.meta.frameFormat,
            transitionName: menu.meta.transitionName,
            hideBreadcrumb: mapMetaBoolean('hideBreadcrumb', menu.meta),
            hideChildrenInMenu: mapMetaBoolean('hideChildrenInMenu', menu.meta),
            carryParam: mapMetaBoolean('carryParam', menu.meta),
            single: mapMetaBoolean('single', menu.meta),
            currentActiveMenu: menu.meta.currentActiveMenu,
            hideTab: mapMetaBoolean('hideTab', menu.meta),
            hideMenu: mapMetaBoolean('hideMenu', menu.meta),
            isLink: mapMetaBoolean('isLink', menu.meta),
            roles: menu.meta.roles,
          },
        };
        if (menu.children) {
          r.children = this.filterDynamicRoutes(menu.children);
        }
        routeList.push(r);
      });

      return routeList;
    },
    /** 验证功能 */
    validationFeatures(meta: RouteMeta) {
      // 如果声明了必须某些功能而系统为启用此功能,则不加入路由中
      if (meta.requiredFeatures) {
        let featureHasEnabled = true;
        const abpStore = useAbpStoreWithOut();
        const { features } = abpStore.getApplication;
        const definedFeatures = features.values;
        if (definedFeatures === undefined) {
          return featureHasEnabled;
        }
        let requiredFeatures = meta.requiredFeatures;
        if (!Array.isArray(requiredFeatures)) {
          requiredFeatures = requiredFeatures.split(',');
        }
        for (const i in requiredFeatures) {
          if (definedFeatures[requiredFeatures[i]] === 'false') {
            featureHasEnabled = false;
            continue;
          }
        }
        return featureHasEnabled;
      }
      return true;
    },
  },
});

// Need to be used outside the setup
export function usePermissionStoreWithOut() {
  return usePermissionStore(store);
}
