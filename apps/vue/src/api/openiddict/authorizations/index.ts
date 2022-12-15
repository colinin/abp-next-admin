import { defAbpHttp } from '/@/utils/http/abp';
import {
  OpenIddictAuthorizationDto,
  OpenIddictAuthorizationGetListInput,
} from './model';

const remoteServiceName = 'AbpOpenIddict';
const controllerName = 'OpenIddictAuthorization';

export const getById = (id: string) => {
  return defAbpHttp.request<OpenIddictAuthorizationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    params: {
      id: id,
    },
  });
};

export const getList = (input: OpenIddictAuthorizationGetListInput) => {
  return defAbpHttp.pagedRequest<OpenIddictAuthorizationDto>({
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
