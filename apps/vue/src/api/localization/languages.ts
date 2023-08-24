import { defAbpHttp } from '/@/utils/http/abp';
import { LanguageListResult, LanguageCreate, LanguageUpdate, Language, GetLanguageWithFilter } from './model/languagesModel';

const remoteServiceName = 'LocalizationManagement';
const controllerName = 'Language';

enum Api {
  GetList = '/api/abp/localization/languages',
}

export const getList = (input: GetLanguageWithFilter) => {
  return defAbpHttp.get<LanguageListResult>({
    url: Api.GetList,
    params: input,
  });
};

export const GetAsyncByName = (name: string) => {
  return defAbpHttp.request<Language>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncByName',
    params: {
      name: name,
    },
  });
};

export const CreateAsyncByInput = (input: LanguageCreate) => {
  return defAbpHttp.request<Language>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    uniqueName: 'CreateAsyncByInput',
    data: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: LanguageUpdate) => {
  return defAbpHttp.request<Language>({
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