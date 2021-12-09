import { computed } from 'vue';
import { useAbpStoreWithOut } from '/@/store/modules/abp';

type SettingValue = NameValue<string>;
/**
 * 设置接口
 */
interface ISettingProvider {
  /**
   * 查询 number 类型设定值
   * @param name 设置名称
   * @returns 返回类型为 number 的设定值, 默认0
   */
  getNumber(name: string): number;
  /**
   * 查询 boolean 类型设定值
   * @param name 设置名称
   */
  isTrue(name: string): boolean;
  /**
   * 获取设定值,如果为空返回空字符串
   * @param name 设置名称
   */
  getOrEmpty(name: string): string;
  /**
   * 获取设定值结合
   * @param names 过滤的设置名称
   */
  getAll(...names: string[]): SettingValue[];
}

export function useSettings() {
  const getSettings = computed(() => {
    const abpStore = useAbpStoreWithOut();
    const { values: settings } = abpStore.getApplication.setting;
    const settingValues = Object.keys(settings).map((key): SettingValue => {
      return {
        name: key,
        value: settings[key],
      };
    });
    return settingValues;
  });

  function get(name: string): SettingValue | undefined {
    return getSettings.value.find((setting) => name === setting.name);
  }

  function getAll(...names: string[]): SettingValue[] {
    if (names) {
      return getSettings.value.filter((setting) => names.includes(setting.name));
    }
    return getSettings.value;
  }

  function getOrDefault<T>(name: string, defaultValue: T): T | string {
    var setting = get(name);
    if (!setting) {
      return defaultValue;
    }
    return setting.value;
  }

  const settingProvider: ISettingProvider = {
    getOrEmpty(name: string) {
      return getOrDefault(name, '');
    },
    getAll(...names: string[]) {
      return getAll(...names);
    },
    getNumber(name: string, defaultValue: number = 0) {
      var value = getOrDefault(name, defaultValue);
      const num = Number(value);
      return isNaN(num) ? defaultValue : num;
    },
    isTrue(name: string) {
      var value = getOrDefault(name, 'false');
      return value.toLowerCase() === 'true';
    },
  };

  return { settingProvider };
}
