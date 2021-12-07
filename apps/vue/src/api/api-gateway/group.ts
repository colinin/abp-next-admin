import { defAbpHttp } from '/@/utils/http/abp';
import {
  RouteGroup,
  CreateRouteGroup,
  UpdateRouteGroup,
  RouteGroupListResult,
  GetRouteGroupPagedRequest,
  RouteGroupPagedResult,
} from './model/groupModel';

enum Api {
  RemoteService = 'ApiGateway',
  Controller = 'RouteGroup',
  GetList = '/api/ApiGateway/RouteGroups',
  GetActivedList = '/api/ApiGateway/RouteGroups/Actived',
}

export const create = (input: CreateRouteGroup) => {
  return defAbpHttp.request<RouteGroup>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'CreateAsync',
    data: input,
  });
};

export const update = (input: UpdateRouteGroup) => {
  return defAbpHttp.request<RouteGroup>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'UpdateAsync',
    data: input,
  });
};

export const getByAppId = (appId: string) => {
  return defAbpHttp.request<RouteGroup>({
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

export const getActivedList = () => {
  return defAbpHttp.get<RouteGroupListResult>({
    url: Api.GetList,
  });
};

export const getList = (input: GetRouteGroupPagedRequest) => {
  return defAbpHttp.get<RouteGroupPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const deleteByAppId = (appId: string) => {
  return defAbpHttp.request<RouteGroup>({
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
