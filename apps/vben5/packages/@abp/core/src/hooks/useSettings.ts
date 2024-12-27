import type { ISettingProvider, SettingValue } from '../types/settings';

import { computed } from 'vue';

import { useAbpStore } from '../store';

export function useSettings(): ISettingProvider {
  const abpStore = useAbpStore();
  const getSettings = computed(() => {
    if (!abpStore.application) {
      return [];
    }
    const { values: settings } = abpStore.application.setting;
    const settingValues = Object.keys(settings).map((key): SettingValue => {
      return {
        name: key,
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        value: settings[key]!,
      };
    });
    return settingValues;
  });

  function get(name: string): SettingValue | undefined {
    return getSettings.value.find((setting) => name === setting.name);
  }

  function getAll(...names: string[]): SettingValue[] {
    if (names) {
      return getSettings.value.filter((setting) =>
        names.includes(setting.name),
      );
    }
    return getSettings.value;
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
