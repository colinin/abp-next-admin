import type { RouteMeta } from '/@/router/types';
export interface RouteItem {
  path: string;
  component: any;
  components?: any;
  meta: RouteMeta;
  name?: string;
  alias?: string | string[];
  redirect?: string;
  caseSensitive?: boolean;
  children?: RouteItem[];
  fullPath?: string;
  props?: any;
}

/**
 * @description: Get menu return value
 */
export type getMenuListResultModel = RouteItem[];
