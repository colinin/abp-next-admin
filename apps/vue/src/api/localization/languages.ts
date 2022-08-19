import { defAbpHttp } from '/@/utils/http/abp';
import { LanguageListResult } from './model/languagesModel';

enum Api {
  GetList = '/api/abp/localization/languages',
}

export const getList = () => {
  return defAbpHttp.get<LanguageListResult>({
    url: Api.GetList,
  });
};
