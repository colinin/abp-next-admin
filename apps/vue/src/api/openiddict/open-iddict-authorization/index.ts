import { defAbpHttp } from '/@/utils/http/abp';
import { OpenIddictAuthorizationDto,OpenIddictAuthorizationGetListInput,  } from './model';

const remoteServiceName = 'AbpOpenIddict';
const controllerName = 'OpenIddictAuthorization';

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

export const GetAsyncById = (id: string) => {
  return defAbpHttp.request<OpenIddictAuthorizationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncById',
    params: {
      id: id,
    },
  });
};

export const GetListAsyncByInput = (input: OpenIddictAuthorizationGetListInput) => {
  return defAbpHttp.pagedRequest<OpenIddictAuthorizationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    uniqueName: 'GetListAsyncByInput',
    params: {
      input: input,
    },
  });
};

