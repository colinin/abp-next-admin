import { defHttp } from '/@/utils/http/axios';
import { PermissionProvider, PermissionResult, UpdatePermissions } from './model';

export const get = (provider: PermissionProvider) => {
  return defHttp.get<PermissionResult>({
    url: '/api/permission-management/permissions',
    params: provider,
  });
};

export const update = (provider: PermissionProvider, input: UpdatePermissions) => {
  return defHttp.put<void>({
    url: '/api/permission-management/permissions',
    data: input,
    params: provider,
  });
};
