import { defAbpHttp } from '/@/utils/http/abp';
import {
  CacheKeys,
  CacheValue,
  CacheRefreshRequest,
  GetCacheKeysRequest,
} from './model';

const remoteServiceName = 'CachingManagement';
const controllerName = 'Cache';

export const getKeys = (input: GetCacheKeysRequest) => {
  return defAbpHttp.request<CacheKeys>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetKeysAsync',
    params: {
      input: input,
    },
  });
};

export const getValue = (key: string) => {
  return defAbpHttp.request<CacheValue>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetValueAsync',
    params: {
      input: {
        key: key,
      }
    },
  });
};

export const refresh = (input: CacheRefreshRequest) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'RefreshAsync',
    data: input,
  });
};

export const remove = (key: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'RemoveAsync',
    params: {
      input: {
        key: key,
      }
    },
  });
};
