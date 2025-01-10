import type { AxiosRequestConfig } from 'axios';

import { onUnmounted } from 'vue';

import { requestClient } from '@abp/request';

type HttpMethod =
  | 'CONNECT'
  | 'DELETE'
  | 'GET'
  | 'HEAD'
  | 'OPTIONS'
  | 'PATCH'
  | 'POST'
  | 'PURGE'
  | 'PUT'
  | 'TRACE';

interface RequestConfig extends AxiosRequestConfig {
  method: HttpMethod;
}

interface RequestLifeCycle {
  /** 是否自动销毁令牌 */
  autoDestroy?: boolean;
}

export function useRequest(options?: RequestLifeCycle) {
  const controllers = new Set<AbortController>();

  function request<T>(url: string, config: RequestConfig): Promise<T> {
    const controller = new AbortController();
    controllers.add(controller);
    return requestClient
      .request<T>(url, {
        ...config,
        signal: controller.signal,
      })
      .finally(() => {
        controllers.delete(controller);
      });
  }

  function cancel(message?: string) {
    controllers.forEach((controller) => controller.abort(message));
    controllers.clear();
  }

  onUnmounted(() => {
    if (options?.autoDestroy === false) {
      return;
    }
    cancel('The Component has Unmounted!');
  });

  return {
    cancel,
    request,
  };
}
