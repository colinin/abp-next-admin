import { defAbpHttp } from '/@/utils/http/abp';
import {
  Language,
  LanguageCreateOrUpdate,
  GetLanguagePagedRequest,
  LanguageListResult,
  LanguagePagedResult,
} from './model/languagesModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = '/api/localization/languages',
  DeleteById = '/api/localization/languages/{id}',
  GetById = '/api/localization/languages/{id}',
  GetList = '/api/localization/languages',
  GetAllList = '/api/localization/languages/all',
}

export const get = (id: string) => {
  return defAbpHttp.get<Language>({
    url: format(Api.GetById, { id: id }),
  });
};

export const create = (input: LanguageCreateOrUpdate) => {
  return defAbpHttp.post<Language>({
    url: Api.Create,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.GetById, { id: id }),
  });
};

export const update = (id: string, input: LanguageCreateOrUpdate) => {
  return defAbpHttp.put<Language>({
    url: format(Api.GetById, { id: id }),
    data: input,
  });
};

export const getList = (input: GetLanguagePagedRequest) => {
  return defAbpHttp.get<LanguagePagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const getAll = () => {
  return defAbpHttp.get<LanguageListResult>({
    url: Api.GetAllList,
  });
};
