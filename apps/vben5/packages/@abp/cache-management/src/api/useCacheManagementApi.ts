import type {
  CacheKeyInput,
  CacheKeysDto,
  CacheRefreshInput,
  CacheRemoveKeysInput,
  CacheSetInput,
  CacheValueDto,
  GetCacheKeysInput,
} from '../types';

import { useRequest } from '@abp/request';

/** 缓存管理API接口 */
export function useCacheManagementApi() {
  const apiPrefix = '/api/caching-management/cache';
  const { cancel, request } = useRequest();

  /**
   * 获取所有缓存键
   * @param input 参数
   */
  function getKeysApi(input?: GetCacheKeysInput): Promise<CacheKeysDto> {
    return request<CacheKeysDto>(`${apiPrefix}/keys`, {
      method: 'GET',
      params: input,
    });
  }

  /**
   * 获取缓存值
   * @param input 参数
   */
  function getKeyValueApi(input: CacheKeyInput): Promise<CacheValueDto> {
    return request<CacheValueDto>(`${apiPrefix}/value`, {
      method: 'GET',
      params: input,
    });
  }

  /**
   * 设置缓存值
   * @param input 参数
   */
  function setValueApi(input: CacheSetInput): Promise<void> {
    return request(`${apiPrefix}/set`, {
      method: 'PUT',
      data: input,
    });
  }

  /**
   * 刷新缓存
   * @param input 参数
   */
  function refreshApi(input: CacheRefreshInput): Promise<void> {
    return request(`${apiPrefix}/refresh`, {
      method: 'PUT',
      data: input,
    });
  }

  /**
   * 移除缓存
   * @param input 参数
   */
  function removeApi(input: CacheKeyInput): Promise<void> {
    return request(`${apiPrefix}/remove`, {
      method: 'DELETE',
      params: input,
    });
  }

  /**
   * 批量移除缓存
   * @param input 参数
   */
  function bulkRemoveApi(input: CacheRemoveKeysInput): Promise<void> {
    return request(`${apiPrefix}/bulk/remove`, {
      method: 'DELETE',
      data: input,
    });
  }

  return {
    cancel,
    getKeysApi,
    getKeyValueApi,
    refreshApi,
    setValueApi,
    removeApi,
    bulkRemoveApi,
  };
}
