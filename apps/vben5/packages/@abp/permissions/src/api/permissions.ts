import type {
  PermissionProvider,
  PermissionResultDto,
  PermissionsUpdateDto,
} from '../types/permissions';

import { requestClient } from '@abp/request';

/**
 * 查询权限
 * @param provider
 * @returns 权限实体数据传输对象
 */
export function getApi(
  provider: PermissionProvider,
): Promise<PermissionResultDto> {
  return requestClient.get<PermissionResultDto>(
    `/api/permission-management/permissions`,
    {
      params: provider,
    },
  );
}

/**
 * 更新权限
 * @param provider
 * @param input
 */
export function updateApi(
  provider: PermissionProvider,
  input: PermissionsUpdateDto,
): Promise<void> {
  return requestClient.put(`/api/permission-management/permissions`, input, {
    params: provider,
  });
}
