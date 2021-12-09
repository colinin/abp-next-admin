import { defAbpHttp } from '/@/utils/http/abp';
import {
  Tenant,
  CreateTenant,
  UpdateTenant,
  GetTenantPagedRequest,
  TenantPagedResult,
  TenantConnectionString,
  TenantConnectionStringListResult,
} from './model/tenantModel';
import { format } from '/@/utils/strings';

/** 与 multi-tenancy中不同,此为管理tenant api */
enum Api {
  Create = '/api/tenant-management/tenants',
  DeleteById = '/api/tenant-management/tenants/{id}',
  GetById = '/api/tenant-management/tenants/{id}',
  GetList = '/api/tenant-management/tenants',
  Update = '/api/tenant-management/tenants/{id}',
  GetConnectionStrings = '/api/tenant-management/tenants/{id}/connection-string',
  SetConnectionString = '/api/tenant-management/tenants/{id}/connection-string',
  DeleteConnectionString = '/api/tenant-management/tenants/{id}/connection-string/{name}',
}

export const getById = (id: string) => {
  return defAbpHttp.get<Tenant>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetTenantPagedRequest) => {
  return defAbpHttp.get<TenantPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const create = (input: CreateTenant) => {
  return defAbpHttp.post<Tenant>({
    url: Api.Create,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.GetById, { id: id }),
  });
};

export const update = (id: string, input: UpdateTenant) => {
  return defAbpHttp.put<Tenant>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const getConnectionStrings = (id: string) => {
  return defAbpHttp.get<TenantConnectionStringListResult>({
    url: format(Api.GetConnectionStrings, { id: id }),
  });
};

export const setConnectionString = (id: string, input: TenantConnectionString) => {
  return defAbpHttp.put<void>({
    url: format(Api.SetConnectionString, { id: id }),
    data: input,
  });
};

export const deleteConnectionString = (id: string, name: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.DeleteConnectionString, { id: id, name: name }),
  });
};
