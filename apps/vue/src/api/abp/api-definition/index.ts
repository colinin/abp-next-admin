import { defHttp } from '/@/utils/http/axios';
import { ApplicationApiDescriptionModel } from './model';

export const GetAsyncByModel = (model?: {
  includeTypes?: boolean;
}) => {
  return defHttp.get<ApplicationApiDescriptionModel>({
    url: '/api/abp/api-definition',
    params: model,
  });
};