import { defHttp } from '/@/utils/http/axios';
import {
  IdentityClaimType,
  CreateIdentityClaimType,
  IdentityClaimTypeListResult,
  UpdateIdentityClaimType,
  GetIdentityClaimTypePagedRequest,
} from './model';

export const create = (input: CreateIdentityClaimType) => {
  return defHttp.post<IdentityClaimType>({
    url: '/api/identity/claim-types',
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity/claim-types/${id}`,
  });
};

export const update = (id: string, input: UpdateIdentityClaimType) => {
  return defHttp.put<IdentityClaimType>({
    url: `/api/identity/claim-types/${id}`,
    data: input,
  });
};

export const getById = (id: string) => {
  return defHttp.get<IdentityClaimType>({
    url: `/api/identity/claim-types/${id}'`,
  });
};

export const getList = (input: GetIdentityClaimTypePagedRequest) => {
  return defHttp.get<IdentityClaimType>({
    url: '/api/identity/claim-types',
    params: input,
  });
};

export const getActivedList = () => {
  return defHttp.get<IdentityClaimTypeListResult>({
    url: '/api/identity/claim-types/actived-list',
  });
};
