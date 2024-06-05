import { computed, onMounted } from 'vue';
import { SettingGroup } from '/@/api/settings-management/settings/model';
import { useSettingManagementStoreWithOut } from '/@/store/modules/settings';
import { useSettings as useAbpSettings, ISettingProvider } from '/@/hooks/abp/useSettings';

type SettingValue = NameValue<string>;

export function useDefineSettings(
  settingKey: string, 
  api: (...args) => Promise<ListResultDto<SettingGroup>>,
  onReady?: () => void) {
  const settingStore = useSettingManagementStoreWithOut();
  const { settingProvider: abpSettingProvider } = useAbpSettings();
  const getSettings = computed(() => {
    const abpSettings = abpSettingProvider.getAll();
    return [...abpSettings, ...settingStore.getSettings];
  });

  onMounted(() => {
    settingStore.initlize(settingKey, api, onReady);
  });

  function get(name: string): SettingValue | undefined {
    return getSettings.value.find((setting) => name === setting.name);
  }

  function getAll(...names: string[]): SettingValue[] {
    if (names.length !== 0) {
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

  function refresh() {
    settingStore.refreshSettings(api);
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

  return {
    ...settingProvider,
    refresh,
  };
}