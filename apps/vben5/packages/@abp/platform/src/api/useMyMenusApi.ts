import type { ListResultDto } from '@abp/core';

import type { MenuDto, MenuGetInput } from '../types/menus';

import { useRequest } from '@abp/request';

export function useMyMenusApi() {
  const { cancel, request } = useRequest();

  function getAllApi(input?: MenuGetInput): Promise<ListResultDto<MenuDto>> {
    return request<ListResultDto<MenuDto>>(
      '/api/platform/menus/by-current-user',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  return {
    cancel,
    getAllApi,
  };
}
