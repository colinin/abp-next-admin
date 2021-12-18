import { computed } from 'vue';
import { merge } from 'lodash-es';
import { useAbpStoreWithOut } from '/@/store/modules/abp';
import { format } from '/@/utils/strings';

interface IStringLocalizer {
  L(key: string, ...args: any[]): string;
}

export function useLocalization(resourceName: string, ...mergeResources: string[]) {
  const getResource = computed(() => {
    const abpStore = useAbpStoreWithOut();
    const { values } = abpStore.getApplication.localization;
    let resource: { [key: string]: string } = values[resourceName];
    mergeResources.forEach((name) => {
      resource = merge(resource, values[name]);
    });
    return resource;
  });

  function L(key: string, ...args: any[]) {
    if (!key) return '';
    if (!getResource.value) return key;
    if (!Reflect.has(getResource.value, key)) return key;
    return format(getResource.value[key], args ?? []);
  }

  const localizer: IStringLocalizer = {
    L: L,
  };

  return { L, localizer };
}
