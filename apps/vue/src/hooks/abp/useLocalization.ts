import { computed } from 'vue';
import { merge } from 'lodash-es';
import { useAbpStoreWithOut } from '/@/store/modules/abp';
import { format } from '/@/utils/strings';

interface IStringLocalizer {
  L(key: string, args?: Recordable | any[] | undefined): string;
}

export function useLocalization(resourceNames?: string | string[]) {
  const getResource = computed(() => {
    const abpStore = useAbpStoreWithOut();
    const { values } = abpStore.getApplication.localization;

    let resource: { [key: string]: string }  = {};
    if (resourceNames) {
      if (Array.isArray(resourceNames)) {
        resourceNames.forEach((name) => {
          resource = merge(resource, values[name]);
        });
      } else {
        resource = merge(resource, values[resourceNames]);
      }
    } else {
      Object.keys(values).forEach((rs) => {
        resource = merge(resource, values[rs]);
      });
    }

    return resource;
  });

  function L(key: string, args?: Recordable | any[] | undefined) {
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
