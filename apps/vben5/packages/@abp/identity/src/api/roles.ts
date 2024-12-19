import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  IdentityClaimCreateDto,
  IdentityClaimDeleteDto,
  IdentityClaimDto,
  IdentityClaimUpdateDto,
} from '../types/claims';
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
export function removeOrganizationUnitApi(
  id: string,
  ouId: string,
): Promise<void> {
  return requestClient.delete(
    `/api/identity/roles/${id}/organization-units/${ouId}`,
  );
}

/**
 * 获取角色声明列表
 * @param id 角色id
 */
export function getClaimsApi(
  id: string,
): Promise<ListResultDto<IdentityClaimDto>> {
  return requestClient.get<ListResultDto<IdentityClaimDto>>(
    `/api/identity/roles/${id}/claims`,
  );
}

/**
 * 删除角色声明
 * @param id 角色id
 * @param input 角色声明dto
 */
export function deleteClaimApi(
  id: string,
  input: IdentityClaimDeleteDto,
): Promise<void> {
  return requestClient.delete(`/api/identity/roles/${id}/claims`, {
    params: input,
  });
}

/**
 * 创建角色声明
 * @param id 角色id
 * @param input 角色声明dto
 */
export function createClaimApi(
  id: string,
  input: IdentityClaimCreateDto,
): Promise<void> {
  return requestClient.post(`/api/identity/roles/${id}/claims`, input);
}

/**
 * 更新角色声明
 * @param id 角色id
 * @param input 用户角色dto
 */
export function updateClaimApi(
  id: string,
  input: IdentityClaimUpdateDto,
): Promise<void> {
  return requestClient.put(`/api/identity/roles/${id}/claims`, input);
}
