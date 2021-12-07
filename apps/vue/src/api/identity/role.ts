import { defAbpHttp } from '/@/utils/http/abp';
import {
  Role,
  RoleListResult,
  GetRolePagedRequest,
  RolePagedResult,
  UpdateRole,
  CreateRole,
  CreateRoleClaim,
  RoleClaimListResult,
  UpdateRoleClaim,
  RoleClaim,
} from './model/roleModel';
import { format } from '/@/utils/strings';

enum Api {
  RemoteService = 'AbpIdentity',
  Controller = 'IdentityRole',
  Create = '/api/identity/roles',
  CreateClaim = '/api/identity/roles/{id}/claims',
  DeleteClaim = '',
  Update = '/api/identity/roles/{id}',
  UpdateClaim = '',
  GetById = '/api/identity/roles/{id}',
  GetAllList = '/api/identity/roles/all',
  GetClaimList = '/api/identity/roles/{id}/claims',
  GetList = '/api/identity/roles',
  GetActivedList = '/api/ApiGateway/RouteGroups/Actived',
  RemoveOrganizationUnit = '/api/identity/roles/{id}/organization-units/{ouId}',
}

export const create = (input: CreateRole) => {
  return defAbpHttp.post<Role>({
    url: Api.Create,
    data: input,
  });
};

export const createClaim = (id: string, input: CreateRoleClaim) => {
  return defAbpHttp.post<void>({
    url: format(Api.CreateClaim, { id: id }),
    data: input,
  });
};

export const update = (id: string, input: UpdateRole) => {
  return defAbpHttp.put<Role>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const updateClaim = (id: string, input: UpdateRoleClaim) => {
  return defAbpHttp.put<void>({
    url: format(Api.CreateClaim, { id: id }),
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.GetById, { id: id }),
  });
};

export const deleteClaim = (id: string, input: RoleClaim) => {
  return defAbpHttp.delete<void>(
    {
      url: format(Api.DeleteClaim, { id: id }),
      params: {
        claimType: input.claimType,
        claimValue: input.claimValue,
      },
    },
    {
      joinParamsToUrl: true,
    }
  );
};

export const getById = (id: string) => {
  return defAbpHttp.get<Role>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getAllList = () => {
  return defAbpHttp.get<RoleListResult>({
    url: Api.GetAllList,
  });
};

export const getClaimList = (request: { id: string }) => {
  return defAbpHttp.get<RoleClaimListResult>({
    url: format(Api.GetClaimList, { id: request.id }),
  });
};

export const getList = (input: GetRolePagedRequest) => {
  return defAbpHttp.get<RolePagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const removeOrganizationUnit = (id: string, ouId: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.RemoveOrganizationUnit, { id: id, ouId: ouId }),
  });
};
