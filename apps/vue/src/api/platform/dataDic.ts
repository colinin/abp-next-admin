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
} from './model/dataModel';
import { format } from '/@/utils/strings';

enum Api {
  GetById = '/api/platform/datas/{id}',
  GetByName = '/api/platform/datas/by-name/{name}',
  GetList = '/api/platform/datas',
  GetAll = '/api/platform/datas/all',
  Create = '/api/platform/datas',
  CreateItem = '/api/platform/datas/{id}/items',
  Delete = '/api/platform/datas/{id}',
  DeleteItem = '/api/platform/datas/{id}/items/{name}',
  Update = '/api/platform/datas/{id}',
  UpdateItem = '/api/platform/datas/{id}/items/{name}',
}

export const create = (input: CreateData) => {
  return defHttp.post<Data>({
    url: Api.Create,
    data: input,
  });
};

export const createItem = (id: string, input: CreateDataItem) => {
  return defHttp.post<void>({
    url: format(Api.CreateItem, { id: id }),
    data: input,
  });
};

export const update = (id: string, input: UpdateData) => {
  return defHttp.put<Data>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const updateItem = (id: string, name: string, input: UpdateDataItem) => {
  return defHttp.put<void>({
    url: format(Api.UpdateItem, { id: id, name: name }),
    data: input,
  });
};

export const remove = (id: string) => {
  return defHttp.delete<void>({
    url: format(Api.Delete, { id: id }),
  });
};

export const removeItem = (id: string, name: string) => {
  return defHttp.delete<void>({
    url: format(Api.DeleteItem, { id: id, name: name }),
  });
};

export const get = (id: string) => {
  return defHttp.get<Data>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getByName = (name: string) => {
  return defHttp.get<Data>({
    url: format(Api.GetByName, { name: name }),
  });
};

export const getList = (input: GetDataByPaged) => {
  return defHttp.get<DataPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const getAll = () => {
  return defHttp.get<DataListResult>({
    url: Api.GetAll,
  });
};
