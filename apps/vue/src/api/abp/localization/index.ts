import { defHttp } from '/@/utils/http/axios';

export const GetAsyncByInput = (input: {
  cultureName: string;
  onlyDynamics?: boolean;
}) => {
  return defHttp.get<ApplicationLocalizationDto>({
    url: '/api/abp/application-localization"',
    params: input,
  });
};