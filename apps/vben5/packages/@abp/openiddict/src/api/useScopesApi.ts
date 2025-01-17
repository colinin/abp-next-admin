import type { PagedResultDto } from '@abp/core';

import type {
  OpenIddictScopeCreateDto,
  OpenIddictScopeDto,
  OpenIddictScopeGetListInput,
  OpenIddictScopeUpdateDto,
} from '../types/scopes';

import { useRequest } from '@abp/request';

export function useScopesApi() {
  const { cancel, request } = useRequest();
  /**
   * 新增范围
   * @param input 参数
   * @returns 范围实体数据传输对象
   */
  function createApi(
    input: OpenIddictScopeCreateDto,
  ): Promise<OpenIddictScopeDto> {
    return request<OpenIddictScopeDto>('/api/openiddict/scopes', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 删除范围
   * @param id 范围id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/openiddict/scopes/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询范围
   * @param id 范围id
   * @returns 范围实体数据传输对象
   */
  function getApi(id: string): Promise<OpenIddictScopeDto> {
    return request<OpenIddictScopeDto>(`/api/openiddict/scopes/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 更新范围
   * @param id 范围id
   * @returns 范围实体数据传输对象
   */
  function updateApi(
    id: string,
    input: OpenIddictScopeUpdateDto,
  ): Promise<OpenIddictScopeDto> {
    return request<OpenIddictScopeDto>(`/api/openiddict/scopes/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 查询范围分页列表
   * @param input 过滤参数
   * @returns 范围实体数据传输对象分页列表
   */
  function getPagedListApi(
    input?: OpenIddictScopeGetListInput,
  ): Promise<PagedResultDto<OpenIddictScopeDto>> {
    return request<PagedResultDto<OpenIddictScopeDto>>(
      `/api/openiddict/scopes`,
      {
        method: 'GET',
        params: input,
      },
    );
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
