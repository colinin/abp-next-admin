import { defHttp } from '/@/utils/http/axios';
import {
  Client,
  ClientClone,
  ClientCreate,
  ClientUpdate,
  GetClientPagedRequest,
} from './model';

export const clone = (id: string, input: ClientClone) => {
  return defHttp.post<Client>({
    url: `/api/identity-server/clients/${id}/clone`,
    data: input,
  });
};

export const create = (input: ClientCreate) => {
  return defHttp.post<Client>({
    url: '/api/identity-server/clients',
    data: input,
  });
};

export const update = (id: string, input: ClientUpdate) => {
  return defHttp.put<Client>({
    url: `/api/identity-server/clients/${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity-server/clients/${id}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<Client>({
    url: `/api/identity-server/clients/${id}`,
  });
};

export const getList = (input: GetClientPagedRequest) => {
  return defHttp.get<PagedResultDto<Client>>({
    url: '/api/identity-server/clients',
    params: input,
  });
};

export const getAssignableApiResources = () => {
  return defHttp.get<ListResultDto<string>>({
    url: '/api/identity-server/clients/assignable-api-resources',
  });
};

export const getAssignableIdentityResources = () => {
  return defHttp.get<ListResultDto<string>>({
    url: '/api/identity-server/clients/assignable-identity-resources',
  });
};

export const getAllDistinctAllowedCorsOrigins = () => {
  return defHttp.get<ListResultDto<string>>({
    url: '/api/identity-server/clients/distinct-cors-origins',
  });
};
