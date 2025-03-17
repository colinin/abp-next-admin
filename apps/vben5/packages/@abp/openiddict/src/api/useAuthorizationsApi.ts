import type { PagedResultDto } from '@abp/core';

import type {
  OpenIddictAuthorizationDto,
  OpenIddictAuthorizationGetListInput,
} from '../types/authorizations';

import { useRequest } from '@abp/request';

export function useAuthorizationsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除授权
   * @param id 授权id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/openiddict/authorizations/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询授权
   * @param id 授权id
   * @returns 授权实体数据传输对象
   */
  function getApi(id: string): Promise<OpenIddictAuthorizationDto> {
    return request<OpenIddictAuthorizationDto>(
      `/api/openiddict/authorizations/${id}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 查询授权分页列表
   * @param input 过滤参数
   * @returns 授权实体数据传输对象分页列表
   */
  function getPagedListApi(
    input?: OpenIddictAuthorizationGetListInput,
  ): Promise<PagedResultDto<OpenIddictAuthorizationDto>> {
    return request<PagedResultDto<OpenIddictAuthorizationDto>>(
      `/api/openiddict/authorizations`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  return {
    cancel,
    deleteApi,
    getApi,
    getPagedListApi,
  };
}
