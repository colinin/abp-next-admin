import type { PagedResultDto } from '@abp/core';

import type {
  OpenIddictTokenDto,
  OpenIddictTokenGetListInput,
} from '../types/tokens';

import { useRequest } from '@abp/request';

export function useTokensApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除令牌
   * @param id 令牌id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/openiddict/tokens/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询令牌
   * @param id 令牌id
   * @returns 令牌实体数据传输对象
   */
  function getApi(id: string): Promise<OpenIddictTokenDto> {
    return request<OpenIddictTokenDto>(`/api/openiddict/tokens/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 查询令牌分页列表
   * @param input 过滤参数
   * @returns 令牌实体数据传输对象分页列表
   */
  function getPagedListApi(
    input?: OpenIddictTokenGetListInput,
  ): Promise<PagedResultDto<OpenIddictTokenDto>> {
    return request<PagedResultDto<OpenIddictTokenDto>>(
      `/api/openiddict/tokens`,
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
