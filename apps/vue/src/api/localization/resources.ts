import { defAbpHttp } from '/@/utils/http/abp';
import { ResourceListResult } from './model/resourcesModel';

enum Api {
  GetList = '/api/abp/localization/resources',
}

export const getList = () => {
  return defAbpHttp.get<ResourceListResult>({
    url: Api.GetList,
  });
};
