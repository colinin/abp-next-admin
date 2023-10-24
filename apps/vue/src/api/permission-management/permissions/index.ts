import { defAbpHttp } from '/@/utils/http/abp';
import { PermissionProvider, PermissionResult, UpdatePermissions } from './model';

export const get = (provider: PermissionProvider) => {
  return defAbpHttp.get<PermissionResult>({
    url: '/api/permission-management/permissions',
    params: provider,
  });
};

export const update = (provider: PermissionProvider, input: UpdatePermissions) => {
  return defAbpHttp.put<void>({
    url: '/api/permission-management/permissions',
    data: input,
    params: provider,
  });
};
