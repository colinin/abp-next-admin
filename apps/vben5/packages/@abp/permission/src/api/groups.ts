import type { ListResultDto } from '@abp/core';

import type {
  PermissionGroupDefinitionCreateDto,
  PermissionGroupDefinitionDto,
  PermissionGroupDefinitionGetListInput,
  PermissionGroupDefinitionUpdateDto,
} from '../types/groups';

import { requestClient } from '@abp/request';

/**
 * 删除权限定义
 * @param name 权限名称
 */
export function deleteApi(name: string): Promise<void> {
  return requestClient.delete(
    `/api/permission-management/definitions/groups/${name}`,
  );
}

/**
 * 查询权限定义
 * @param name 权限名称
 * @returns 权限定义数据传输对象
 */
export function getApi(name: string): Promise<PermissionGroupDefinitionDto> {
  return requestClient.get<PermissionGroupDefinitionDto>(
    `/api/permission-management/definitions/groups/${name}`,
  );
}

/**
 * 查询权限定义列表
 * @param input 权限过滤条件
 * @returns 权限定义数据传输对象列表
 */
export function getListApi(
  input?: PermissionGroupDefinitionGetListInput,
): Promise<ListResultDto<PermissionGroupDefinitionDto>> {
  return requestClient.get<ListResultDto<PermissionGroupDefinitionDto>>(
    `/api/permission-management/definitions/groups`,
    {
      params: input,
    },
  );
}

/**
 * 创建权限定义
 * @param input 权限定义参数
 * @returns 权限定义数据传输对象
 */
export function createApi(
  input: PermissionGroupDefinitionCreateDto,
): Promise<PermissionGroupDefinitionDto> {
  return requestClient.post<PermissionGroupDefinitionDto>(
    '/api/permission-management/definitions/groups',
    input,
  );
}

/**
 * 更新权限定义
 * @param name 权限名称
 * @param input 权限定义参数
 * @returns 权限定义数据传输对象
 */
export function updateApi(
  name: string,
  input: PermissionGroupDefinitionUpdateDto,
): Promise<PermissionGroupDefinitionDto> {
  return requestClient.put<PermissionGroupDefinitionDto>(
    `/api/permission-management/definitions/groups/${name}`,
    input,
  );
}
