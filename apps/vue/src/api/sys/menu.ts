import { defHttp } from '/@/utils/http/axios';
import { RouteItem } from './model/menuModel';

/**
 * @description: Get user menu based on id
 */

export const getMenuList = () => {
  return defHttp.get<ListResultDto<RouteItem>>({
    url: `/api/platform/menus/by-current-user?framework=Vue Vben Admin`,
  });
};
