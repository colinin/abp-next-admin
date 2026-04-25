import type { SseRequestOptions } from '@vben/request';

import { requestClient } from '../index';

export function useSseRequest() {
  const controllers = new Set<AbortController>();

  function requestSSE(
    url: string,
    data?: any,
    requestOptions?: SseRequestOptions,
  ) {
    const controller = new AbortController();
    controllers.add(controller);
    return requestClient
      .postSSE(url, data, {
        ...requestOptions,
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

  return {
    cancel,
    requestSSE,
  };
}
