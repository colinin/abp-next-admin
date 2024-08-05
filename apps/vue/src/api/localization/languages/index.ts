import { defHttp } from '/@/utils/http/axios';
import { LanguageCreate, LanguageUpdate, Language, GetLanguageWithFilter } from './model';

export const getList = (input: GetLanguageWithFilter) => {
  return defHttp.get<ListResultDto<Language>>({
    url: '/api/abp/localization/languages',
    params: input,
  });
};

export const getByName = (name: string) => {
  return defHttp.get<Language>({
    url: `/api/localization/languages/${name}`,
  });
};

export const create = (input: LanguageCreate) => {
  return defHttp.post<Language>({
    url: '/api/localization/languages',
    data: input,
  });
};

export const update = (name: string, input: LanguageUpdate) => {
  return defHttp.put<Language>({
    url: `/api/localization/languages/${name}`,
    data: input,
  });
};

export const deleteByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/localization/languages/${name}`,
  });
};