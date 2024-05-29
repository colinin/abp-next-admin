import { defHttp } from '/@/utils/http/axios';
import {
  ApiResource,
  ApiResourceCreate,
  ApiResourceUpdate,
  GetApiResourcePagedRequest,
} from './model';

export const create = (input: ApiResourceCreate) => {
  return defHttp.post<ApiResource>({
    url: '/api/identity-server/api-resources',
    data: input,
  });
};

export const update = (id: string, input: ApiResourceUpdate) => {
  return defHttp.put<ApiResource>({
    url: `/api/identity-server/api-resources/${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/identity-server/api-resources/${id}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<ApiResource>({
    url: `/api/identity-server/api-resources/${id}`,
  });
};

export const getList = (input: GetApiResourcePagedRequest) => {
  return defHttp.get<PagedResultDto<ApiResource>>({
    url: '/api/identity-server/api-resources',
    params: input,
  });
};
