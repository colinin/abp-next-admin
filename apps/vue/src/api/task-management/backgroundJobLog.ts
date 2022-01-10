import { defAbpHttp } from '/@/utils/http/abp';
import {
  BackgroundJobLog,
  BackgroundJobLogGetListInput,
} from './model/backgroundJobLogModel';
import { format } from '/@/utils/strings';
import { PagedResultDto } from '../model/baseModel';

enum Api {
  GetById = '/api/task-management/background-jobs/logs/{id}',
  GetList = '/api/task-management/background-jobs/logs',
  Delete = '/api/task-management/background-jobs/logs/{id}',
}

export const getById = (id: string) => {
  return defAbpHttp.get<BackgroundJobLog>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: BackgroundJobLogGetListInput) => {
  return defAbpHttp.get<PagedResultDto<BackgroundJobLog>>({
    url: Api.GetList,
    params: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.Delete, { id: id }),
  });
};
