import type { ListResultDto } from '@abp/core';

import type { SettingGroup, SettingsUpdateInput } from '../types/settings';

import { requestClient } from '@abp/request';

/**
 * 获取全局设置
 * @returns 设置数据传输对象列表
 */
export function getGlobalSettingsApi(): Promise<ListResultDto<SettingGroup>> {
  return requestClient.get<ListResultDto<SettingGroup>>(
    `/api/setting-management/settings/by-global`,
  );
}

/**
 * 设置全局设置
 * @returns 设置数据传输对象列表
 */
export function setGlobalSettingsApi(
  input: SettingsUpdateInput,
): Promise<void> {
  return requestClient.put(
    `/api/setting-management/settings/change-global`,
    input,
  );
}

/**
 * 获取租户设置
 * @returns 设置数据传输对象列表
 */
export function getTenantSettingsApi(): Promise<ListResultDto<SettingGroup>> {
  return requestClient.get<ListResultDto<SettingGroup>>(
    `/api/setting-management/settings/by-current-tenant`,
  );
}

/**
 * 设置租户设置
 * @returns 设置数据传输对象列表
 */
export function setTenantSettingsApi(
  input: SettingsUpdateInput,
): Promise<void> {
  return requestClient.put(
    `/api/setting-management/settings/change-current-tenant`,
    input,
  );
}
/**
 * 获取用户设置
 * @returns 设置数据传输对象列表
 */
export function getUserSettingsApi(): Promise<ListResultDto<SettingGroup>> {
  return requestClient.get<ListResultDto<SettingGroup>>(
    `/api/setting-management/settings/by-current-user`,
  );
}

/**
 * 设置用户设置
 * @returns 设置数据传输对象列表
 */
export function setUserSettingsApi(input: SettingsUpdateInput): Promise<void> {
  return requestClient.put(
    `/api/setting-management/settings/change-current-user`,
    input,
  );
}

/**
 * 发送测试邮件
 * @param emailAddress 邮件接收方地址
 */
export const sendTestEmailApi = (emailAddress: string) => {
  return requestClient.post(
    `/api/setting-management/settings/send-test-email`,
    {
      emailAddress,
    },
  );
};
