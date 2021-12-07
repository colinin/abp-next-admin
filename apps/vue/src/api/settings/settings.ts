import { defHttp } from '/@/utils/http/axios';
import { SettingGroup, SettingsUpdate } from './model/settingModel';
import { ListResultDto } from '/@/api/model/baseModel';

enum Api {
  GetGlobalSettings = '/api/setting-management/settings/by-global',
  SetGlobalSettings = '/api/setting-management/settings/change-global',
  GetCurrentTenantSettings = '/api/setting-management/settings/by-current-tenant',
  SetCurrentTenantSettings = '/api/setting-management/settings/change-current-tenant',
}

export const getGlobalSettings = () => {
  return defHttp.get<ListResultDto<SettingGroup>>({
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
  return defHttp.get<ListResultDto<SettingGroup>>({
    url: Api.GetCurrentTenantSettings,
  });
};

export const setCurrentTenantSettings = (payload: SettingsUpdate) => {
  return defHttp.put({
    data: payload,
    url: Api.SetCurrentTenantSettings,
  });
};
