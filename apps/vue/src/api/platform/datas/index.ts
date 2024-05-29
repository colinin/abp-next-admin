import { defHttp } from '/@/utils/http/axios';
import {
  Data,
  GetDataByPaged,
  DataPagedResult,
  DataListResult,
  CreateData,
  UpdateData,
  CreateDataItem,
  UpdateDataItem,
} from './model';

export const create = (input: CreateData) => {
  return defHttp.post<Data>({
    url: '/api/platform/datas',
    data: input,
  });
};

export const createItem = (id: string, input: CreateDataItem) => {
  return defHttp.post<void>({
    url: `/api/platform/datas/${id}/items`,
    data: input,
  });
};

export const update = (id: string, input: UpdateData) => {
  return defHttp.put<Data>({
    url: `/api/platform/datas/${id}`,
    data: input,
  });
};

export const updateItem = (id: string, name: string, input: UpdateDataItem) => {
  return defHttp.put<void>({
    url: `/api/platform/datas/${id}/items/${name}`,
    data: input,
  });
};

export const remove = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/platform/datas/${id}`,
  });
};

export const removeItem = (id: string, name: string) => {
  return defHttp.delete<void>({
    url: `/api/platform/datas/${id}/items/${name}`,
  });
};

export const get = (id: string) => {
  return defHttp.get<Data>({
    url: `/api/platform/datas/${id}`,
  });
};

export const getByName = (name: string) => {
  return defHttp.get<Data>({
    url: `/api/platform/datas/by-name/${name}`,
  });
};

export const getList = (input: GetDataByPaged) => {
  return defHttp.get<DataPagedResult>({
    url: '/api/platform/datas',
    params: input,
  });
};

export const getAll = () => {
  return defHttp.get<DataListResult>({
    url: '/api/platform/datas/all',
  });
};
