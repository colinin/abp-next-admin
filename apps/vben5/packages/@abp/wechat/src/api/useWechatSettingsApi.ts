import type { ListResultDto } from '@abp/core';
import type { SettingGroup } from '@abp/settings';

import { useRequest } from '@abp/request';

export function useWechatSettingsApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取全局设置
   * @returns 设置数据传输对象列表
   */
  function getGlobalSettingsApi(): Promise<ListResultDto<SettingGroup>> {
    return request<ListResultDto<SettingGroup>>(
      `/api/wechat/setting-management/by-global`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 获取租户设置
   * @returns 设置数据传输对象列表
   */
  function getTenantSettingsApi(): Promise<ListResultDto<SettingGroup>> {
    return request<ListResultDto<SettingGroup>>(
      `/api/wechat/setting-management/by-current-tenant`,
      {
        method: 'GET',
      },
    );
  }

  return {
    cancel,
    getGlobalSettingsApi,
    getTenantSettingsApi,
  };
}
