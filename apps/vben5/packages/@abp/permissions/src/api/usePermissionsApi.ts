import type {
  PermissionProvider,
  PermissionResultDto,
  PermissionsUpdateDto,
} from '../types/permissions';

import { useRequest } from '@abp/request';

export function usePermissionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询权限
   * @param provider
   * @returns 权限实体数据传输对象
   */
  function getApi(provider: PermissionProvider): Promise<PermissionResultDto> {
    return request<PermissionResultDto>(
      `/api/permission-management/permissions`,
      {
        method: 'GET',
        params: provider,
      },
    );
  }

  /**
   * 更新权限
   * @param provider
   * @param input
   */
  function updateApi(
    provider: PermissionProvider,
    input: PermissionsUpdateDto,
  ): Promise<void> {
    return request(`/api/permission-management/permissions`, {
      data: input,
      method: 'PUT',
      params: provider,
    });
  }

  return {
    cancel,
    getApi,
    updateApi,
  };
}
