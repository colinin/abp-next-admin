import { defAbpHttp } from '/@/utils/http/abp';
import { ResourceListResult, Resource, ResourceCreate, ResourceUpdate, GetWithFilter } from './model/resourcesModel';
import { format } from '/@/utils/strings';

const remoteServiceName = 'LocalizationManagement';
const controllerName = 'Resource';

enum Api {
  GetList = '/api/abp/localization/resources?filter={filter}',
}

export const getList = (input: GetWithFilter) => {
  return defAbpHttp.get<ResourceListResult>({
    url: format(Api.GetList, input),
  });
};

export const GetAsyncByName = (name: string) => {
  return defAbpHttp.request<Resource>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    uniqueName: 'GetAsyncByName',
    params: {
      name: name,
    },
  });
};

export const CreateAsyncByInput = (input: ResourceCreate) => {
  return defAbpHttp.request<Resource>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    uniqueName: 'CreateAsyncByInput',
    data: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: ResourceUpdate) => {
  return defAbpHttp.request<Resource>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UpdateAsync',
    uniqueName: 'UpdateAsyncByNameAndInput',
    params: {
      name: name,
    },
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteAsync',
    uniqueName: 'DeleteAsyncByName',
    params: {
      name: name,
    },
  });
};
