/**
 * 该文件可自行根据业务逻辑进行调整
 */
import type { RequestClientOptions } from '@vben/request';

import { useAppConfig } from '@vben/hooks';
import { RequestClient } from '@vben/request';

export * from './constants';
export * from './hooks';
export * from './types';

const { apiURL } = useAppConfig(import.meta.env, import.meta.env.PROD);

function createRequestClient(baseURL: string, options?: RequestClientOptions) {
  const client = new RequestClient({
    ...options,
    baseURL,
  });

  return client;
}

export const requestClient = createRequestClient(apiURL, {
  responseReturn: 'data',
});
