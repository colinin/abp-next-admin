import { defHttp } from '/@/utils/http/axios';
import {
  Role,
  GetRolePagedRequest,
  UpdateRole,
  CreateRole,
  RoleClaim,
} from './model';
import { CreateIdentityClaim, UpdateIdentityClaim } from '../claims/model';

export const create = (input: CreateRole) => {
  return defHttp.post<Role>({
    url: '/api/identity/roles',
    data: input,
  });
};

export const createClaim = (id: string, input: CreateIdentityClaim) => {
  return defHttp.post<void>({
    url: `/api/identity/roles/${id}/claims`,
    data: input,
  });
};

export const update = (id: string, input: UpdateRole) => {
  return defHttp.put<Role>({
    url: `/api/identity/roles/${id}`,
    data: input,
  });
};

export const updateClaim = (id: string, input: UpdateIdentityClaim) => {
  return defHttp.put<void>({
    url: `/api/identity/roles/${id}/claims`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity/roles/${id}`,
  });
};

export const deleteClaim = (id: string, input: RoleClaim) => {
  return defHttp.delete<void>(
    {
      url: `/api/identity/roles/${id}/claims`,
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
  return defHttp.get<Role>({
    url: `/api/identity/roles/${id}`,
  });
};

export const getAllList = () => {
  return defHttp.get<ListResultDto<Role>>({
    url: '/api/identity/roles/all',
  });
};

export const getClaimList = (input: { id: string }) => {
  return defHttp.get<ListResultDto<RoleClaim>>({
    url: `/api/identity/roles/${input.id}/claims`,
  });
};

export const getList = (input: GetRolePagedRequest) => {
  return defHttp.get<PagedResultDto<Role>>({
    url: '/api/identity/roles',
    params: input,
  });
};

export const removeOrganizationUnit = (id: string, ouId: string) => {
  return defHttp.delete<void>({
    url: `/api/identity/roles/${id}/organization-units/${ouId}`,
  });
};
