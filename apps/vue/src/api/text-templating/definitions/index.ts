import { defAbpHttp } from '/@/utils/http/abp';
import {
  TextTemplateDefinitionDto,
  TextTemplateDefinitionCreateDto,
  TextTemplateDefinitionUpdateDto,
  TextTemplateDefinitionGetListInput
} from './model';

const remoteServiceName = 'AbpTextTemplating';
const controllerName = 'TextTemplateDefinition';

export const CreateAsyncByInput = (input: TextTemplateDefinitionCreateDto) => {
  return defAbpHttp.request<TextTemplateDefinitionDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    uniqueName: 'CreateAsyncByInput',
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteAsync',
    uniqueName: 'DeleteAsyncByName',
    params: {
      name: name,
    },
  });
};

export const GetByNameAsyncByName = (name: string) => {
  return defAbpHttp.request<TextTemplateDefinitionDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetByNameAsync',
    uniqueName: 'GetByNameAsyncByName',
    params: {
      name: name,
    },
  });
};

export const GetListAsyncByInput = (input: TextTemplateDefinitionGetListInput) => {
  return defAbpHttp.request<PagedResultDto<TextTemplateDefinitionDto>>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    uniqueName: 'GetListAsyncByInput',
    params: {
      input: input,
    },
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: TextTemplateDefinitionUpdateDto) => {
  return defAbpHttp.request<TextTemplateDefinitionDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UpdateAsync',
    uniqueName: 'UpdateAsyncByNameAndInput',
    params: {
      name: name,
    },
    data: input,
  });
};
