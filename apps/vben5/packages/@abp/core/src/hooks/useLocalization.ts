import type { Dictionary, StringLocalizer } from '../types';

import { computed, ref, watch } from 'vue';

import merge from 'lodash.merge';

import { useAbpStore } from '../store/abp';
import { format } from '../utils/string';

export function useLocalization(resourceNames?: string | string[]) {
  const abpStore = useAbpStore();

  const localizations = ref<Dictionary<string, Dictionary<string, string>>>({});

  watch(
    () => abpStore.localization,
    (localization) => {
      if (!localization?.resources) {
        localizations.value = {};
        return;
      }
      const localizationResource: Dictionary<
        string,
        Dictionary<string, string>
      > = {};
      Object.keys(localization.resources).forEach((resourceName) => {
        if (localization.resources[resourceName]) {
          localizationResource[resourceName] =
            localization.resources[resourceName].texts;
        }
      });
      localizations.value = localizationResource;
    },
    {
      deep: true,
      immediate: true,
    },
  );

  const getResource = computed(() => {
    let resource: { [key: string]: string } = {};
    if (resourceNames) {
      if (Array.isArray(resourceNames)) {
        resourceNames.forEach((name) => {
          resource = merge(resource, localizations.value[name]);
        });
      } else {
        resource = merge(resource, localizations.value[resourceNames]);
      }
    } else {
      Object.keys(localizations.value).forEach((rs) => {
        resource = merge(resource, localizations.value[rs]);
      });
    }

    return resource;
  });
  const getResourceByName = computed(() => {
    return (resource: string): Dictionary<string, string> => {
      return localizations.value[resource] ?? {};
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
