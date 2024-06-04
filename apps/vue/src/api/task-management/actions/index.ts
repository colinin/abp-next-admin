import { defHttp } from '/@/utils/http/axios';
import {
  BackgroundJobAction,
  BackgroundJobActionDefinition,
  CreateBackgroundJobAction,
  UpdateBackgroundJobAction,
  BackgroundJobActionGetDefinitionsInput
} from './model';

export const addAction = (jobId: string, input: CreateBackgroundJobAction) => {
  return defHttp.post<BackgroundJobAction>({
    url: `/api/task-management/background-jobs/actions/${jobId}`,
    data: input,
  });
};

export const updateAction = (id: string, input: UpdateBackgroundJobAction) => {
  return defHttp.put<BackgroundJobAction>({
    url: `/api/task-management/background-jobs/actions/${id}`,
    data: input,
  });
};

export const deleteAction = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/task-management/background-jobs/actions/${id}`,
  });
};

export const getActions = (jobId: string) => {
  return defHttp.get<ListResultDto<BackgroundJobAction>>({
    url: `/api/task-management/background-jobs/actions/${jobId}`,
  });
};

export const getDefinitions = (input: BackgroundJobActionGetDefinitionsInput) => {
  return defHttp.get<ListResultDto<BackgroundJobActionDefinition>>({
    url: '/api/task-management/background-jobs/actions/definitions',
    params: input,
  });
}; 
