import { computed } from 'vue';

import { useAbpStore } from '../store/abp';
import { isNullOrWhiteSpace } from '../utils/string';

export function useGlobalFeatures() {
  const getGlobalFeatures = computed(() => {
    const abpStore = useAbpStore();
    const enabledFeatures =
      abpStore.application?.globalFeatures.enabledFeatures ?? [];
    return enabledFeatures;
  });

  function get(name: string): string | undefined {
    return getGlobalFeatures.value.find((feature) => name === feature);
  }

  function _isEnabled(name: string): boolean {
    const feature = get(name);
    return !isNullOrWhiteSpace(feature);
  }

  function isEnabled(
    featureNames: string | string[],
    requiresAll?: boolean,
  ): boolean {
    if (Array.isArray(featureNames)) {
      if (featureNames.length === 0) return true;
      if (requiresAll === undefined || requiresAll === true) {
        for (const featureName of featureNames) {
          if (!_isEnabled(featureName)) return false;
        }
        return true;
      }

      for (const featureName of featureNames) {
        if (_isEnabled(featureName)) return true;
      }
    } else {
      return _isEnabled(featureNames);
    }

    return false;
  }

  return {
    isEnabled,
  };
}
