import { defAbpHttp } from '/@/utils/http/abp';
import {
  PersistedGrant,
  GetPersistedGrantPagedRequest,
  PersistedGrantPagedResult,
} from './model/persistedGrantsModel';
import { format } from '/@/utils/strings';

enum Api {
  DeleteById = '/api/identity-server/persisted-grants/{id}',
  GetById = '/api/identity-server/persisted-grants/{id}',
  GetList = '/api/identity-server/persisted-grants',
}

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.GetById, { id: id }),
  });
};

export const get = (id: string) => {
  return defAbpHttp.get<PersistedGrant>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetPersistedGrantPagedRequest) => {
  return defAbpHttp.get<PersistedGrantPagedResult>({
    url: Api.GetList,
    params: input,
  });
};
