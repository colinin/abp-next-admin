import { defHttp } from '/@/utils/http/axios';
import {
  CreateLayout,
  Layout,
  UpdateLayout,
  LayoutListResult,
  GetLayoutPagedRequest,
  LayoutPagedResult,
} from './model';

export const create = (input: CreateLayout) => {
  return defHttp.post<Layout>({
    url: '/api/platform/layouts',
    data: input,
  });
};

export const update = (id: string, input: UpdateLayout) => {
  return defHttp.put<Layout>({
    url: `/api/platform/layouts/${id}`,
    data: input,
  });
};

export const get = (id: string) => {
  return defHttp.get<Layout>({
    url: `/api/platform/layouts/${id}`,
  });
};

export const getAll = () => {
  return defHttp.get<LayoutListResult>({
    url: '/api/platform/layouts/all',
  });
};

export const getList = (input: GetLayoutPagedRequest) => {
  return defHttp.get<LayoutPagedResult>({
    url: '/api/platform/layouts',
    params: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/platform/layouts/${id}`,
  });
};
