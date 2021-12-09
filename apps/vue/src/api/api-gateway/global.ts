import { defAbpHttp } from '/@/utils/http/abp';
import {
  GlobalConfiguration,
  CreateGlobalConfiguration,
  UpdateGlobalConfiguration,
  GetGlobalPagedRequest,
  GlobalConfigurationPagedResult,
} from './model/globalModel';

enum Api {
  RemoteService = 'ApiGateway',
  Controller = 'GlobalConfiguration',
  GetList = '/api/ApiGateway/Globals',
  GetActivedList = '/api/ApiGateway/RouteGroups/Actived',
}

export const create = (input: CreateGlobalConfiguration) => {
  return defAbpHttp.request<GlobalConfiguration>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'CreateAsync',
    data: input,
  });
};

export const update = (input: UpdateGlobalConfiguration) => {
  return defAbpHttp.request<GlobalConfiguration>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'UpdateAsync',
    data: input,
  });
};

export const getByAppId = (appId: string) => {
  return defAbpHttp.request<GlobalConfiguration>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'GetAsync',
    params: {
      input: {
        appId: appId,
      },
    },
  });
};

export const getList = (input: GetGlobalPagedRequest) => {
  return defAbpHttp.get<GlobalConfigurationPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const deleteByAppId = (appId: string) => {
  return defAbpHttp.request<GlobalConfiguration>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'DeleteAsync',
    params: {
      input: {
        appId: appId,
      },
    },
  });
};
