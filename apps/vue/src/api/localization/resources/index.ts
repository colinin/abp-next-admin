import { defHttp } from '/@/utils/http/axios';
import { Resource, ResourceCreate, ResourceUpdate, GetResourceWithFilter } from './model';

export const getList = (input: GetResourceWithFilter) => {
  return defHttp.get<ListResultDto<Resource>>({
    url: '/api/abp/localization/resources',
    params: input,
  });
};

export const getByName = (name: string) => {
  return defHttp.get<Resource>({
    url: `/api/localization/resources/${name}`
  });
};

export const create = (input: ResourceCreate) => {
  return defHttp.post<Resource>({
    url: '/api/localization/resources',
    data: input,
  });
};

export const update = (name: string, input: ResourceUpdate) => {
  return defHttp.put<Resource>({
    url: `/api/localization/resources/${name}`,
    data: input,
  });
};

export const deleteByName = (name: string) => {
  return defHttp.request<void>({
    url: `/api/localization/resources/${name}`,
  });
};
