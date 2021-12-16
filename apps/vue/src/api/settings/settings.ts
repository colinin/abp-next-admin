import { defHttp } from '/@/utils/http/axios';
import { SettingGroupResult, SettingsUpdate } from './model/settingModel';

enum Api {
  GetGlobalSettings = '/api/setting-management/settings/by-global',
  SetGlobalSettings = '/api/setting-management/settings/change-global',
  GetCurrentTenantSettings = '/api/setting-management/settings/by-current-tenant',
  SetCurrentTenantSettings = '/api/setting-management/settings/change-current-tenant',
  GetCurrentUserSettings = '/api/setting-management/settings/by-current-user',
  SetCurrentUserSettings = '/api/setting-management/settings/change-current-user',
}

export const getGlobalSettings = () => {
  return defHttp.get<SettingGroupResult>({
    url: Api.GetGlobalSettings,
  });
};

export const setGlobalSettings = (payload: SettingsUpdate) => {
  return defHttp.put({
    data: payload,
    url: Api.SetGlobalSettings,
  });
};

export const getCurrentTenantSettings = () => {
  return defHttp.get<SettingGroupResult>({
    url: Api.GetCurrentTenantSettings,
  });
};

export const setCurrentTenantSettings = (payload: SettingsUpdate) => {
  return defHttp.put({
    data: payload,
    url: Api.SetCurrentTenantSettings,
  });
};

export const getCurrentUserSettings = () => {
  return defHttp.get<SettingGroupResult>({
    url: Api.GetCurrentUserSettings,
  });
};

export const setCurrentUserSettings = (payload: SettingsUpdate) => {
  return defHttp.put({
    data: payload,
    url: Api.SetCurrentUserSettings,
  });
};
