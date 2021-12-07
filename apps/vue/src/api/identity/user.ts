import { defAbpHttp } from '/@/utils/http/abp';
import {
  User,
  UserClaimListResult,
  CreateUser,
  CreateUserClaim,
  ChangePassword,
  UpdateUser,
  GetUserPagedRequest,
  UserPagedResult,
  UpdateUserClaim,
  UserClaim,
} from './model/userModel';
import { RoleListResult } from './model/roleModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = '/api/identity/users',
  CreateClaim = '/api/identity/users/{id}/claims',
  DeleteClaim = '/api/identity/users/{id}/claims',
  ChangePassword = '/api/identity/users/change-password',
  Delete = '/api/identity/users/{id}',
  GetById = '/api/identity/users/{id}',
  GetList = '/api/identity/users',
  GetClaimList = '/api/identity/users/{id}/claims',
  GetRoleList = '/api/identity/users/{id}/roles',
  GetActivedList = '/api/ApiGateway/RouteGroups/Actived',
  GetAssignableRoles = '/api/identity/users/assignable-roles',
  Update = '/api/identity/users/{id}',
  UpdateClaim = '/api/identity/users/{id}/claims',
  Lock = '/api/identity/users/{id}/lock/{seconds}',
  UnLock = '/api/identity/users/{id}/unlock',
  RemoveOrganizationUnit = '/api/identity/users/{id}/organization-units/{ouId}',
}

export const create = (input: CreateUser) => {
  return defAbpHttp.post<User>({
    url: Api.Create,
    data: input,
  });
};

export const createClaim = (id: string, input: CreateUserClaim) => {
  return defAbpHttp.post<void>({
    url: format(Api.CreateClaim, { id: id }),
    data: input,
  });
};

export const changePassword = (id: string, input: ChangePassword) => {
  return defAbpHttp.put<void>({
    url: Api.ChangePassword,
    data: input,
    params: {
      id: id,
    },
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.Delete, { id: id }),
  });
};

export const deleteClaim = (id: string, input: UserClaim) => {
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
  return defAbpHttp.get<User>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getAssignableRoles = () => {
  return defAbpHttp.get<RoleListResult>({
    url: Api.GetAssignableRoles,
  });
};

export const getRoleList = (id: string) => {
  return defAbpHttp.get<RoleListResult>({
    url: format(Api.GetRoleList, { id: id }),
  });
};

export const getClaimList = (request: { id: string }) => {
  return defAbpHttp.get<UserClaimListResult>({
    url: format(Api.GetClaimList, { id: request.id }),
  });
};

export const getList = (input: GetUserPagedRequest) => {
  return defAbpHttp.get<UserPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const update = (id: string, input: UpdateUser) => {
  return defAbpHttp.put<User>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const updateClaim = (id: string, input: UpdateUserClaim) => {
  return defAbpHttp.put<void>({
    url: format(Api.CreateClaim, { id: id }),
    data: input,
  });
};

export const lock = (id: string, seconds: number) => {
  return defAbpHttp.put<void>({
    url: format(Api.Lock, { id: id, seconds: seconds }),
  });
};

export const unlock = (id: string) => {
  return defAbpHttp.put<void>({
    url: format(Api.UnLock, { id: id }),
  });
};

export const removeOrganizationUnit = (id: string, ouId: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.RemoveOrganizationUnit, { id: id, ouId: ouId }),
  });
};
