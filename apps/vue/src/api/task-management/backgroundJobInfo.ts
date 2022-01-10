import { defAbpHttp } from '/@/utils/http/abp';
import {
  BackgroundJobInfo,
  BackgroundJobInfoCreate,
  BackgroundJobInfoUpdate,
  BackgroundJobInfoGetListInput,
} from './model/backgroundJobInfoModel';
import { format } from '/@/utils/strings';
import { PagedResultDto } from '../model/baseModel';

enum Api {
  GetById = '/api/task-management/background-jobs/{id}',
  GetList = '/api/task-management/background-jobs',
  Create = '/api/task-management/background-jobs',
  Update = '/api/task-management/background-jobs/{id}',
  Delete = '/api/task-management/background-jobs/{id}',
  Pause = '/api/task-management/background-jobs/{id}/pause',
  Resume = '/api/task-management/background-jobs/{id}/resume',
  Trigger = '/api/task-management/background-jobs/{id}/trigger',
  Stop = '/api/task-management/background-jobs/{id}/stop',
}

export const getById = (id: string) => {
  return defAbpHttp.get<BackgroundJobInfo>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: BackgroundJobInfoGetListInput) => {
  return defAbpHttp.get<PagedResultDto<BackgroundJobInfo>>({
    url: Api.GetList,
    params: input,
  });
};

export const create = (input: BackgroundJobInfoCreate) => {
  return defAbpHttp.post<BackgroundJobInfo>({
    url: Api.Create,
    data: input,
  });
};

export const update = (id: string, input: BackgroundJobInfoUpdate) => {
  return defAbpHttp.put<BackgroundJobInfo>({
    url: format(Api.Update, { id: id }),
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.Delete, { id: id }),
  });
};

export const pause = (id: string) => {
  return defAbpHttp.put<void>({
    url: format(Api.Pause, { id: id }),
  });
};

export const resume = (id: string) => {
  return defAbpHttp.put<void>({
    url: format(Api.Resume, { id: id }),
  });
};

export const trigger = (id: string) => {
  return defAbpHttp.put<void>({
    url: format(Api.Trigger, { id: id }),
  });
};

export const stop = (id: string) => {
  return defAbpHttp.put<void>({
    url: format(Api.Stop, { id: id }),
  });
};
