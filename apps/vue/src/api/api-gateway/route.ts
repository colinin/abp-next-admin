import { defAbpHttp } from '/@/utils/http/abp';
import {
  Route,
  CreateRoute,
  UpdateRoute,
  GetRoutePagedRequest,
  RoutePagedResult,
} from './model/routeModel';

enum Api {
  RemoteService = 'ApiGateway',
  Controller = 'ReRoute',
  GetList = '/api/ApiGateway/Globals',
  GetActivedList = '/api/ApiGateway/RouteGroups/Actived',
}

export const create = (input: CreateRoute) => {
  return defAbpHttp.request<Route>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'CreateAsync',
    data: input,
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.request<void>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'DeleteAsync',
    params: {
      input: {
        RouteId: id,
      },
    },
  });
};

export const getById = (id: string) => {
  return defAbpHttp.request<Route>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'GetAsync',
    params: {
      input: {
        RouteId: id,
      },
    },
  });
};

export const getList = (input: GetRoutePagedRequest) => {
  return defAbpHttp.request<RoutePagedResult>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'GetListAsync',
    params: {
      input: input,
    },
  });
};

export const update = (input: UpdateRoute) => {
  return defAbpHttp.request<Route>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'UpdateAsync',
    data: input,
  });
};
