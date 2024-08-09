import { defHttp } from '/@/utils/http/axios';
import { TenantDto,TenantGetListInput, TenantCreateDto, TenantUpdateDto, TenantConnectionStringDto,TenantConnectionStringCreateOrUpdate,  } from './model';

export const GetAsyncById = (id: string) => {
  return defHttp.get<TenantDto>({
    url: `/api/saas/tenants/${id}`,
  });
};

export const GetAsyncByName = (name: string) => {
  return defHttp.get<TenantDto>({
    url: `/api/saas/tenants/by-name/${name}`,
  });
};

export const GetListAsyncByInput = (input: TenantGetListInput) => {
  return defHttp.get<PagedResultDto<TenantDto>>({
    url: `/api/saas/tenants`,
    params: input,
  });
};

export const CreateAsyncByInput = (input: TenantCreateDto) => {
  return defHttp.post<TenantDto>({
    url: `/api/saas/tenants`,
    data: input,
  });
};

export const UpdateAsyncByIdAndInput = (id: string, input: TenantUpdateDto) => {
  return defHttp.put<TenantDto>({
    url: `/api/saas/tenants/${id}`,
    data: input,
  });
};

export const DeleteAsyncById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/saas/tenants/${id}`,
  });
};

export const GetConnectionStringAsyncByIdAndName = (id: string, name: string) => {
  return defHttp.get<TenantConnectionStringDto>({
    url: `/api/saas/tenants/${id}/connection-string/${name}`,
  });
};

export const GetConnectionStringAsyncById = (id: string) => {
  return defHttp.get<ListResultDto<TenantConnectionStringDto>>({
    url: `/api/saas/tenants/${id}/connection-string`,
  });
};

export const SetConnectionStringAsyncByIdAndInput = (id: string, input: TenantConnectionStringCreateOrUpdate) => {
  return defHttp.put<TenantConnectionStringDto>({
    url: `/api/saas/tenants/${id}/connection-string`,
    data: input,
  });
};

export const DeleteConnectionStringAsyncByIdAndName = (id: string, name: string) => {
  return defHttp.delete<void>({
    url: `/api/saas/tenants/${id}/connection-string/${name}`,
  });
};
