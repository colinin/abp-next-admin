import type { PagedResultDto } from '@abp/core';

import type {
  GetRolePagedListInput,
  IdentityRoleCreateDto,
  IdentityRoleDto,
  IdentityRoleUpdateDto,
} from '../types/roles';

import { requestClient } from '@abp/request';

/**
 * 新增角色
 * @param input 参数
 * @returns 角色实体数据传输对象
 */
export function createApi(
  input: IdentityRoleCreateDto,
): Promise<IdentityRoleDto> {
  return requestClient.post<IdentityRoleDto>('/api/identity/roles', input);
}

/**
 * 删除角色
 * @param id 角色id
 */
export function deleteApi(id: string): Promise<void> {
  return requestClient.delete(`/api/identity/roles/${id}`);
}

/**
 * 查询角色
 * @param id 角色id
 * @returns 角色实体数据传输对象
 */
export function getApi(id: string): Promise<IdentityRoleDto> {
  return requestClient.get<IdentityRoleDto>(`/api/identity/roles/${id}`);
}

/**
 * 更新角色
 * @param id 角色id
 * @returns 角色实体数据传输对象
 */
export function updateApi(
  id: string,
  input: IdentityRoleUpdateDto,
): Promise<IdentityRoleDto> {
  return requestClient.put<IdentityRoleDto>(`/api/identity/roles/${id}`, input);
}

/**
 * 查询角色分页列表
 * @param input 过滤参数
 * @returns 角色实体数据传输对象分页列表
 */
export function getPagedListApi(
  input?: GetRolePagedListInput,
): Promise<PagedResultDto<IdentityRoleDto>> {
  return requestClient.get<PagedResultDto<IdentityRoleDto>>(
    `/api/identity/roles`,
    {
      params: input,
    },
  );
}

/**
 * 从组织机构中移除角色
 * @param id 角色id
 * @param ouId 组织机构id
 */
export function removeOrganizationUnit(
  id: string,
  ouId: string,
): Promise<void> {
  return requestClient.delete(
    `/api/identity/roles/${id}/organization-units/${ouId}`,
  );
}
