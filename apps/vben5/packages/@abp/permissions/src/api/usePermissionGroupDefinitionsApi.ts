import type { ListResultDto } from '@abp/core';

import type {
  PermissionGroupDefinitionCreateDto,
  PermissionGroupDefinitionDto,
  PermissionGroupDefinitionGetListInput,
  PermissionGroupDefinitionUpdateDto,
} from '../types/groups';

import { useRequest } from '@abp/request';

export function usePermissionGroupDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除权限定义
   * @param name 权限名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/permission-management/definitions/groups/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询权限定义
   * @param name 权限名称
   * @returns 权限定义数据传输对象
   */
  function getApi(name: string): Promise<PermissionGroupDefinitionDto> {
    return request<PermissionGroupDefinitionDto>(
      `/api/permission-management/definitions/groups/${name}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 查询权限定义列表
   * @param input 权限过滤条件
   * @returns 权限定义数据传输对象列表
   */
  function getListApi(
    input?: PermissionGroupDefinitionGetListInput,
  ): Promise<ListResultDto<PermissionGroupDefinitionDto>> {
    return request<ListResultDto<PermissionGroupDefinitionDto>>(
      `/api/permission-management/definitions/groups`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 创建权限定义
   * @param input 权限定义参数
   * @returns 权限定义数据传输对象
   */
  function createApi(
    input: PermissionGroupDefinitionCreateDto,
  ): Promise<PermissionGroupDefinitionDto> {
    return request<PermissionGroupDefinitionDto>(
      '/api/permission-management/definitions/groups',
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 更新权限定义
   * @param name 权限名称
   * @param input 权限定义参数
   * @returns 权限定义数据传输对象
   */
  function updateApi(
    name: string,
    input: PermissionGroupDefinitionUpdateDto,
  ): Promise<PermissionGroupDefinitionDto> {
    return request<PermissionGroupDefinitionDto>(
      `/api/permission-management/definitions/groups/${name}`,
      {
        data: input,
        method: 'PUT',
      },
    );
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getListApi,
    updateApi,
  };
}
