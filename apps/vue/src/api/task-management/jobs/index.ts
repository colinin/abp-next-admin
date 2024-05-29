import { defHttp } from '/@/utils/http/axios';
import {
  BackgroundJobInfo,
  BackgroundJobInfoCreate,
  BackgroundJobInfoUpdate,
  BackgroundJobDefinition,
  BackgroundJobInfoGetListInput,
} from './model';
import { DefineParamter, DynamicQueryable } from '/@/components/Table/src/types/advancedSearch';

export const getById = (id: string) => {
  return defHttp.get<BackgroundJobInfo>({
    url: `/api/task-management/background-jobs/${id}`,
  });
};

export const getList = (input: BackgroundJobInfoGetListInput) => {
  return defHttp.get<PagedResultDto<BackgroundJobInfo>>({
    url: '/api/task-management/background-jobs',
    params: input,
  });
};

export const getAvailableFields = () => {
  return defHttp.get<ListResultDto<DefineParamter>>({
    url: '/api/task-management/background-jobs/available-fields',
  });
}

export const advancedSearch = (input: DynamicQueryable) => {
  return defHttp.post<PagedResultDto<BackgroundJobInfo>>({
    url: '/api/task-management/background-jobs/search',
    data: input,
  });
}

export const getDefinitions = () => {
  return defHttp.get<ListResultDto<BackgroundJobDefinition>>({
    url: '/api/task-management/background-jobs/definitions',
  });
}; 

export const create = (input: BackgroundJobInfoCreate) => {
  return defHttp.post<BackgroundJobInfo>({
    url: '/api/task-management/background-jobs',
    data: input,
  });
};

export const update = (id: string, input: BackgroundJobInfoUpdate) => {
  return defHttp.put<BackgroundJobInfo>({
    url: `/api/task-management/background-jobs/${id}`,
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/task-management/background-jobs/${id}`,
  });
};

export const pause = (id: string) => {
  return defHttp.put<void>({
    url: `/api/task-management/background-jobs/${id}/pause`,
  });
};

export const resume = (id: string) => {
  return defHttp.put<void>({
    url: `/api/task-management/background-jobs/${id}/resume`,
  });
};

export const trigger = (id: string) => {
  return defHttp.put<void>({
    url: `/api/task-management/background-jobs/${id}/trigger`,
  });
};

export const start = (id: string) => {
  return defHttp.put<void>({
    url: `/api/task-management/background-jobs/${id}/start`,
  });
};

export const stop = (id: string) => {
  return defHttp.put<void>({
    url: `/api/task-management/background-jobs/${id}/stop`,
  });
};

export const bulkPause = (ids: string[]) => {
  return defHttp.put<void>({
    url: '/api/task-management/background-jobs/bulk-pause',
    data: {
      jobIds: ids,
    },
  });
};

export const bulkResume = (ids: string[]) => {
  return defHttp.put<void>({
    url: '/api/task-management/background-jobs/bulk-resume',
    data: {
      jobIds: ids,
    },
  });
};

export const bulkTrigger = (ids: string[]) => {
  return defHttp.put<void>({
    url: '/api/task-management/background-jobs/bulk-trigger',
    data: {
      jobIds: ids,
    },
  });
};

export const bulkStart = (ids: string[]) => {
  return defHttp.put<void>({
    url: '/api/task-management/background-jobs/bulk-start',
    data: {
      jobIds: ids,
    },
  });
};

export const bulkStop = (ids: string[]) => {
  return defHttp.put<void>({
    url: '/api/task-management/background-jobs/bulk-stop',
    data: {
      jobIds: ids,
    },
  });
};

export const bulkDelete = (ids: string[]) => {
  return defHttp.put<void>({
    url: '/api/task-management/background-jobs/bulk-delete',
    data: {
      jobIds: ids,
    },
  });
};
