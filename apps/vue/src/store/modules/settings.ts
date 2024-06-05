import { defineStore } from 'pinia';
import { store } from '/@/store';
import { createLocalStorage } from '/@/utils/cache';
import { SettingGroup } from '/@/api/settings-management/settings/model';

const ls = createLocalStorage();
const SETTING_ID = 'setting-management';
type SettingValue = NameValue<string>;

interface IState {
  settingKey: string;
  settings: SettingValue[];
}

export const useSettingManagementStore = defineStore({
  id: SETTING_ID,
  state: (): IState => ({
    settingKey: 'unknown',
    settings: [],
  }),
  getters: {
    getSettings(state) {
      return state.settings;
    },
  },
  actions: {
    initlize(settingKey: string, api: (...args) => Promise<ListResultDto<SettingGroup>>, onReady?: () => void) {
      this.settingKey = settingKey;
      this.settings = ls.get(this.settingKey);
      if (!this.settings || this.settings.length === 0) {
        this.refreshSettings(api, onReady);
      } else {
        onReady?.call(null);
      }
    },
    refreshSettings(api: (...args) => Promise<ListResultDto<SettingGroup>>, onReady?: () => void) {
      api().then((res) => {
        const settings: SettingValue[] = [];
        res.items.forEach((group) => {
          group.settings.forEach((setting) => {
            setting.details.forEach((detail) => {
              settings.push({
                name: detail.name,
                value: detail.value ?? detail.defaultValue,
              });
            });
          });
        });
        this.settings = settings;
        ls.set(this.settingKey, settings);
        onReady?.call(null);
      });
    },
  },
});

export function useSettingManagementStoreWithOut() {
  return useSettingManagementStore(store);
}
