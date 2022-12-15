import { defAbpHttp } from '/@/utils/http/abp';
import { EditionCreateDto, EditionDto,EditionGetListInput, EditionUpdateDto,  } from './model';

const remoteServiceName = 'AbpSaas';
const controllerName = 'Edition';

export const CreateAsyncByInput = (input: EditionCreateDto) => {
  return defAbpHttp.request<EditionDto>({
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
  return defAbpHttp.request<EditionDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncById',
    params: {
      id: id,
    },
  });
};

export const GetListAsyncByInput = (input: EditionGetListInput) => {
  return defAbpHttp.pagedRequest<EditionDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    uniqueName: 'GetListAsyncByInput',
    params: {
      input: input,
    },
  });
};

export const UpdateAsyncByIdAndInput = (id: string, input: EditionUpdateDto) => {
  return defAbpHttp.request<EditionDto>({
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
