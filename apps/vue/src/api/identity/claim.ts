import { defAbpHttp } from '/@/utils/http/abp';
import {
  IdentityClaimType,
  CreateIdentityClaimType,
  IdentityClaimTypeListResult,
  UpdateIdentityClaimType,
  GetIdentityClaimTypePagedRequest,
} from './model/claimModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = '/api/identity/claim-types',
  Delete = '/api/identity/claim-types/{id}',
  Update = '/api/identity/claim-types/{id}',
  GetById = '/api/identity/claim-types/{id}',
  GetList = '/api/identity/claim-types',
  GetActivedList = '/api/identity/claim-types/actived-list',
}

export const create = (input: CreateIdentityClaimType) => {
  return defAbpHttp.post<IdentityClaimType>({
    url: Api.Create,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.Delete, { id: id }),
  });
};

export const update = (id: string, input: UpdateIdentityClaimType) => {
  return defAbpHttp.put<IdentityClaimType>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const getById = (id: string) => {
  return defAbpHttp.get<IdentityClaimType>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetIdentityClaimTypePagedRequest) => {
  return defAbpHttp.get<IdentityClaimType>({
    url: Api.GetList,
    params: input,
  });
};

export const getActivedList = () => {
  return defAbpHttp.get<IdentityClaimTypeListResult>({
    url: Api.GetActivedList,
  });
};
