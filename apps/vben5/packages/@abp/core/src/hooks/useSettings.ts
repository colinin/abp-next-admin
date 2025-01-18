import type { ISettingProvider, SettingValue } from '../types/settings';

import { ref, watch } from 'vue';

import { useAbpStore } from '../store';

export function useSettings(): ISettingProvider {
  const abpStore = useAbpStore();

  const settings = ref<SettingValue[]>([]);

  watch(
    () => abpStore.application,
    (application) => {
      if (!application?.setting.values) {
        settings.value = [];
        return;
      }
      const settingsSet: SettingValue[] = [];
      Object.keys(application.setting.values).forEach((name) => {
        if (application.setting.values[name]) {
          settingsSet.push({
            name,
            value: application.setting.values[name],
          });
        }
      });
      settings.value = settingsSet;
    },
    {
      deep: true,
      immediate: true,
    },
  );

  function get(name: string): SettingValue | undefined {
    return settings.value.find((setting) => name === setting.name);
  }

  function getAll(...names: string[]): SettingValue[] {
    if (names) {
      return settings.value.filter((setting) => names.includes(setting.name));
    }
    return settings.value;
  }

  function getOrDefault<T>(name: string, defaultValue: T): string | T {
    const setting = get(name);
    if (!setting) {
      return defaultValue;
    }
    return setting.value;
  }

  const settingProvider: ISettingProvider = {
    getAll(...names: string[]) {
      return getAll(...names);
    },
    getNumber(name: string, defaultValue: number = 0) {
      const value = getOrDefault(name, defaultValue);
      const num = Number(value);
      return Number.isNaN(num) ? defaultValue : num;
    },
    getOrEmpty(name: string) {
      return getOrDefault(name, '');
    },
    isTrue(name: string) {
      const value = getOrDefault(name, 'false');
      return value.toLowerCase() === 'true';
    },
  };

  return settingProvider;
}
