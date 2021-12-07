import { defAbpHttp } from '/@/utils/http/abp';
import { Log, GetLogPagedRequest, LogPagedResult } from './model/loggingModel';
import { format } from '/@/utils/strings';

enum Api {
  GetById = '/api/auditing/logging/{id}',
  GetList = '/api/auditing/logging',
}

export const get = (id: string) => {
  return defAbpHttp.get<Log>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetLogPagedRequest) => {
  return defAbpHttp.get<LogPagedResult>({
    url: Api.GetList,
    params: input,
  });
};
