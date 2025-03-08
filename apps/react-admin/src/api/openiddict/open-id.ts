import type { OpenIdConfiguration } from "#/abp-core/openid";

import requestClient from "../request";

/**
 * openid发现端点
 * @returns OpenId配置数据
 */
export function discoveryApi(): Promise<OpenIdConfiguration> {
	return requestClient.get<OpenIdConfiguration>("/.well-known/openid-configuration");
}
