import type { ApplicationLocalizationDto } from '@abp/core';

import { useRequest } from '@abp/request';

export function useLocalizationsApi() {
  const { cancel, request } = useRequest();
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
        method: 'GET',
        params: options,
      },
    );
  }

  return {
    cancel,
    getLocalizationApi,
  };
}
