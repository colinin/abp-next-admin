import { defHttp } from '/@/utils/http/axios';
import { format } from '/@/utils/strings';
import {
  CreateLayout,
  Layout,
  UpdateLayout,
  LayoutListResult,
  GetLayoutPagedRequest,
  LayoutPagedResult,
} from './model/layoutModel';

enum Api {
  GetById = '/api/platform/layouts/{id}',
  GetList = '/api/platform/layouts',
  GetAll = '/api/platform/layouts/all',
  Create = '/api/platform/layouts',
  Delete = '/api/platform/layouts/{id}',
  Update = '/api/platform/layouts/{id}',
}

export const create = (input: CreateLayout) => {
  return defHttp.post<Layout>({
    url: Api.Create,
    data: input,
  });
};

export const update = (id: string, input: UpdateLayout) => {
  return defHttp.put<Layout>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const get = (id: string) => {
  return defHttp.get<Layout>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getAll = () => {
  return defHttp.get<LayoutListResult>({
    url: Api.GetAll,
  });
};

export const getList = (input: GetLayoutPagedRequest) => {
  return defHttp.get<LayoutPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: format(Api.Delete, { id: id }),
  });
};
