import type { ListResultDto } from '@abp/core';

import type {
  MenuDto,
  MenuGetByRoleInput,
  SetRoleMenuInput,
  SetRoleMenuStartupInput,
} from '../types/menus';

import { useRequest } from '@abp/request';

export function useRoleMenusApi() {
  const { cancel, request } = useRequest();

  function getAllApi(
    input: MenuGetByRoleInput,
  ): Promise<ListResultDto<MenuDto>> {
    return request<ListResultDto<MenuDto>>(
      `/api/platform/menus/by-role/${input.role}/${input.framework}`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  function setMenusApi(input: SetRoleMenuInput): Promise<void> {
    return request('/api/platform/menus/by-role', {
      data: input,
      method: 'PUT',
    });
  }

  function setStartupMenuApi(
    meudId: string,
    input: SetRoleMenuStartupInput,
  ): Promise<void> {
    return request(`/api/platform/menus/startup/${meudId}/by-role`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    getAllApi,
    setMenusApi,
    setStartupMenuApi,
  };
}
