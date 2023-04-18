import { defHttp } from '/@/utils/http/axios';

export const GetAsyncByOptions = (options?: {
  includeLocalizationResources?: boolean;
}) => {
  return defHttp.get<ApplicationConfigurationDto>({
    url: '/api/abp/application-configuration',
    params: options,
  });
};