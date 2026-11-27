import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  EntityChangeDto,
  EntityChangeGetListInput,
  EntityChangeGetWithUsernameInput,
  EntityChangeWithUsernameDto,
} from '../types/entity-changes';

import { useRequest } from '@abp/request';
import qs from 'qs';

export function useEntityChangesApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取实体变更记录
   * @param id 记录id
   */
  function getApi(id: string): Promise<EntityChangeDto> {
    return request<EntityChangeDto>(`/api/auditing/entity-changes/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 导出实体变更列表
   * @param input 参数
   */
  function exportApi(input: EntityChangeGetListInput): Promise<Blob> {
    return request<Blob>('/api/auditing/entity-changes/export', {
      method: 'GET',
      params: input,
      responseType: 'blob',
    });
  }

  /**
   * 获取导出实体变更列表请求Url
   * @param input 参数
   */
  function getExportApiUrl(input: EntityChangeGetListInput) {
    const query = qs.stringify(input, { arrayFormat: 'brackets' });
    return `/api/auditing/entity-changes/export${query ? `?${query}` : ''}`;
  }

  /**
   * 获取实体变更分页列表
   * @param input 参数
   */
  function getPagedListApi(
    input: EntityChangeGetListInput,
  ): Promise<PagedResultDto<EntityChangeDto>> {
    return request<PagedResultDto<EntityChangeDto>>(
      '/api/auditing/entity-changes',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 获取包含用户名称的实体变更列表
   * @param input 参数
   */
  function getListWithUsernameApi(
    input: EntityChangeGetWithUsernameInput,
  ): Promise<ListResultDto<EntityChangeWithUsernameDto>> {
    return request<ListResultDto<EntityChangeWithUsernameDto>>(
      '/api/auditing/entity-changes/with-username',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  return {
    cancel,
    exportApi,
    getApi,
    getExportApiUrl,
    getListWithUsernameApi,
    getPagedListApi,
  };
}
