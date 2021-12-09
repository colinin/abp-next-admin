import { defAbpHttp } from '/@/utils/http/abp';
import { SecurityLog, GetSecurityLogPagedRequest } from './model/securityLogModel';

enum Api {
  RemoteService = 'AbpAuditing',
  Controller = 'SecurityLog',
}

export const deleteById = (id: string) => {
  return defAbpHttp.pagedRequest<SecurityLog>({
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
  return defAbpHttp.pagedRequest<SecurityLog>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'GetAsync',
    params: {
      input: {
        id: id,
      },
    },
  });
};

export const getList = (input: GetSecurityLogPagedRequest) => {
  return defAbpHttp.pagedRequest<SecurityLog>({
    service: Api.RemoteService,
    controller: Api.Controller,
    action: 'GetListAsync',
    params: {
      input: input,
    },
  });
};
