import { defHttp } from '/@/utils/http/axios';
import {
  CacheKeys,
  CacheValue,
  CacheRefreshRequest,
  GetCacheKeysRequest,
} from './model';

export const getKeys = (input: GetCacheKeysRequest) => {
  return defHttp.get<CacheKeys>({
    url: '/api/caching-management/cache/keys',
    params: input,
  });
};

export const getValue = (key: string) => {
  return defHttp.get<CacheValue>({
    url: `/api/caching-management/cache/value?key=${key}`,
  });
};

export const refresh = (input: CacheRefreshRequest) => {
  return defHttp.put<void>({
    url: `/api/caching-management/cache/refresh`,
    data: input,
  });
};

export const remove = (key: string) => {
  return defHttp.delete<void>({
    url: `/api/caching-management/cache/remove?key=${key}`,
  });
};
