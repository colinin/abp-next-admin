import { defHttp } from '/@/utils/http/axios';
import {
  OrganizationUnit,
  CreateOrganizationUnit,
  UpdateOrganizationUnit,
  GetOrganizationUnitPagedRequest,
} from './model';
import { GetUserPagedRequest, User } from '../users/model';
import { GetRolePagedRequest, Role } from '../roles/model';


export const create = (input: CreateOrganizationUnit) => {
  return defHttp.post<OrganizationUnit>({
    url: '/api/identity/organization-units',
    data: input,
  });
};

export const update = (id: string, input: UpdateOrganizationUnit) => {
  return defHttp.put<OrganizationUnit>({
    url: `/api/identity/organization-units/${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity/organization-units/${id}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<OrganizationUnit>({
    url: `/api/identity/organization-units/${id}`,
  });
}

export const getList = (input: GetOrganizationUnitPagedRequest) => {
  return defHttp.get<PagedResultDto<OrganizationUnit>>({
    url: '/api/identity/organization-units',
    params: input,
  });
};

export const getUnaddedMemberList = (input: { id: string } & GetUserPagedRequest) => {
  return defHttp.get<PagedResultDto<User>>({
    url: `/api/identity/organization-units/${input.id}/unadded-users`,
    params: {
      filter: input.filter,
      sorting: input.sorting,
      skipCount: input.skipCount,
      maxResultCount: input.maxResultCount,
    },
  });
};

export const getMemberList = (id: string, input: GetUserPagedRequest) => {
  return defHttp.get<PagedResultDto<User>>({
    url: `/api/identity/organization-units/${id}/users`,
    params: input,
  });
};

export const getUnaddedRoleList = (input: { id: string } & GetRolePagedRequest) => {
  return defHttp.get<PagedResultDto<Role>>({
    url: `/api/identity/organization-units/${input.id}/unadded-roles`,
    params: {
      filter: input.filter,
      sorting: input.sorting,
      skipCount: input.skipCount,
      maxResultCount: input.maxResultCount,
    },
  });
};

export const getRoleList = (id: string, input: GetRolePagedRequest) => {
  return defHttp.get<PagedResultDto<Role>>({
    url: `/api/identity/organization-units/${id}/roles`,
    params: input,
  });
};

export const getAll = () => {
  return defHttp.get<ListResultDto<OrganizationUnit>>({
    url: '/api/identity/organization-units/all',
  });
};

export const move = (id: string, parentId?: string) => {
  return defHttp.put<void>({
    url: `/api/identity/organization-units/${id}/move`,
    data: {
      parentId: parentId,
    },
  });
};

export const addMembers = (id: string, userIdList: string[]) => {
  return defHttp.post<void>({
    url: `/api/identity/organization-units/${id}/users`,
    data: { userIds: userIdList },
  });
};

export const addRoles = (id: string, roleIdList: string[]) => {
  return defHttp.post<void>({
    url: `/api/identity/organization-units/${id}/roles`,
    data: { roleIds: roleIdList },
  });
};
