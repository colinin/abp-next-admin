import type { ListResultDto } from '@abp/core';

import type {
  UserFavoriteMenuCreateDto,
  UserFavoriteMenuDto,
} from '../types/favorites';

import { useRequest } from '@abp/request';

export function useMyFavoriteMenusApi() {
  const { cancel, request } = useRequest();

  /**
   * 新增常用菜单
   * @param input 参数
   * @returns 常用菜单
   */
  function createApi(input: UserFavoriteMenuCreateDto) {
    return request<UserFavoriteMenuDto>(
      `/api/platform/menus/favorites/my-favorite-menus`,
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 获取常用菜单列表
   * @param framework ui框架
   * @returns 菜单列表
   */
  function getListApi(
    framework?: string,
  ): Promise<ListResultDto<UserFavoriteMenuDto>> {
    return request<ListResultDto<UserFavoriteMenuDto>>(
      `/api/platform/menus/favorites/my-favorite-menus?framework=${framework}`,
      {
        method: 'GET',
      },
    );
  }

  return {
    cancel,
    createApi,
    getListApi,
  };
}
