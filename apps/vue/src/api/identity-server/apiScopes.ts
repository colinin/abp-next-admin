import { defAbpHttp } from '/@/utils/http/abp';
import {
  ApiScope,
  ApiScopeCreate,
  ApiScopeUpdate,
  GetApiScopePagedRequest,
  ApiScopePagedResult,
} from './model/apiScopesModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = '/api/identity-server/api-scopes',
  DeleteById = '/api/identity-server/api-scopes/{id}',
  GetById = '/api/identity-server/api-scopes/{id}',
  GetList = '/api/identity-server/api-scopes',
}

export const create = (input: ApiScopeCreate) => {
  return defAbpHttp.post<ApiScope>({
    url: Api.Create,
    data: input,
  });
};

export const update = (id: string, input: ApiScopeUpdate) => {
  return defAbpHttp.put<ApiScope>({
    url: format(Api.GetById, { id: id }),
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.GetById, { id: id }),
  });
};

export const get = (id: string) => {
  return defAbpHttp.get<ApiScope>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetApiScopePagedRequest) => {
  return defAbpHttp.get<ApiScopePagedResult>({
    url: Api.GetList,
    data: input,
  });
};
