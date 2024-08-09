import { defHttp } from '/@/utils/http/axios';
import { Log, GetLogPagedRequest } from './model';

export const get = (id: string) => {
  return defHttp.get<Log>({
    url: `/api/auditing/logging/${id}`,
  });
};

export const getList = (input: GetLogPagedRequest) => {
  return defHttp.get<PagedResultDto<Log>>({
    url: '/api/auditing/logging',
    params: input,
  });
};
