import type { ListResultDto } from '@abp/core';

import type { SettingGroup, SettingsUpdateInput } from '../types';

import { useRequest } from '@abp/request';

export function useSettingsApi() {
  const { cancel, request } = useRequest();
  /**
   * 获取设置
   * @returns 设置数据传输对象列表
   */
  function getSettingsApi(): Promise<ListResultDto<SettingGroup>> {
    return request<ListResultDto<SettingGroup>>(
      `/api/v2/setting-management/settings`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 变更设置
   * @returns 设置数据传输对象列表
   */
  function setSettingsApi(input: SettingsUpdateInput): Promise<void> {
    return request(`/api/v2/setting-management/settings`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 获取全局设置
   * @returns 设置数据传输对象列表
   */
  function getGlobalSettingsApi(): Promise<ListResultDto<SettingGroup>> {
    return request<ListResultDto<SettingGroup>>(
      `/api/setting-management/settings/by-global`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 设置全局设置
   * @returns 设置数据传输对象列表
   */
  function setGlobalSettingsApi(input: SettingsUpdateInput): Promise<void> {
    return request(`/api/setting-management/settings/change-global`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 获取租户设置
   * @returns 设置数据传输对象列表
   */
  function getTenantSettingsApi(): Promise<ListResultDto<SettingGroup>> {
    return request<ListResultDto<SettingGroup>>(
      `/api/setting-management/settings/by-current-tenant`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 设置租户设置
   * @returns 设置数据传输对象列表
   */
  function setTenantSettingsApi(input: SettingsUpdateInput): Promise<void> {
    return request(`/api/setting-management/settings/change-current-tenant`, {
      data: input,
      method: 'PUT',
    });
  }
  /**
   * 获取用户设置
   * @returns 设置数据传输对象列表
   */
  function getUserSettingsApi(): Promise<ListResultDto<SettingGroup>> {
    return request<ListResultDto<SettingGroup>>(
      `/api/v2/setting-management/my-settings`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 设置用户设置
   * @returns 设置数据传输对象列表
   */
  function setUserSettingsApi(input: SettingsUpdateInput): Promise<void> {
    return request(`/api/v2/setting-management/my-settings`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 发送测试邮件
   * @param emailAddress 邮件接收方地址
   */
  const sendTestEmailApi = (emailAddress: string) => {
    return request(`/api/setting-management/settings/send-test-email`, {
      data: {
        emailAddress,
      },
      method: 'POST',
    });
  };

  return {
    cancel,
    getSettingsApi,
    setSettingsApi,
    getGlobalSettingsApi,
    getTenantSettingsApi,
    getUserSettingsApi,
    sendTestEmailApi,
    setGlobalSettingsApi,
    setTenantSettingsApi,
    setUserSettingsApi,
  };
}
