import { defAbpHttp } from '/@/utils/http/abp';
import { FindTenantResult } from './models/tenantModel';

export const findTenantByName = (name: string) => {
  return defAbpHttp.request<FindTenantResult>({
    service: 'abp',
    controller: 'AbpTenant',
    action: 'FindTenantByNameAsync',
    params: { name: name },
  });
};

export const findTenantById = (id: string) => {
  return defAbpHttp.request<FindTenantResult>({
    service: 'abp',
    controller: 'AbpTenant',
    action: 'FindTenantByIdAsync',
    params: { id: id },
  });
};
