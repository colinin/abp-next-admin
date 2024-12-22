import type { PagedResultDto } from '@abp/core';

import type {
  OpenIddictAuthorizationDto,
  OpenIddictAuthorizationGetListInput,
} from '../types/authorizations';

import { requestClient } from '@abp/request';

/**
 * 删除授权
 * @param id 授权id
 */
export function deleteApi(id: string): Promise<void> {
  return requestClient.delete(`/api/openiddict/authorizations/${id}`);
}

/**
 * 查询授权
 * @param id 授权id
 * @returns 授权实体数据传输对象
 */
export function getApi(id: string): Promise<OpenIddictAuthorizationDto> {
  return requestClient.get<OpenIddictAuthorizationDto>(
    `/api/openiddict/authorizations/${id}`,
  );
}

/**
 * 查询授权分页列表
 * @param input 过滤参数
 * @returns 授权实体数据传输对象分页列表
 */
export function getPagedListApi(
  input?: OpenIddictAuthorizationGetListInput,
): Promise<PagedResultDto<OpenIddictAuthorizationDto>> {
  return requestClient.get<PagedResultDto<OpenIddictAuthorizationDto>>(
    `/api/openiddict/authorizations`,
    {
      params: input,
    },
  );
}
