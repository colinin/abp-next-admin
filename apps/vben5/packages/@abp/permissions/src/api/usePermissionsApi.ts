import type {
  GetPermissionGrantedWithProviderListResultDto,
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

  /**
   * 获取已授权对象
   * @param permissionName 权限名称
   * @param providerName 授权提供者
   * @returns
   */
  function getGrantedByProviderApi(
    permissionName: string,
    providerName: string,
  ): Promise<GetPermissionGrantedWithProviderListResultDto> {
    return request<GetPermissionGrantedWithProviderListResultDto>(
      `/api/permission-management/permissions/granted/by-provider`,
      {
        method: 'GET',
        params: {
          permissionName,
          providerName,
        },
      },
    );
  }

  return {
    cancel,
    getApi,
    updateApi,
    getGrantedByProviderApi,
  };
}
