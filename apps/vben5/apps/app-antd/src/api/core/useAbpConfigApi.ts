import type {
  ApplicationConfigurationDto,
  ApplicationLocalizationDto,
} from '@abp/core';

import { useRequest } from '@abp/request';

export function useAbpConfigApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取应用程序配置信息
   */
  function getConfigApi(options?: {
    includeLocalizationResources?: boolean;
  }): Promise<ApplicationConfigurationDto> {
    return request<ApplicationConfigurationDto>(
      '/api/abp/application-configuration',
      {
        params: options,
        method: 'GET',
      },
    );
  }

  /**
   * 获取应用程序语言
   * @returns 本地化配置
   */
  function getLocalizationApi(options: {
    cultureName: string;
    onlyDynamics?: boolean;
  }): Promise<ApplicationLocalizationDto> {
    return request<ApplicationLocalizationDto>(
      '/api/abp/application-localization',
      {
        params: options,
        method: 'GET',
      },
    );
  }

  return {
    cancel,
    getConfigApi,
    getLocalizationApi,
  };
}
