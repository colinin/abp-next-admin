import type { OpenIdConfiguration } from '@abp/core';

import { useRequest } from '@abp/request';

export function useOpenIdApi() {
  const { cancel, request } = useRequest();

  /**
   * openid发现端点
   * @returns OpenId配置数据
   */
  function discoveryApi(): Promise<OpenIdConfiguration> {
    return request<OpenIdConfiguration>('/.well-known/openid-configuration', {
      method: 'GET',
    });
  }

  return {
    cancel,
    discoveryApi,
  };
}
