import type { TransferItem } from 'ant-design-vue/es/transfer';

import { ref } from 'vue';

import { useAbpStore } from '@abp/core';

import { useScopesApi } from '../api/useScopesApi';

export function useScopeTransfer() {
  const availableResources = ref<TransferItem[]>([]);
  const abpStore = useAbpStore();
  const { getAssignableScopesApi } = useScopesApi();
  async function initAssignableScopes() {
    const currentCulture =
      abpStore.application?.localization.currentCulture.cultureName;
    const { items } = await getAssignableScopesApi();
    availableResources.value = items.map((resource) => {
      let displayName = resource.displayName;
      let description = resource.description;
      if (resource.displayNames && currentCulture) {
        const locales = Object.keys(resource.displayNames);
        const locale = locales.find((x) => x.includes(currentCulture));
        if (locale) {
          displayName = resource.displayNames[locale];
        }
      }
      if (resource.descriptions && currentCulture) {
        const locales = Object.keys(resource.descriptions);
        const locale = locales.find((x) => x.includes(currentCulture));
        if (locale) {
          description = resource.descriptions[locale];
        }
      }
      return {
        key: resource.name,
        title: displayName,
        description,
      };
    });
  }

  return {
    initAssignableScopes,
    availableResources,
  };
}
