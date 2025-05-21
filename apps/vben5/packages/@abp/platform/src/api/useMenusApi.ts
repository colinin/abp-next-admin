import type { ListResultDto } from '@abp/core';

import type {
  MenuCreateDto,
  MenuDto,
  MenuGetAllInput,
  MenuUpdateDto,
} from '../types/menus';

import { useRequest } from '@abp/request';

export function useMenusApi() {
  const { cancel, request } = useRequest();

  function createApi(input: MenuCreateDto): Promise<MenuDto> {
    return request<MenuDto>(`/api/platform/menus`, {
      data: input,
      method: 'POST',
    });
  }

  function getAllApi(input?: MenuGetAllInput): Promise<ListResultDto<MenuDto>> {
    return request<ListResultDto<MenuDto>>('/api/platform/menus/all', {
      method: 'GET',
      params: input,
    });
  }

  function getApi(id: string): Promise<MenuDto> {
    return request<MenuDto>(`/api/platform/menus/${id}`, {
      method: 'GET',
    });
  }

  function deleteApi(id: string): Promise<void> {
    return request(`/api/platform/menus/${id}`, {
      method: 'DELETE',
    });
  }

  function updateApi(id: string, input: MenuUpdateDto): Promise<MenuDto> {
    return request<MenuDto>(`/api/platform/menus/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getAllApi,
    getApi,
    updateApi,
  };
}
