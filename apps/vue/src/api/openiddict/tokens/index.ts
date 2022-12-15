import { defAbpHttp } from '/@/utils/http/abp';
import {
  OpenIddictTokenDto,
  OpenIddictTokenGetListInput,
} from './model';

const remoteServiceName = 'AbpOpenIddict';
const controllerName = 'OpenIddictToken';

export const getById = (id: string) => {
  return defAbpHttp.request<OpenIddictTokenDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    params: {
      id: id,
    },
  });
};

export const getList = (input: OpenIddictTokenGetListInput) => {
  return defAbpHttp.pagedRequest<OpenIddictTokenDto>({
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
