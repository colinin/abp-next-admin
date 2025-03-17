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

import { useRequest } from '@abp/request';

export function useUsersApi() {
  const { cancel, request } = useRequest();

  /**
   * 新增用户
   * @param input 参数
   * @returns 用户实体数据传输对象
   */
  function createApi(input: IdentityUserCreateDto): Promise<IdentityUserDto> {
    return request<IdentityUserDto>('/api/identity/users', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 删除用户
   * @param id 用户id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/identity/users/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询用户
   * @param id 用户id
   * @returns 用户实体数据传输对象
   */
  function getApi(id: string): Promise<IdentityUserDto> {
    return request<IdentityUserDto>(`/api/identity/users/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 更新用户
   * @param id 用户id
   * @returns 用户实体数据传输对象
   */
  function updateApi(
    id: string,
    input: IdentityUserUpdateDto,
  ): Promise<IdentityUserDto> {
    return request<IdentityUserDto>(`/api/identity/users/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 查询用户分页列表
   * @param input 过滤参数
   * @returns 用户实体数据传输对象分页列表
   */
  function getPagedListApi(
    input?: GetUserPagedListInput,
  ): Promise<PagedResultDto<IdentityUserDto>> {
    return request<PagedResultDto<IdentityUserDto>>(`/api/identity/users`, {
      method: 'GET',
      params: input,
    });
  }

  /**
   * 从组织机构中移除用户
   * @param id 用户id
   * @param ouId 组织机构id
   */
  function removeOrganizationUnitApi(id: string, ouId: string): Promise<void> {
    return request(`/api/identity/users/${id}/organization-units/${ouId}`, {
      method: 'DELETE',
    });
  }

  /**
   * 获取用户组织机构列表
   * @param id 用户id
   */
  function getOrganizationUnitsApi(
    id: string,
  ): Promise<ListResultDto<OrganizationUnitDto>> {
    return request<ListResultDto<OrganizationUnitDto>>(
      `/api/identity/users/${id}/organization-units`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 锁定用户
   * @param id 用户id
   * @param seconds 锁定时长(秒)
   */
  function lockApi(id: string, seconds: number): Promise<void> {
    return request(`/api/identity/users/${id}/lock/${seconds}`, {
      method: 'PUT',
    });
  }

  /**
   * 解锁用户
   * @param id 用户id
   */
  function unLockApi(id: string): Promise<void> {
    return request(`/api/identity/users/${id}/unlock`, {
      method: 'PUT',
    });
  }

  /**
   * 更改用户密码
   * @param id 用户id
   * @param input 密码变更dto
   */
  function changePasswordApi(
    id: string,
    input: ChangeUserPasswordInput,
  ): Promise<void> {
    return request(`/api/identity/users/change-password?id=${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 获取可用的角色列表
   */
  function getAssignableRolesApi(): Promise<ListResultDto<IdentityRoleDto>> {
    return request<ListResultDto<IdentityRoleDto>>(
      `/api/identity/users/assignable-roles`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 获取用户角色列表
   * @param id 用户id
   */
  function getRolesApi(id: string): Promise<ListResultDto<IdentityRoleDto>> {
    return request<ListResultDto<IdentityRoleDto>>(
      `/api/identity/users/${id}/roles`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 获取用户声明列表
   * @param id 用户id
   */
  function getClaimsApi(id: string): Promise<ListResultDto<IdentityClaimDto>> {
    return request<ListResultDto<IdentityClaimDto>>(
      `/api/identity/users/${id}/claims`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 删除用户声明
   * @param id 用户id
   * @param input 用户声明dto
   */
  function deleteClaimApi(
    id: string,
    input: IdentityClaimDeleteDto,
  ): Promise<void> {
    return request(`/api/identity/users/${id}/claims`, {
      method: 'DELETE',
      params: input,
    });
  }

  /**
   * 创建用户声明
   * @param id 用户id
   * @param input 用户声明dto
   */
  function createClaimApi(
    id: string,
    input: IdentityClaimCreateDto,
  ): Promise<void> {
    return request(`/api/identity/users/${id}/claims`, {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 更新用户声明
   * @param id 用户id
   * @param input 用户声明dto
   */
  function updateClaimApi(
    id: string,
    input: IdentityClaimUpdateDto,
  ): Promise<void> {
    return request(`/api/identity/users/${id}/claims`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    changePasswordApi,
    createApi,
    createClaimApi,
    deleteApi,
    deleteClaimApi,
    getApi,
    getAssignableRolesApi,
    getClaimsApi,
    getOrganizationUnitsApi,
    getPagedListApi,
    getRolesApi,
    lockApi,
    removeOrganizationUnitApi,
    unLockApi,
    updateApi,
    updateClaimApi,
  };
}
