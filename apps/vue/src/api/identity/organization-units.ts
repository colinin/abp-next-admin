import { defAbpHttp } from '/@/utils/http/abp';
import {
  OrganizationUnit,
  CreateOrganizationUnit,
  UpdateOrganizationUnit,
  GetOrganizationUnitPagedRequest,
  OrganizationUnitPagedResult,
  OrganizationUnitListResult,
} from './model/organizationUnitsModel';
import { UserPagedResult, GetUserPagedRequest } from './model/userModel';
import { RolePagedResult, GetRolePagedRequest } from './model/roleModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = '/api/identity/organization-units',
  Delete = '/api/identity/organization-units/{id}',
  Update = '/api/identity/organization-units/{id}',
  GetById = '/api/identity/organization-units​/{id}',
  GetList = '/api​/identity​/organization-units​',
  GetAllList = '/api/identity/organization-units/all',
  GetUnaddedMemberList = '/api/identity/organization-units/{id}/unadded-users',
  GetMemberList = '/api/identity/organization-units/{id}/users',
  GetUnaddedRoleList = '/api/identity/organization-units/{id}/unadded-roles',
  GetRoleList = '/api/identity/organization-units/{id}/roles',
  Move = 'api/identity/organization-units/{id}/move',
  AddMembers = '/api/identity/organization-units/{id}/users',
  AddRoles = '/api/identity/organization-units/{id}/roles',
}

export const create = (input: CreateOrganizationUnit) => {
  return defAbpHttp.post<OrganizationUnit>({
    url: Api.Create,
    data: input,
  });
};

export const update = (id: string, input: UpdateOrganizationUnit) => {
  return defAbpHttp.put<OrganizationUnit>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.Delete, { id: id }),
  });
};

export const getList = (input: GetOrganizationUnitPagedRequest) => {
  return defAbpHttp.get<OrganizationUnitPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const getUnaddedMemberList = (input: { id: string } & GetUserPagedRequest) => {
  return defAbpHttp.get<UserPagedResult>({
    url: format(Api.GetUnaddedMemberList, { id: input.id }),
    params: {
      filter: input.filter,
      sorting: input.sorting,
      skipCount: input.skipCount,
      maxResultCount: input.maxResultCount,
    },
  });
};

export const getMemberList = (id: string, input: GetUserPagedRequest) => {
  return defAbpHttp.get<UserPagedResult>({
    url: format(Api.GetMemberList, { id: id }),
    params: input,
  });
};

export const getUnaddedRoleList = (input: { id: string } & GetRolePagedRequest) => {
  return defAbpHttp.get<RolePagedResult>({
    url: format(Api.GetUnaddedRoleList, { id: input.id }),
    params: {
      filter: input.filter,
      sorting: input.sorting,
      skipCount: input.skipCount,
      maxResultCount: input.maxResultCount,
    },
  });
};

export const getRoleList = (id: string, input: GetRolePagedRequest) => {
  return defAbpHttp.get<RolePagedResult>({
    url: format(Api.GetRoleList, { id: id }),
    params: input,
  });
};

export const getAll = () => {
  return defAbpHttp.get<OrganizationUnitListResult>({
    url: Api.GetAllList,
  });
};

export const move = (id: string, parentId?: string) => {
  return defAbpHttp.put<void>({
    url: format(Api.Move, { id: id }),
    data: {
      parentId: parentId,
    },
  });
};

export const addMembers = (id: string, userIdList: string[]) => {
  return defAbpHttp.post<void>({
    url: format(Api.AddMembers, { id: id }),
    data: { userIds: userIdList },
  });
};

export const addRoles = (id: string, roleIdList: string[]) => {
  return defAbpHttp.post<void>({
    url: format(Api.AddRoles, { id: id }),
    data: { roleIds: roleIdList },
  });
};
