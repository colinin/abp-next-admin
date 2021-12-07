import { defAbpHttp } from '/@/utils/http/abp';
import {
  ApiResource,
  ApiResourceCreate,
  ApiResourceUpdate,
  GetApiResourcePagedRequest,
  ApiResourcePagedResult,
} from './model/apiResourcesModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = '/api/identity-server/api-resources',
  DeleteById = '/api/identity-server/api-resources/{id}',
  GetById = '/api/identity-server/api-resources/{id}',
  GetList = '/api/identity-server/api-resources',
}

export const create = (input: ApiResourceCreate) => {
  return defAbpHttp.post<ApiResource>({
    url: Api.Create,
    data: input,
  });
};

export const update = (id: string, input: ApiResourceUpdate) => {
  return defAbpHttp.put<ApiResource>({
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
  return defAbpHttp.get<ApiResource>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetApiResourcePagedRequest) => {
  return defAbpHttp.get<ApiResourcePagedResult>({
    url: Api.GetList,
    data: input,
  });
};
