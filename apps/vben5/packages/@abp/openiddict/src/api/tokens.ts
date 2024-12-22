import type { PagedResultDto } from '@abp/core';

import type {
  OpenIddictTokenDto,
  OpenIddictTokenGetListInput,
} from '../types/tokens';

import { requestClient } from '@abp/request';

/**
 * 删除令牌
 * @param id 令牌id
 */
export function deleteApi(id: string): Promise<void> {
  return requestClient.delete(`/api/openiddict/tokens/${id}`);
}

/**
 * 查询令牌
 * @param id 令牌id
 * @returns 令牌实体数据传输对象
 */
export function getApi(id: string): Promise<OpenIddictTokenDto> {
  return requestClient.get<OpenIddictTokenDto>(`/api/openiddict/tokens/${id}`);
}

/**
 * 查询令牌分页列表
 * @param input 过滤参数
 * @returns 令牌实体数据传输对象分页列表
 */
export function getPagedListApi(
  input?: OpenIddictTokenGetListInput,
): Promise<PagedResultDto<OpenIddictTokenDto>> {
  return requestClient.get<PagedResultDto<OpenIddictTokenDto>>(
    `/api/openiddict/tokens`,
    {
      params: input,
    },
  );
}
