import type { ListResultDto } from '@abp/core';

import type {
  PermissionDefinitionCreateDto,
  PermissionDefinitionDto,
  PermissionDefinitionGetListInput,
  PermissionDefinitionUpdateDto,
} from '../types/definitions';

import { requestClient } from '@abp/request';

/**
 * 删除权限定义
 * @param name 权限名称
 */
export function deleteApi(name: string): Promise<void> {
  return requestClient.delete(`/api/permission-management/definitions/${name}`);
}

/**
 * 查询权限定义
 * @param name 权限名称
 * @returns 权限定义数据传输对象
 */
export function getApi(name: string): Promise<PermissionDefinitionDto> {
  return requestClient.get<PermissionDefinitionDto>(
    `/api/permission-management/definitions/${name}`,
  );
}

/**
 * 查询权限定义列表
 * @param input 权限过滤条件
 * @returns 权限定义数据传输对象列表
 */
export function getListApi(
  input?: PermissionDefinitionGetListInput,
): Promise<ListResultDto<PermissionDefinitionDto>> {
  return requestClient.get<ListResultDto<PermissionDefinitionDto>>(
    `/api/permission-management/definitions`,
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
  input: PermissionDefinitionCreateDto,
): Promise<PermissionDefinitionDto> {
  return requestClient.post<PermissionDefinitionDto>(
    '/api/permission-management/definitions',
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
  input: PermissionDefinitionUpdateDto,
): Promise<PermissionDefinitionDto> {
  return requestClient.put<PermissionDefinitionDto>(
    `/api/permission-management/definitions/${name}`,
    input,
  );
}
