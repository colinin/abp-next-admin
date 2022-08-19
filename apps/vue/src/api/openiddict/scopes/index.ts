import { defAbpHttp } from '/@/utils/http/abp';
import {
  OpenIddictScopeDto,
  OpenIddictScopeCreateDto,
  OpenIddictScopeUpdateDto,
  OpenIddictScopeGetListInput,
} from './model';

const remoteServiceName = 'AbpOpenIddict';
const controllerName = 'OpenIddictScope';

export const getById = (id: string) => {
  return defAbpHttp.request<OpenIddictScopeDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    params: {
      id: id,
    },
  });
};

export const getList = (input: OpenIddictScopeGetListInput) => {
  return defAbpHttp.pagedRequest<OpenIddictScopeDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    params: {
      input: input,
    },
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteAsync',
    params: {
      id: id,
    },
  });
};

export const create = (input: OpenIddictScopeCreateDto) => {
  return defAbpHttp.request<OpenIddictScopeDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    data: input,
  });
};

export const update = (input: OpenIddictScopeUpdateDto) => {
  return defAbpHttp.request<OpenIddictScopeDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UpdateAsync',
    data: input,
  });
};
