import { defAbpHttp } from '/@/utils/http/abp';
import {
  BackgroundJobAction,
  BackgroundJobActionDefinition,
  CreateBackgroundJobAction,
  UpdateBackgroundJobAction,
  BackgroundJobActionGetDefinitionsInput
} from './model/backgroundJobActionModel';
import { format } from '/@/utils/strings';
import { ListResultDto } from '../model/baseModel';

enum Api {
  AddAction = '/api/task-management/background-jobs/actions/{jobId}',
  UpdateAction = '/api/task-management/background-jobs/actions/{id}',
  DeleteAction = '/api/task-management/background-jobs/actions/{id}',
  GetActions = '/api/task-management/background-jobs/actions/{jobId}',
  GetDefinitions = '/api/task-management/background-jobs/actions/definitions',
}

export const addAction = (jobId: string, input: CreateBackgroundJobAction) => {
  return defAbpHttp.post<BackgroundJobAction>({
    url: format(Api.AddAction, { jobId: jobId }),
    data: input,
  });
};

export const updateAction = (id: string, input: UpdateBackgroundJobAction) => {
  return defAbpHttp.put<BackgroundJobAction>({
    url: format(Api.UpdateAction, { id: id }),
    data: input,
  });
};

export const deleteAction = (id: string) => {
  return defAbpHttp.delete<void>({
    url: format(Api.DeleteAction, { id: id }),
  });
};

export const getActions = (jobId: string) => {
  return defAbpHttp.get<ListResultDto<BackgroundJobAction>>({
    url: format(Api.GetActions, { jobId: jobId }),
  });
};

export const getDefinitions = (input: BackgroundJobActionGetDefinitionsInput) => {
  return defAbpHttp.get<ListResultDto<BackgroundJobActionDefinition>>({
    url: Api.GetDefinitions,
    params: input,
  });
}; 
