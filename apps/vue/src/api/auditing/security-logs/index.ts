import { defHttp } from '/@/utils/http/axios';
import { SecurityLog, GetSecurityLogPagedRequest } from './model';

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/auditing/security-log/${id}`,
  });
};

export const getById = (id: string) => {
  return defHttp.get<SecurityLog>({
    url: `/api/auditing/security-log/${id}`,
  });
};

export const getList = (input: GetSecurityLogPagedRequest) => {
  return defHttp.get<PagedResultDto<SecurityLog>>({
    url: '/api/auditing/security-log',
    params: input,
  });
};
