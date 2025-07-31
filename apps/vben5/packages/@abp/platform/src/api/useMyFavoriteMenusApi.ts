import type { ListResultDto } from '@abp/core';

import type { UserFavoriteMenuDto } from '../types/favorites';

import { useRequest } from '@abp/request';

export function useMyFavoriteMenusApi() {
  const { cancel, request } = useRequest();

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
    getListApi,
  };
}
