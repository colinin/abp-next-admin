import { defAbpHttp } from '/@/utils/http/abp';
import {
  Text,
  SetTextInput,
  GetTextByKey,
  GetTextRequest,
  TextListResult,
} from './model/textsModel';

enum Api {
  SetText = '/api/localization/texts',
  GetList = '/api/abp/localization/texts',
  GetByCulture = '/api/abp/localization/texts/by-culture-key',
}

export const getByCulture = (input: GetTextByKey) => {
  return defAbpHttp.get<Text>({
    url: Api.GetByCulture,
    params: input,
  });
};

export const setText = (input: SetTextInput) => {
  return defAbpHttp.put<Text>({
    url: Api.SetText,
    data: input,
  });
};

export const getList = (input: GetTextRequest) => {
  return defAbpHttp.get<TextListResult>({
    url: Api.GetList,
    params: input,
  });
};
