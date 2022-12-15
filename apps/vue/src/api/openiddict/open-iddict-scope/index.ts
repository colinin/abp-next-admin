import { defAbpHttp } from '/@/utils/http/abp';
import { OpenIddictScopeCreateDto, OpenIddictScopeDto,OpenIddictScopeGetListInput, OpenIddictScopeUpdateDto,  } from './model';

const remoteServiceName = 'AbpOpenIddict';
const controllerName = 'OpenIddictScope';

export const CreateAsyncByInput = (input: OpenIddictScopeCreateDto) => {
  return defAbpHttp.request<OpenIddictScopeDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    uniqueName: 'CreateAsyncByInput',
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

export const GetAsyncById = (id: string) => {
  return defAbpHttp.request<OpenIddictScopeDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncById',
    params: {
      id: id,
    },
  });
};

export const GetListAsyncByInput = (input: OpenIddictScopeGetListInput) => {
  return defAbpHttp.pagedRequest<OpenIddictScopeDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    uniqueName: 'GetListAsyncByInput',
    params: {
      input: input,
    },
  });
};

export const UpdateAsyncByIdAndInput = (id: string, input: OpenIddictScopeUpdateDto) => {
  return defAbpHttp.request<OpenIddictScopeDto>({
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

