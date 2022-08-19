import { defAbpHttp } from '/@/utils/http/abp';
import { PagedResultDto } from '../../model/baseModel';
import {
  TextTemplateDefinition,
  TextTemplateContent,
  TextTemplateContentGetInput,
  TextTemplateUpdateInput,
  TextTemplateRestoreInput,
  TextTemplateDefinitionGetListInput,
} from './model';

const remoteServiceName = 'AbpTextTemplating';
const controllerName = 'TextTemplate';

export const get = (name: string) => {
  return defAbpHttp.request<TextTemplateDefinition>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    params: {
      name: name,
    },
  });
};

export const getContent = (input: TextTemplateContentGetInput) => {
  return defAbpHttp.request<TextTemplateContent>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetContentAsync',
    params: {
      input: input,
    },
  });
}

export const getList = (input: TextTemplateDefinitionGetListInput) => {
  return defAbpHttp.request<PagedResultDto<TextTemplateDefinition>>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    params: {
      input: input,
    },
  });
};

export const restoreToDefault = (input: TextTemplateRestoreInput) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'RestoreToDefaultAsync',
    data: input,
  });
};

export const update = (input: TextTemplateUpdateInput) => {
  return defAbpHttp.request<TextTemplateDefinition>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UpdateAsync',
    data: input,
  });
};
