import { defAbpHttp } from '/@/utils/http/abp';
import { AuditLog, GetAuditLogPagedRequest } from './model/auditLogModel';

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
      input: {
        id: id,
      },
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
