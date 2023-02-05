import { defAbpHttp } from '/@/utils/http/abp';
import { ResourceListResult, Resource, ResourceCreate, ResourceUpdate } from './model/resourcesModel';

const remoteServiceName = 'LocalizationManagement';
const controllerName = 'Resource';

enum Api {
  GetList = '/api/abp/localization/resources',
}

export const getList = () => {
  return defAbpHttp.get<ResourceListResult>({
    url: Api.GetList,
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
