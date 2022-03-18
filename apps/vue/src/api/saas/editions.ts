import { defAbpHttp } from '/@/utils/http/abp';
import {
  Edition,
  EditionCreate,
  EditionUpdate,
  EditionGetListInput,
} from './model/editionsModel';
import { format } from '/@/utils/strings';
import { PagedResultDto } from '../model/baseModel';

enum Api {
  Create = '/api/saas/editions',
  DeleteById = '/api/saas/editions/{id}',
  GetById = '/api/saas/editions/{id}',
  GetList = '/api/saas/editions',
  Update = '/api/saas/editions/{id}',
}

export const getById = (id: string) => {
  return defAbpHttp.get<Edition>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: EditionGetListInput) => {
  return defAbpHttp.get<PagedResultDto<Edition>>({
    url: Api.GetList,
    params: input,
  });
};

export const create = (input: EditionCreate) => {
  return defAbpHttp.post<Edition>({
    url: Api.Create,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.GetById, { id: id }),
  });
};

export const update = (id: string, input: EditionUpdate) => {
  return defAbpHttp.put<Edition>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};
