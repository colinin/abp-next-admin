import type { RouteMeta } from 'vue-router';
export interface RouteItem {
  path: string;
  component: any;
  meta: RouteMeta;
  name?: string;
  alias?: string | string[];
  redirect?: string;
  caseSensitive?: boolean;
  children?: RouteItem[];
  fullPath?: string;
  props?: any;
  startup: boolean;
}

/**
 * @description: Get menu return value
 */
export type getMenuListResultModel = RouteItem[];
