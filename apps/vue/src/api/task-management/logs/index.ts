import { defHttp } from '/@/utils/http/axios';
import {
  BackgroundJobLog,
  BackgroundJobLogGetListInput,
} from './model';

export const getById = (id: string) => {
  return defHttp.get<BackgroundJobLog>({
    url: `/api/task-management/background-jobs/logs/${id}`,
  });
};

export const getList = (input: BackgroundJobLogGetListInput) => {
  return defHttp.get<PagedResultDto<BackgroundJobLog>>({
    url: '/api/task-management/background-jobs/logs',
    params: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/task-management/background-jobs/logs/${id}`,
  });
};
