import type {
  ApplicationConfigurationDto,
  ApplicationLocalizationDto,
} from '@abp/core';

import { requestClient } from '@abp/request';

/**
 * 获取应用程序配置信息
 */
export function getConfigApi(options?: {
  includeLocalizationResources?: boolean;
}): Promise<ApplicationConfigurationDto> {
  return requestClient.get<ApplicationConfigurationDto>(
    '/api/abp/application-configuration',
    {
      params: options,
    },
  );
}

/**
 * 获取应用程序语言
 * @returns 本地化配置
 */
export function getLocalizationApi(options: {
  cultureName: string;
  onlyDynamics?: boolean;
}): Promise<ApplicationLocalizationDto> {
  return requestClient.get<ApplicationLocalizationDto>(
    '/api/abp/application-localization',
    {
      params: options,
    },
  );
}
