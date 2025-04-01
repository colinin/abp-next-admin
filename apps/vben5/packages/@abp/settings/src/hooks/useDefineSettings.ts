import type { ISettingProvider, SettingValue } from '@abp/core';

import type { SettingGroup } from '../types/settings';

import { ref } from 'vue';

interface UserDefineSettingProvider extends ISettingProvider {
  /**
   * 初始化配置项
   * @param groups 配置分组
   */
  initlize(groups: SettingGroup[]): void;
}

/**
 * 自定义设置管理Hooks
 * @returns 自定义设置管理器
 * @summary 请事先获取配置分组后通过 initlize 函数设置初始化设置值
 */
export function useDefineSettings(): UserDefineSettingProvider {
  const settings = ref<SettingValue[]>([]);

  function initlize(groups: SettingGroup[]) {
    const setSettings: SettingValue[] = [];
    groups.forEach((group) => {
      group.settings.forEach((setting) => {
        setting.details.forEach((detail) => {
          setSettings.push({
            name: detail.name,
            value: detail.value ?? detail.defaultValue,
          });
        });
      });
    });
    settings.value = setSettings;
  }

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

  return {
    getAll,
    getNumber(name: string, defaultValue: number = 0) {
      const value = getOrDefault(name, defaultValue);
      const num = Number(value);
      return Number.isNaN(num) ? defaultValue : num;
    },
    getOrEmpty(name: string) {
      return getOrDefault(name, '');
    },
    initlize,
    isTrue(name: string) {
      const value = getOrDefault(name, 'false');
      return value.toLowerCase() === 'true';
    },
  };
}
