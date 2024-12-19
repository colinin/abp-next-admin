import type { ListResultDto, PagedResultDto } from '@abp/core';

import type { IdentityRoleDto, OrganizationUnitDto } from '../types';
import type {
  IdentityClaimCreateDto,
  IdentityClaimDeleteDto,
  IdentityClaimDto,
  IdentityClaimUpdateDto,
} from '../types/claims';
import type {
  ChangeUserPasswordInput,
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
export function removeOrganizationUnitApi(
  id: string,
  ouId: string,
): Promise<void> {
  return requestClient.delete(
    `/api/identity/users/${id}/organization-units/${ouId}`,
  );
}

/**
 * 获取用户组织机构列表
 * @param id 用户id
 */
export function getOrganizationUnitsApi(
  id: string,
): Promise<ListResultDto<OrganizationUnitDto>> {
  return requestClient.get<ListResultDto<OrganizationUnitDto>>(
    `/api/identity/users/${id}/organization-units`,
  );
}

/**
 * 锁定用户
 * @param id 用户id
 * @param seconds 锁定时长(秒)
 */
export function lockApi(id: string, seconds: number): Promise<void> {
  return requestClient.put(`/api/identity/users/${id}/lock/${seconds}`);
}

/**
 * 解锁用户
 * @param id 用户id
 */
export function unLockApi(id: string): Promise<void> {
  return requestClient.put(`/api/identity/users/${id}/unlock`);
}

/**
 * 更改用户密码
 * @param id 用户id
 * @param input 密码变更dto
 */
export function changePasswordApi(
  id: string,
  input: ChangeUserPasswordInput,
): Promise<void> {
  return requestClient.put(
    `/api/identity/users/change-password?id=${id}`,
    input,
  );
}

/**
 * 获取可用的角色列表
 */
export function getAssignableRolesApi(): Promise<
  ListResultDto<IdentityRoleDto>
> {
  return requestClient.get<ListResultDto<IdentityRoleDto>>(
    `/api/identity/users/assignable-roles`,
  );
}

/**
 * 获取用户角色列表
 * @param id 用户id
 */
export function getRolesApi(
  id: string,
): Promise<ListResultDto<IdentityRoleDto>> {
  return requestClient.get<ListResultDto<IdentityRoleDto>>(
    `/api/identity/users/${id}/roles`,
  );
}

/**
 * 获取用户声明列表
 * @param id 用户id
 */
export function getClaimsApi(
  id: string,
): Promise<ListResultDto<IdentityClaimDto>> {
  return requestClient.get<ListResultDto<IdentityClaimDto>>(
    `/api/identity/users/${id}/claims`,
  );
}

/**
 * 删除用户声明
 * @param id 用户id
 * @param input 用户声明dto
 */
export function deleteClaimApi(
  id: string,
  input: IdentityClaimDeleteDto,
): Promise<void> {
  return requestClient.delete(`/api/identity/users/${id}/claims`, {
    params: input,
  });
}

/**
 * 创建用户声明
 * @param id 用户id
 * @param input 用户声明dto
 */
export function createClaimApi(
  id: string,
  input: IdentityClaimCreateDto,
): Promise<void> {
  return requestClient.post(`/api/identity/users/${id}/claims`, input);
}

/**
 * 更新用户声明
 * @param id 用户id
 * @param input 用户声明dto
 */
export function updateClaimApi(
  id: string,
  input: IdentityClaimUpdateDto,
): Promise<void> {
  return requestClient.put(`/api/identity/users/${id}/claims`, input);
}
