import { defHttp } from '/@/utils/http/axios';
import {
  User,
  CreateUser,
  SetPassword,
  UpdateUser,
  GetUserPagedRequest,
  UserClaim,
  IdentityUserOrganizationUnitUpdateDto,
} from './model';
import { CreateIdentityClaim, UpdateIdentityClaim } from '../claims/model';
import { Role } from '../roles/model';
import { OrganizationUnit } from '../organization-units/model';

export const create = (input: CreateUser) => {
  return defHttp.post<User>({
    url: '/api/identity/users',
    data: input,
  });
};

export const createClaim = (id: string, input: CreateIdentityClaim) => {
  return defHttp.post<void>({
    url: `/api/identity/users/${id}/claims`,
    data: input,
  });
};

export const changePassword = (id: string, input: SetPassword) => {
  return defHttp.put<void>({
    url: `/api/identity/users/change-password?id=${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity/users/${id}`,
  });
};

export const deleteClaim = (id: string, input: UserClaim) => {
  return defHttp.delete<void>({
    url: `/api/identity/users/${id}/claims`,
    params: {
     claimType: input.claimType,
     claimValue: input.claimValue,
    },
  },{
    joinParamsToUrl: true,
  });
};

export const getById = (id: string) => {
  return defHttp.get<User>({
    url: `/api/identity/users/${id}`,
  });
};

export const getAssignableRoles = () => {
  return defHttp.get<ListResultDto<Role>>({
    url: '/api/identity/users/assignable-roles',
  });
};

export const getRoleList = (id: string) => {
  return defHttp.get<ListResultDto<Role>>({
    url: `/api/identity/users/${id}/roles`,
  });
};

export const getClaimList = (input: { id: string }) => {
  return defHttp.get<ListResultDto<UserClaim>>({
    url: `/api/identity/users/${input.id}/claims`,
  });
};

export const getList = (input: GetUserPagedRequest) => {
  return defHttp.get<PagedResultDto<User>>({
    url: '/api/identity/users',
    params: input,
  });
};

export const update = (id: string, input: UpdateUser) => {
  return defHttp.put<User>({
    url: `/api/identity/users/${id}`,
    data: input,
  });
};

export const updateClaim = (id: string, input: UpdateIdentityClaim) => {
  return defHttp.put<void>({
    url: `/api/identity/users/${id}/claims`,
    data: input,
  });
};

export const lock = (id: string, seconds: number) => {
  return defHttp.put<void>({
    url: `/api/identity/users/${id}/lock/${seconds}`,
  });
};

export const unlock = (id: string) => {
  return defHttp.put<void>({
    url: `/api/identity/users/${id}/unlock`,
  });
};

export const getOrganizationUnits = (id: string) => {
  return defHttp.get<ListResultDto<OrganizationUnit>>({
    url: `/api/identity/users/${id}/organization-units`,
  });
};

export const setOrganizationUnits = (id: string, input: IdentityUserOrganizationUnitUpdateDto) => {
  return defHttp.put<ListResultDto<OrganizationUnit>>({
    url: `/api/identity/users/${id}/organization-units`,
    data: input,
  });
};

export const removeOrganizationUnit = (id: string, ouId: string) => {
  return defHttp.delete<void>({
    url: `/api/identity/users/${id}/organization-units/${ouId}`,
  });
};
