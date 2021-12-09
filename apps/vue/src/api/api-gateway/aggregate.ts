import { defAbpHttp } from '/@/utils/http/abp';
import {
  AggregateRoute,
  CreateAggregateRoute,
  CreateAggregateRouteConfig,
  UpdateAggregateRoute,
  GetAggregateRoutePagedRequest,
  AggregateRoutePagedResult,
} from './model/aggregateModel';
import { format } from '/@/utils/strings';

enum Api {
  RemoteService = 'ApiGateway',
  Controller = 'AggregateReRoute',
  GetById = '/api/ApiGateway/Aggregates/{routeId}',
  GetList = '/api/ApiGateway/Globals',
  GetActivedList = '/api/ApiGateway/RouteGroups/Actived',
}

export const create = (input: CreateAggregateRoute) => {
  return defAbpHttp.request<AggregateRoute>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'CreateAsync',
    data: input,
  });
};

export const createConfig = (input: CreateAggregateRouteConfig) => {
  return defAbpHttp.request<AggregateRoute>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'AddRouteConfigAsync',
    data: input,
  });
};

export const deleteById = (routeId: string) => {
  return defAbpHttp.request<void>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'DeleteAsync',
    params: {
      input: {
        RouteId: routeId,
      },
    },
  });
};

export const deleteConfig = (routeId: string, routeKey: string) => {
  return defAbpHttp.request<void>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'DeleteAsync',
    params: {
      input: {
        RouteId: routeId,
        ReRouteKey: routeKey,
      },
    },
  });
};

export const getById = (id: string) => {
  return defAbpHttp.get<AggregateRoute>({
    url: format(Api.GetById, { routeId: id }),
  });
};

export const getList = (input: GetAggregateRoutePagedRequest) => {
  return defAbpHttp.request<AggregateRoutePagedResult>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'GetPagedListAsync',
    params: {
      input: input,
    },
  });
};

export const update = (input: UpdateAggregateRoute) => {
  return defAbpHttp.request<AggregateRoute>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'UpdateAsync',
    data: input,
  });
};
