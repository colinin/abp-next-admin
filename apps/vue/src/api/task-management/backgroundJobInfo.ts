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
  Start = '/api/task-management/background-jobs/{id}/start',
  Stop = '/api/task-management/background-jobs/{id}/stop',
  BulkPause = '/api/task-management/background-jobs/bulk-pause',
  BulkResume = '/api/task-management/background-jobs/bulk-resume',
  BulkTrigger = '/api/task-management/background-jobs/bulk-trigger',
  BulkStart = '/api/task-management/background-jobs/bulk-start',
  BulkStop = '/api/task-management/background-jobs/bulk-stop',
  BulkDelete = '/api/task-management/background-jobs/bulk-delete',
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

export const start = (id: string) => {
  return defAbpHttp.put<void>({
    url: format(Api.Stop, { id: id }),
  });
};

export const stop = (id: string) => {
  return defAbpHttp.put<void>({
    url: format(Api.Stop, { id: id }),
  });
};

export const bulkPause = (ids: string[]) => {
  return defAbpHttp.put<void>({
    url: Api.BulkPause,
    data: {
      jobIds: ids,
    },
  });
};

export const bulkResume = (ids: string[]) => {
  return defAbpHttp.put<void>({
    url: Api.BulkResume,
    data: {
      jobIds: ids,
    },
  });
};

export const bulkTrigger = (ids: string[]) => {
  return defAbpHttp.put<void>({
    url: Api.BulkTrigger,
    data: {
      jobIds: ids,
    },
  });
};

export const bulkStart = (ids: string[]) => {
  return defAbpHttp.put<void>({
    url: Api.BulkStart,
    data: {
      jobIds: ids,
    },
  });
};

export const bulkStop = (ids: string[]) => {
  return defAbpHttp.put<void>({
    url: Api.BulkStop,
    data: {
      jobIds: ids,
    },
  });
};

export const bulkDelete = (ids: string[]) => {
  return defAbpHttp.put<void>({
    url: Api.BulkDelete,
    data: {
      jobIds: ids,
    },
  });
};
