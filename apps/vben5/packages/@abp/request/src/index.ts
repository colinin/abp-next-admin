/**
 * 该文件可自行根据业务逻辑进行调整
 */

import { useAppConfig } from '@vben/hooks';
import { RequestClient } from '@vben/request';

export * from './hooks';
export * from './types';

const { apiURL } = useAppConfig(import.meta.env, import.meta.env.PROD);

function createRequestClient(baseURL: string) {
  const client = new RequestClient({
    baseURL,
  });

  return client;
}

export const requestClient = createRequestClient(apiURL);
