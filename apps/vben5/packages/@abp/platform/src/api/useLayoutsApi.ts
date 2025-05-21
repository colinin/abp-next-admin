import type { PagedResultDto } from '@abp/core';

import type {
  LayoutCreateDto,
  LayoutDto,
  LayoutGetPagedListInput,
  LayoutUpdateDto,
} from '../types/layouts';

import { useRequest } from '@abp/request';

export function useLayoutsApi() {
  const { cancel, request } = useRequest();

  function createApi(input: LayoutCreateDto): Promise<LayoutDto> {
    return request<LayoutDto>(`/api/platform/layouts`, {
      data: input,
      method: 'POST',
    });
  }

  function getPagedListApi(
    input?: LayoutGetPagedListInput,
  ): Promise<PagedResultDto<LayoutDto>> {
    return request<PagedResultDto<LayoutDto>>('/api/platform/layouts', {
      method: 'GET',
      params: input,
    });
  }

  function getApi(id: string): Promise<LayoutDto> {
    return request<LayoutDto>(`/api/platform/layouts/${id}`, {
      method: 'GET',
    });
  }

  function deleteApi(id: string): Promise<void> {
    return request(`/api/platform/layouts/${id}`, {
      method: 'DELETE',
    });
  }

  function updateApi(id: string, input: LayoutUpdateDto): Promise<LayoutDto> {
    return request<LayoutDto>(`/api/platform/layouts/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getPagedListApi,
    updateApi,
  };
}
