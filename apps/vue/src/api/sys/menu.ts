import { defAbpHttp } from '/@/utils/http/abp';
import { RouteItem } from './model/menuModel';
import { ListResultDto } from '../model/baseModel';

/**
 * @description: Get user menu based on id
 */

export const getMenuList = () => {
  return defAbpHttp.request<ListResultDto<RouteItem>>({
    service: 'Platform',
    controller: 'Menu',
    action: 'GetCurrentUserMenuListAsync',
    params: {
      input: {
        framework: 'Vue Vben Admin',
      },
    },
  });
};
