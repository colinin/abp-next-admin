import { defAbpHttp } from '/@/utils/http/abp';
import { OpenIddictApplicationDto,OpenIddictApplicationGetListInput, OpenIddictApplicationCreateDto, OpenIddictApplicationUpdateDto,  } from './model';

const remoteServiceName = 'AbpOpenIddict';
const controllerName = 'OpenIddictApplication';

export const GetAsyncById = (id: string) => {
  return defAbpHttp.request<OpenIddictApplicationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncById',
    params: {
      id: id,
    },
  });
};

export const GetListAsyncByInput = (input: OpenIddictApplicationGetListInput) => {
  return defAbpHttp.pagedRequest<OpenIddictApplicationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    uniqueName: 'GetListAsyncByInput',
    params: {
      input: input,
    },
  });
};

export const CreateAsyncByInput = (input: OpenIddictApplicationCreateDto) => {
  return defAbpHttp.request<OpenIddictApplicationDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    uniqueName: 'CreateAsyncByInput',
    data: input,
  });
};

export const UpdateAsyncByIdAndInput = (id: string, input: OpenIddictApplicationUpdateDto) => {
  return defAbpHttp.request<OpenIddictApplicationDto>({
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

