import { defAbpHttp } from '/@/utils/http/abp';
import { PermissionProvider, PermissionResult, UpdatePermissions } from './model/permissionModel';

enum Api {
  Get = '/api/permission-management/permissions',
  Update = '/api/permission-management/permissions',
}

export const get = (provider: PermissionProvider) => {
  return defAbpHttp.get<PermissionResult>({
    url: Api.Get,
    params: provider,
  });
};

export const update = (provider: PermissionProvider, input: UpdatePermissions) => {
  return defAbpHttp.put<void>({
    url: Api.Update,
    data: input,
    params: provider,
  });
};
