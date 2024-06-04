import { defHttp } from '/@/utils/http/axios';
import {
  ApiScope,
  ApiScopeCreate,
  ApiScopeUpdate,
  GetApiScopePagedRequest,
} from './model';

export const create = (input: ApiScopeCreate) => {
  return defHttp.post<ApiScope>({
    url: '/api/identity-server/api-scopes',
    data: input,
  });
};

export const update = (id: string, input: ApiScopeUpdate) => {
  return defHttp.put<ApiScope>({
    url: `/api/identity-server/api-scopes/${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity-server/api-scopes/${id}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<ApiScope>({
    url: `/api/identity-server/api-scopes/${id}`,
  });
};

export const getList = (input: GetApiScopePagedRequest) => {
  return defHttp.get<PagedResultDto<ApiScope>>({
    url: '/api/identity-server/api-scopes',
    params: input,
  });
};
