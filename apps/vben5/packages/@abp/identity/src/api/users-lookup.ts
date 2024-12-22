import type { ListResultDto } from '@abp/core';

import type {
  IdentityUserDto,
  UserLookupCountInput,
  UserLookupSearchInput,
} from '../types/users';

import { requestClient } from '@abp/request';

/**
 * 通过id查询用户
 * @param id 用户id
 * @returns 用户实体数据传输对象
 */
export function findByIdApi(id: string): Promise<IdentityUserDto> {
  return requestClient.get<IdentityUserDto>(`/api/identity/users/lookup/${id}`);
}

/**
 * 通过用户名查询用户
 * @param userName 用户名
 * @returns 用户实体数据传输对象
 */
export function findByUserNameApi(userName: string): Promise<IdentityUserDto> {
  return requestClient.get<IdentityUserDto>(
    `/api/identity/users/lookup/by-username/${userName}`,
  );
}

/**
 * 搜索用户列表
 * @param input 搜索过滤条件
 * @returns 用户实体数据传输对象列表
 */
export function searchApi(
  input?: UserLookupSearchInput,
): Promise<ListResultDto<IdentityUserDto>> {
  return requestClient.get<ListResultDto<IdentityUserDto>>(
    `/api/identity/users/lookup/search`,
    {
      params: input,
    },
  );
}

/**
 * 搜索用户数量
 * @param input 搜索过滤条件
 */
export function countApi(input?: UserLookupCountInput): Promise<number> {
  return requestClient.get<number>(`/api/identity/users/lookup/count`, {
    params: input,
  });
}
