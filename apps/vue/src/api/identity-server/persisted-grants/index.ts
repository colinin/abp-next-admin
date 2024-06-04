import { defHttp } from '/@/utils/http/axios';
import {
  PersistedGrant,
  GetPersistedGrantPagedRequest,
} from './model';

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity-server/persisted-grants/${id}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<PersistedGrant>({
    url: `/api/identity-server/persisted-grants/${id}`,
  });
};

export const getList = (input: GetPersistedGrantPagedRequest) => {
  return defHttp.get<PagedResultDto<PersistedGrant>>({
    url: '/api/identity-server/persisted-grants',
    params: input,
  });
};
