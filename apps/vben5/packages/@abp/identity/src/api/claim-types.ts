import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  GetIdentityClaimTypePagedListInput,
  IdentityClaimTypeCreateDto,
  IdentityClaimTypeDto,
  IdentityClaimTypeUpdateDto,
} from '../types/claim-types';

import { requestClient } from '@abp/request';

/**
 * 新增用户声明
 * @param input 参数
 * @returns 用户声明实体数据传输对象
 */
export function createApi(
  input: IdentityClaimTypeCreateDto,
): Promise<IdentityClaimTypeDto> {
  return requestClient.post<IdentityClaimTypeDto>(
    '/api/identity/claim-types',
    input,
  );
}

/**
 * 删除用户声明
 * @param id 用户声明id
 */
export function deleteApi(id: string): Promise<void> {
  return requestClient.delete(`/api/identity/claim-types/${id}`);
}

/**
 * 查询用户声明
 * @param id 用户声明id
 * @returns 用户声明实体数据传输对象
 */
export function getApi(id: string): Promise<IdentityClaimTypeDto> {
  return requestClient.get<IdentityClaimTypeDto>(
    `/api/identity/claim-types/${id}`,
  );
}

/**
 * 更新用户声明
 * @param id 用户声明id
 * @returns 用户声明实体数据传输对象
 */
export function updateApi(
  id: string,
  input: IdentityClaimTypeUpdateDto,
): Promise<IdentityClaimTypeDto> {
  return requestClient.put<IdentityClaimTypeDto>(
    `/api/identity/claim-types/${id}`,
    input,
  );
}

/**
 * 查询用户声明分页列表
 * @param input 过滤参数
 * @returns 用户声明实体数据传输对象分页列表
 */
export function getPagedListApi(
  input?: GetIdentityClaimTypePagedListInput,
): Promise<PagedResultDto<IdentityClaimTypeDto>> {
  return requestClient.get<PagedResultDto<IdentityClaimTypeDto>>(
    `/api/identity/claim-types`,
    {
      params: input,
    },
  );
}

/**
 * 获取可用的声明类型列表
 */
export function getAssignableClaimsApi(): Promise<
  ListResultDto<IdentityClaimTypeDto>
> {
  return requestClient.get<ListResultDto<IdentityClaimTypeDto>>(
    `/api/identity/claim-types/actived-list`,
  );
}
