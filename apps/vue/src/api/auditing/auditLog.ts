import { defAbpHttp } from '/@/utils/http/abp';
import {
  AuditLog,
  EntityChange,
  EntityChangeWithUsername,
  GetAuditLogPagedRequest,
  EntityChangeGetByPagedRequest,
  EntityChangeGetWithUsernameInput
} from './model/auditLogModel';
import { ListResultDto } from '../model/baseModel';

enum Api {
  RemoteService = 'AbpAuditing',
  Controller = 'AuditLog',
}

export const deleteById = (id: string) => {
  return defAbpHttp.pagedRequest<void>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'DeleteAsync',
    params: {
      id: id,
    },
  });
};

export const getById = (id: string) => {
  return defAbpHttp.request<AuditLog>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'GetAsync',
    params: {
      id: id,
    },
  });
};

export const getList = (input: GetAuditLogPagedRequest) => {
  return defAbpHttp.pagedRequest<AuditLog>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'GetListAsync',
    params: {
      input: input,
    },
  });
};

export const getEntityChanges = (input: EntityChangeGetByPagedRequest) => {
  return defAbpHttp.pagedRequest<EntityChange>({
    service: Api.RemoteService,
    controller: 'EntityChanges',
    action: 'GetListAsync',
    params: {
      input: input,
    },
  });
}

export const getEntityChangesWithUsername = (input: EntityChangeGetWithUsernameInput) => {
  return defAbpHttp.get<ListResultDto<EntityChangeWithUsername>>({
    url: '/api/auditing/entity-changes/with-username',
    params: input,
  });
}
