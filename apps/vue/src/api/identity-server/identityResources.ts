import { defAbpHttp } from '/@/utils/http/abp';
import {
  IdentityResource,
  IdentityResourceCreate,
  IdentityResourceUpdate,
  GetIdentityResourcePagedRequest,
  IdentityResourcePagedResult,
} from './model/identityResourcesModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = '/api/identity-server/identity-resources',
  DeleteById = '/api/identity-server/identity-resources/{id}',
  GetById = '/api/identity-server/identity-resources/{id}',
  GetList = '/api/identity-server/identity-resources',
}

export const create = (input: IdentityResourceCreate) => {
  return defAbpHttp.post<IdentityResource>({
    url: Api.Create,
    data: input,
  });
};

export const update = (id: string, input: IdentityResourceUpdate) => {
  return defAbpHttp.put<IdentityResource>({
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
  return defAbpHttp.get<IdentityResource>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetIdentityResourcePagedRequest) => {
  return defAbpHttp.get<IdentityResourcePagedResult>({
    url: Api.GetList,
    data: input,
  });
};
