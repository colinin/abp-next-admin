import { defHttp } from '/@/utils/http/axios';
import { Text, SetTextInput, GetTextByKey, GetTextRequest } from './model';

export const getByCulture = (input: GetTextByKey) => {
  return defHttp.get<Text>({
    url: '/api/abp/localization/texts/by-culture-key',
    params: input,
  });
};

export const setText = (input: SetTextInput) => {
  return defHttp.put<Text>({
    url: '/api/localization/texts',
    data: input,
  });
};

export const getList = (input: GetTextRequest) => {
  return defHttp.get<ListResultDto<Text>>({
    url: '/api/abp/localization/texts',
    params: input,
  });
};
