import type { Dictionary, StringLocalizer } from '../types';

import { computed } from 'vue';

import { merge } from 'lodash';

import { useAbpStore } from '../store/abp';
import { format } from '../utils/string';

export function useLocalization(resourceNames?: string | string[]) {
  const abpStore = useAbpStore();
  const getResource = computed(() => {
    if (!abpStore.application) {
      return {};
    }
    const { values } = abpStore.application.localization;

    let resource: { [key: string]: string } = {};
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
  const getResourceByName = computed(() => {
    return (resource: string): Dictionary<string, string> => {
      if (!abpStore.application) {
        return {};
      }
      const { values } = abpStore.application.localization;
      return values[resource] ?? {};
    };
  });

  function L(key: string, args?: any[] | Record<string, string> | undefined) {
    if (!key) return '';
    if (!getResource.value) return key;
    if (!getResource.value[key]) return key;
    return format(getResource.value[key], args ?? []);
  }

  function Lr(
    resource: string,
    key: string,
    args?: any[] | Record<string, string> | undefined,
  ) {
    if (!key) return '';
    const findResource = getResourceByName.value(resource);
    if (!findResource) return key;
    if (!findResource[key]) return key;
    return format(findResource[key], args ?? []);
  }

  const localizer: StringLocalizer = {
    L,
    Lr,
  };

  return { L, localizer, Lr };
}
