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

import { useRequest } from '@abp/request';

export function useRolesApi() {
  const { cancel, request } = useRequest();
  /**
   * 新增角色
   * @param input 参数
   * @returns 角色实体数据传输对象
   */
  function createApi(input: IdentityRoleCreateDto): Promise<IdentityRoleDto> {
    return request<IdentityRoleDto>('/api/identity/roles', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 删除角色
   * @param id 角色id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/identity/roles/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询角色
   * @param id 角色id
   * @returns 角色实体数据传输对象
   */
  function getApi(id: string): Promise<IdentityRoleDto> {
    return request<IdentityRoleDto>(`/api/identity/roles/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 更新角色
   * @param id 角色id
   * @returns 角色实体数据传输对象
   */
  function updateApi(
    id: string,
    input: IdentityRoleUpdateDto,
  ): Promise<IdentityRoleDto> {
    return request<IdentityRoleDto>(`/api/identity/roles/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 查询角色分页列表
   * @param input 过滤参数
   * @returns 角色实体数据传输对象分页列表
   */
  function getPagedListApi(
    input?: GetRolePagedListInput,
  ): Promise<PagedResultDto<IdentityRoleDto>> {
    return request<PagedResultDto<IdentityRoleDto>>(`/api/identity/roles`, {
      method: 'GET',
      params: input,
    });
  }

  /**
   * 从组织机构中移除角色
   * @param id 角色id
   * @param ouId 组织机构id
   */
  function removeOrganizationUnitApi(id: string, ouId: string): Promise<void> {
    return request(`/api/identity/roles/${id}/organization-units/${ouId}`, {
      method: 'DELETE',
    });
  }

  /**
   * 获取角色声明列表
   * @param id 角色id
   */
  function getClaimsApi(id: string): Promise<ListResultDto<IdentityClaimDto>> {
    return request<ListResultDto<IdentityClaimDto>>(
      `/api/identity/roles/${id}/claims`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 删除角色声明
   * @param id 角色id
   * @param input 角色声明dto
   */
  function deleteClaimApi(
    id: string,
    input: IdentityClaimDeleteDto,
  ): Promise<void> {
    return request(`/api/identity/roles/${id}/claims`, {
      method: 'DELETE',
      params: input,
    });
  }

  /**
   * 创建角色声明
   * @param id 角色id
   * @param input 角色声明dto
   */
  function createClaimApi(
    id: string,
    input: IdentityClaimCreateDto,
  ): Promise<void> {
    return request(`/api/identity/roles/${id}/claims`, {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 更新角色声明
   * @param id 角色id
   * @param input 用户角色dto
   */
  function updateClaimApi(
    id: string,
    input: IdentityClaimUpdateDto,
  ): Promise<void> {
    return request(`/api/identity/roles/${id}/claims`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    createClaimApi,
    deleteApi,
    deleteClaimApi,
    getApi,
    getClaimsApi,
    getPagedListApi,
    removeOrganizationUnitApi,
    updateApi,
    updateClaimApi,
  };
}
