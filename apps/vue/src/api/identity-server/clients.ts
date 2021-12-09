import { defAbpHttp } from '/@/utils/http/abp';
import {
  Client,
  ClientClone,
  ClientCreate,
  ClientUpdate,
  GetClientPagedRequest,
  ClientPagedResult,
} from './model/clientsModel';
import { format } from '/@/utils/strings';
import { ListResultDto } from '../model/baseModel';

enum Api {
  Clone = '/api/identity-server/clients/{id}/clone',
  Create = '/api/identity-server/clients',
  DeleteById = '/api/identity-server/clients/{id}',
  GetById = '/api/identity-server/clients/{id}',
  GetList = '/api/identity-server/clients',
  GetAssignableApiResources = '/api/identity-server/clients/assignable-api-resources',
  GetAssignableIdentityResources = '/api/identity-server/clients/assignable-identity-resources',
  GetAllDistinctAllowedCorsOrigins = '/api/identity-server/clients/distinct-cors-origins',
}

export const clone = (id: string, input: ClientClone) => {
  return defAbpHttp.post<Client>({
    url: format(Api.Clone, { id: id }),
    data: input,
  });
};

export const create = (input: ClientCreate) => {
  return defAbpHttp.post<Client>({
    url: Api.Create,
    data: input,
  });
};

export const update = (id: string, input: ClientUpdate) => {
  return defAbpHttp.put<Client>({
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
  return defAbpHttp.get<Client>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetClientPagedRequest) => {
  return defAbpHttp.get<ClientPagedResult>({
    url: Api.GetList,
    data: input,
  });
};

export const getAssignableApiResources = () => {
  return defAbpHttp.get<ListResultDto<string>>({
    url: Api.GetAssignableApiResources,
  });
};

export const getAssignableIdentityResources = () => {
  return defAbpHttp.get<ListResultDto<string>>({
    url: Api.GetAssignableIdentityResources,
  });
};

export const getAllDistinctAllowedCorsOrigins = () => {
  return defAbpHttp.get<ListResultDto<string>>({
    url: Api.GetAllDistinctAllowedCorsOrigins,
  });
};
