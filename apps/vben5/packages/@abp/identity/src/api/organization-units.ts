import type { ListResultDto, PagedResultDto } from '@abp/core';

import type { IdentityRoleDto, IdentityUserDto } from '../types';
import type {
  GetIdentityRolesInput,
  GetIdentityUsersInput,
  GetOrganizationUnitPagedListInput,
  GetUnaddedRoleListInput,
  GetUnaddedUserListInput,
  OrganizationUnitAddRoleDto,
  OrganizationUnitAddUserDto,
  OrganizationUnitCreateDto,
  OrganizationUnitDto,
  OrganizationUnitGetChildrenDto,
  OrganizationUnitUpdateDto,
} from '../types/organization-units';

import { requestClient } from '@abp/request';

/**
 * 新增组织机构
 * @param input 参数
 * @returns 组织机构实体数据传输对象
 */
export function createApi(
  input: OrganizationUnitCreateDto,
): Promise<OrganizationUnitDto> {
  return requestClient.post<OrganizationUnitDto>(
    '/api/identity/organization-units',
    input,
  );
}

/**
 * 删除组织机构
 * @param id 组织机构id
 */
export function deleteApi(id: string): Promise<void> {
  return requestClient.delete(`/api/identity/organization-units/${id}`);
}

/**
 * 查询组织机构
 * @param id 组织机构id
 * @returns 组织机构实体数据传输对象
 */
export function getApi(id: string): Promise<OrganizationUnitDto> {
  return requestClient.get<OrganizationUnitDto>(
    `/api/identity/organization-units/${id}`,
  );
}

/**
 * 更新组织机构
 * @param id 组织机构id
 * @returns 组织机构实体数据传输对象
 */
export function updateApi(
  id: string,
  input: OrganizationUnitUpdateDto,
): Promise<OrganizationUnitDto> {
  return requestClient.put<OrganizationUnitDto>(
    `/api/identity/organization-units/${id}`,
    input,
  );
}

/**
 * 查询组织机构分页列表
 * @param input 过滤参数
 * @returns 组织机构实体数据传输对象分页列表
 */
export function getPagedListApi(
  input?: GetOrganizationUnitPagedListInput,
): Promise<PagedResultDto<OrganizationUnitDto>> {
  return requestClient.get<PagedResultDto<OrganizationUnitDto>>(
    `/api/identity/organization-units`,
    {
      params: input,
    },
  );
}

/**
 * 查询根组织机构列表
 * @returns 组织机构实体数据传输对象列表
 */
export function getRootListApi(): Promise<ListResultDto<OrganizationUnitDto>> {
  return requestClient.get<ListResultDto<OrganizationUnitDto>>(
    `/api/identity/organization-units/root-node`,
  );
}

/**
 * 查询组织机构列表
 * @returns 组织机构实体数据传输对象列表
 */
export function getAllListApi(): Promise<ListResultDto<OrganizationUnitDto>> {
  return requestClient.get<ListResultDto<OrganizationUnitDto>>(
    `/api/identity/organization-units/all`,
  );
}

/**
 * 查询下级组织机构列表
 * @param input 查询参数
 * @returns 组织机构实体数据传输对象列表
 */
export function getChildrenApi(
  input: OrganizationUnitGetChildrenDto,
): Promise<ListResultDto<OrganizationUnitDto>> {
  return requestClient.get<ListResultDto<OrganizationUnitDto>>(
    `/api/identity/organization-units/find-children`,
    {
      params: input,
    },
  );
}

/**
 * 查询组织机构用户列表
 * @param id 组织机构id
 * @param input 查询过滤参数
 * @returns 用户实体数据传输对象分页列表
 */
export function getUserListApi(
  id: string,
  input?: GetIdentityUsersInput,
): Promise<PagedResultDto<IdentityUserDto>> {
  return requestClient.get<PagedResultDto<IdentityUserDto>>(
    `/api/identity/organization-units/${id}/users`,
    {
      params: input,
    },
  );
}

/**
 * 查询未加入组织机构的用户列表
 * @param input 查询过滤参数
 * @returns 用户实体数据传输对象分页列表
 */
export function getUnaddedUserListApi(
  input: GetUnaddedUserListInput,
): Promise<PagedResultDto<IdentityUserDto>> {
  return requestClient.get<PagedResultDto<IdentityUserDto>>(
    `/api/identity/organization-units/${input.id}/unadded-users`,
    {
      params: input,
    },
  );
}

/**
 * 用户添加到组织机构
 * @param id 组织机构id
 * @param input 用户id列表
 */
export function addMembers(
  id: string,
  input: OrganizationUnitAddUserDto,
): Promise<void> {
  return requestClient.post(
    `/api/identity/organization-units/${id}/users`,
    input,
  );
}

/**
 * 查询组织机构角色列表
 * @param id 组织机构id
 * @param input 查询过滤参数
 * @returns 角色实体数据传输对象分页列表
 */
export function getRoleListApi(
  id: string,
  input?: GetIdentityRolesInput,
): Promise<PagedResultDto<IdentityRoleDto>> {
  return requestClient.get<PagedResultDto<IdentityRoleDto>>(
    `/api/identity/organization-units/${id}/roles`,
    {
      params: input,
    },
  );
}

/**
 * 查询未加入组织机构的角色列表
 * @param input 查询过滤参数
 * @returns 角色实体数据传输对象分页列表
 */
export function getUnaddedRoleListApi(
  input: GetUnaddedRoleListInput,
): Promise<PagedResultDto<IdentityRoleDto>> {
  return requestClient.get<PagedResultDto<IdentityRoleDto>>(
    `/api/identity/organization-units/${input.id}/unadded-roles`,
    {
      params: input,
    },
  );
}

/**
 * 角色添加到组织机构
 * @param id 组织机构id
 * @param input 角色id列表
 */
export function addRoles(
  id: string,
  input: OrganizationUnitAddRoleDto,
): Promise<void> {
  return requestClient.post(
    `/api/identity/organization-units/${id}/roles`,
    input,
  );
}

/**
 * 移动组织机构
 * @param id 组织机构id
 * @param parentId 父级组织机构id
 */
export function moveTo(id: string, parentId?: string): Promise<void> {
  return requestClient.put(`api/identity/organization-units/${id}/move`, {
    parentId,
  });
}
