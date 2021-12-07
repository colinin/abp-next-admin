import { defAbpHttp } from '/@/utils/http/abp';
import {
  Text,
  TextCreate,
  TextUpdate,
  GetTextByKey,
  GetTextPagedRequest,
  TextPagedResult,
} from './model/textsModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = '/api/localization/texts',
  DeleteById = '/api/localization/texts/{id}',
  GetById = '/api/localization/texts/{id}',
  GetList = '/api/localization/texts',
  GetByCulture = '/api/localization/texts/by-culture-key',
}

export const get = (id: number) => {
  return defAbpHttp.get<Text>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getByCulture = (input: GetTextByKey) => {
  return defAbpHttp.get<Text>({
    url: Api.GetByCulture,
    params: input,
  });
};

export const create = (input: TextCreate) => {
  return defAbpHttp.post<Text>({
    url: Api.Create,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.GetById, { id: id }),
  });
};

export const update = (id: number, input: TextUpdate) => {
  return defAbpHttp.put<Text>({
    url: format(Api.GetById, { id: id }),
    data: input,
  });
};

export const getList = (input: GetTextPagedRequest) => {
  return defAbpHttp.get<TextPagedResult>({
    url: Api.GetList,
    params: input,
  });
};
