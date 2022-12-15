import { defAbpHttp } from '/@/utils/http/abp';
import { TenantDto,TenantGetListInput, TenantCreateDto, TenantUpdateDto, TenantConnectionStringDto,TenantConnectionStringCreateOrUpdate,  } from './model';

const remoteServiceName = 'AbpSaas';
const controllerName = 'Tenant';

export const GetAsyncById = (id: string) => {
  return defAbpHttp.request<TenantDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncById',
    params: {
      id: id,
    },
  });
};

export const GetAsyncByName = (name: string) => {
  return defAbpHttp.request<TenantDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncByName',
    params: {
      name: name,
    },
  });
};

export const GetListAsyncByInput = (input: TenantGetListInput) => {
  return defAbpHttp.pagedRequest<TenantDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    uniqueName: 'GetListAsyncByInput',
    params: {
      input: input,
    },
  });
};

export const CreateAsyncByInput = (input: TenantCreateDto) => {
  return defAbpHttp.request<TenantDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    uniqueName: 'CreateAsyncByInput',
    data: input,
  });
};

export const UpdateAsyncByIdAndInput = (id: string, input: TenantUpdateDto) => {
  return defAbpHttp.request<TenantDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UpdateAsync',
    uniqueName: 'UpdateAsyncByIdAndInput',
    params: {
      id: id,
    },
    data: input,
  });
};

export const DeleteAsyncById = (id: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteAsync',
    uniqueName: 'DeleteAsyncById',
    params: {
      id: id,
    },
  });
};

export const GetConnectionStringAsyncByIdAndName = (id: string, name: string) => {
  return defAbpHttp.request<TenantConnectionStringDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetConnectionStringAsync',
    uniqueName: 'GetConnectionStringAsyncByIdAndName',
    params: {
      id: id,
      name: name,
    },
  });
};

export const GetConnectionStringAsyncById = (id: string) => {
  return defAbpHttp.listRequest<TenantConnectionStringDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetConnectionStringAsync',
    uniqueName: 'GetConnectionStringAsyncById',
    params: {
      id: id,
    },
  });
};

export const SetConnectionStringAsyncByIdAndInput = (id: string, input: TenantConnectionStringCreateOrUpdate) => {
  return defAbpHttp.request<TenantConnectionStringDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'SetConnectionStringAsync',
    uniqueName: 'SetConnectionStringAsyncByIdAndInput',
    params: {
      id: id,
    },
    data: input,
  });
};

export const DeleteConnectionStringAsyncByIdAndName = (id: string, name: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteConnectionStringAsync',
    uniqueName: 'DeleteConnectionStringAsyncByIdAndName',
    params: {
      id: id,
      name: name,
    },
  });
};
