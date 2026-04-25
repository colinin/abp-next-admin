import type { NameValue } from '@abp/core';

import { useRequest } from '@abp/request';

export function useTimeZoneSettingsApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询系统时区设置
   * @returns 系统时区
   */
  function getApi(): Promise<string> {
    return request<string>(`/api/setting-management/timezone`, {
      method: 'GET',
    });
  }

  /**
   * 查询当前用户时区设置
   * @returns 用户时区
   */
  function getMyTimezoneApi(): Promise<string> {
    return request<string>(`/api/setting-management/timezone/my-timezone`, {
      method: 'GET',
    });
  }

  /**
   * 查询时区列表
   * @returns 时区列表
   */
  function getTimezonesApi(): Promise<NameValue<string>[]> {
    return request<NameValue<string>[]>(
      `/api/setting-management/timezone/timezones`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 更新时区
   * @param timezone 时区名称
   */
  function updateApi(timezone: string): Promise<void> {
    return request(`/api/setting-management/timezone?timezone=${timezone}`, {
      method: 'POST',
    });
  }

  /**
   * 更新当前用户时区
   * @param timezone 时区名称
   */
  function updateMyTimezoneApi(timezone: string): Promise<void> {
    return request(
      `/api/setting-management/timezone/my-timezone?timezone=${timezone}`,
      {
        method: 'POST',
      },
    );
  }

  return {
    cancel,
    getApi,
    getMyTimezoneApi,
    getTimezonesApi,
    updateMyTimezoneApi,
    updateApi,
  };
}
