import { defHttp } from '/@/utils/http/axios';
import { FindTenantResult } from './model';

export const findTenantByName = (name: string) => {
  return defHttp.get<FindTenantResult>({
    url: `/api/abp/multi-tenancy/tenants/by-name/${name}`
  });
};

export const findTenantById = (id: string) => {
  return defHttp.get<FindTenantResult>({
    url: `/api/abp/multi-tenancy/tenants/by-id/${id}`
  });
};
