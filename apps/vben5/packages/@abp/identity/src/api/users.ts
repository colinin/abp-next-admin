import type { PagedResultDto } from '@abp/core';

import type {
  GetUserPagedListInput,
  IdentityUserCreateDto,
  IdentityUserDto,
  IdentityUserUpdateDto,
} from '../types/users';

import { requestClient } from '@abp/request';

/**
 * 新增用户
 * @param input 参数
 * @returns 用户实体数据传输对象
 */
export function createApi(
  input: IdentityUserCreateDto,
): Promise<IdentityUserDto> {
  return requestClient.post<IdentityUserDto>('/api/identity/users', input);
}

/**
 * 删除用户
 * @param id 用户id
 */
export function deleteApi(id: string): Promise<void> {
  return requestClient.delete(`/api/identity/users/${id}`);
}

/**
 * 查询用户
 * @param id 用户id
 * @returns 用户实体数据传输对象
 */
export function getApi(id: string): Promise<IdentityUserDto> {
  return requestClient.get<IdentityUserDto>(`/api/identity/users/${id}`);
}

/**
 * 更新用户
 * @param id 用户id
 * @returns 用户实体数据传输对象
 */
export function updateApi(
  id: string,
  input: IdentityUserUpdateDto,
): Promise<IdentityUserDto> {
  return requestClient.put<IdentityUserDto>(`/api/identity/users/${id}`, input);
}

/**
 * 查询用户分页列表
 * @param input 过滤参数
 * @returns 用户实体数据传输对象分页列表
 */
export function getPagedListApi(
  input?: GetUserPagedListInput,
): Promise<PagedResultDto<IdentityUserDto>> {
  return requestClient.get<PagedResultDto<IdentityUserDto>>(
    `/api/identity/users`,
    {
      params: input,
    },
  );
}

/**
 * 从组织机构中移除用户
 * @param id 用户id
 * @param ouId 组织机构id
 */
export function removeOrganizationUnit(
  id: string,
  ouId: string,
): Promise<void> {
  return requestClient.delete(
    `/api/identity/users/${id}/organization-units/${ouId}`,
  );
}
