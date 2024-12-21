import type { OpenIdConfiguration } from '@abp/core';

import { requestClient } from '@abp/request';

/**
 * openid发现端点
 * @returns OpenId配置数据
 */
export function discoveryApi(): Promise<OpenIdConfiguration> {
  return requestClient.get<OpenIdConfiguration>(
    '/.well-known/openid-configuration',
  );
}
