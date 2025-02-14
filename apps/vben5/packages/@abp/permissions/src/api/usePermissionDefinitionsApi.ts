import type { ListResultDto } from '@abp/core';

import type {
  PermissionDefinitionCreateDto,
  PermissionDefinitionDto,
  PermissionDefinitionGetListInput,
  PermissionDefinitionUpdateDto,
} from '../types/definitions';

import { useRequest } from '@abp/request';

export function usePermissionDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除权限定义
   * @param name 权限名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/permission-management/definitions/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询权限定义
   * @param name 权限名称
   * @returns 权限定义数据传输对象
   */
  function getApi(name: string): Promise<PermissionDefinitionDto> {
    return request<PermissionDefinitionDto>(
      `/api/permission-management/definitions/${name}`,
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
    input?: PermissionDefinitionGetListInput,
  ): Promise<ListResultDto<PermissionDefinitionDto>> {
    return request<ListResultDto<PermissionDefinitionDto>>(
      `/api/permission-management/definitions`,
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
    input: PermissionDefinitionCreateDto,
  ): Promise<PermissionDefinitionDto> {
    return request<PermissionDefinitionDto>(
      '/api/permission-management/definitions',
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
    input: PermissionDefinitionUpdateDto,
  ): Promise<PermissionDefinitionDto> {
    return request<PermissionDefinitionDto>(
      `/api/permission-management/definitions/${name}`,
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
