import { defAbpHttp } from '/@/utils/http/abp';
import {
  TextTemplateContentDto,
  TextTemplateContentGetInput,
  TextTemplateRestoreInput,
  TextTemplateContentUpdateDto
} from './model';

const remoteServiceName = 'AbpTextTemplating';
const controllerName = 'TextTemplate';

export const GetAsyncByInput = (input: TextTemplateContentGetInput) => {
  return defAbpHttp.request<TextTemplateContentDto>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncByInput',
    params: input,
  });
};

export const RestoreToDefaultAsyncByNameAndInput = (name: string, input: TextTemplateRestoreInput) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'RestoreToDefaultAsync',
    uniqueName: 'RestoreToDefaultAsyncByNameAndInput',
    params: {
      name: name,
    },
    data: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: TextTemplateContentUpdateDto) => {
  return defAbpHttp.request<TextTemplateContentDto>({
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
