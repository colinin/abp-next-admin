import type { ApplicationLocalizationDto } from "#/abp-core";

import requestClient from "../../request";

/**
 * 获取应用程序语言
 * @returns 本地化配置
 */
export function getLocalizationApi(options: {
	cultureName: string;
	onlyDynamics?: boolean;
}): Promise<ApplicationLocalizationDto> {
	return requestClient.get<ApplicationLocalizationDto>("/api/abp/application-localization", {
		params: options,
	});
}
