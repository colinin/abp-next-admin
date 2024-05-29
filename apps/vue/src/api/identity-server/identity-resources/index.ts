import { defHttp } from '/@/utils/http/axios';
import {
  IdentityResource,
  IdentityResourceCreate,
  IdentityResourceUpdate,
  GetIdentityResourcePagedRequest,
} from './model';

export const create = (input: IdentityResourceCreate) => {
  return defHttp.post<IdentityResource>({
    url: '/api/identity-server/identity-resources',
    data: input,
  });
};

export const update = (id: string, input: IdentityResourceUpdate) => {
  return defHttp.put<IdentityResource>({
    url: `/api/identity-server/identity-resources/${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity-server/identity-resources/${id}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<IdentityResource>({
    url: `/api/identity-server/identity-resources/${id}`,
  });
};

export const getList = (input: GetIdentityResourcePagedRequest) => {
  return defHttp.get<PagedResultDto<IdentityResource>>({
    url: '/api/identity-server/identity-resources',
    params: input,
  });
};
