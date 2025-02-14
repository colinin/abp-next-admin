import type { PagedResultDto } from '@abp/core';

import type {
  OpenIddictApplicationCreateDto,
  OpenIddictApplicationDto,
  OpenIddictApplicationGetListInput,
  OpenIddictApplicationUpdateDto,
} from '../types/applications';

import { useRequest } from '@abp/request';

export function useApplicationsApi() {
  const { cancel, request } = useRequest();
  /**
   * 新增应用
   * @param input 参数
   * @returns 应用实体数据传输对象
   */
  function createApi(
    input: OpenIddictApplicationCreateDto,
  ): Promise<OpenIddictApplicationDto> {
    return request<OpenIddictApplicationDto>('/api/openiddict/applications', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 删除应用
   * @param id 应用id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/openiddict/applications/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询应用
   * @param id 应用id
   * @returns 应用实体数据传输对象
   */
  function getApi(id: string): Promise<OpenIddictApplicationDto> {
    return request<OpenIddictApplicationDto>(
      `/api/openiddict/applications/${id}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 更新应用
   * @param id 应用id
   * @returns 应用实体数据传输对象
   */
  function updateApi(
    id: string,
    input: OpenIddictApplicationUpdateDto,
  ): Promise<OpenIddictApplicationDto> {
    return request<OpenIddictApplicationDto>(
      `/api/openiddict/applications/${id}`,
      {
        data: input,
        method: 'PUT',
      },
    );
  }

  /**
   * 查询应用分页列表
   * @param input 过滤参数
   * @returns 应用实体数据传输对象分页列表
   */
  function getPagedListApi(
    input?: OpenIddictApplicationGetListInput,
  ): Promise<PagedResultDto<OpenIddictApplicationDto>> {
    return request<PagedResultDto<OpenIddictApplicationDto>>(
      `/api/openiddict/applications`,
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
