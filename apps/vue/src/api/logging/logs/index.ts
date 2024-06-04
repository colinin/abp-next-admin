import { defAbpHttp } from '/@/utils/http/abp';
import { Log, GetLogPagedRequest } from './model';

export const get = (id: string) => {
  return defAbpHttp.get<Log>({
    url: `/api/auditing/logging/${id}`,
  });
};

export const getList = (input: GetLogPagedRequest) => {
  return defAbpHttp.get<PagedResultDto<Log>>({
    url: '/api/auditing/logging',
    params: input,
  });
};
