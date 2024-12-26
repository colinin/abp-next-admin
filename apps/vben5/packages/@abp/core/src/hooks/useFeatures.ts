import type { FeatureValue, IFeatureChecker } from '../types/features';

import { computed } from 'vue';

import { useAbpStore } from '../store/abp';

export function useFeatures() {
  const abpStore = useAbpStore();
  const getFeatures = computed(() => {
    const fetures = abpStore.application?.features.values ?? {};
    const fetureValues = Object.keys(fetures).map((key): FeatureValue => {
      return {
        name: key,
        value: fetures[key] ?? '',
      };
    });
    return fetureValues;
  });

  function get(name: string): FeatureValue | undefined {
    return getFeatures.value.find((feature) => name === feature.name);
  }

  function _isEnabled(name: string): boolean {
    const setting = get(name);
    return setting?.value.toLowerCase() === 'true';
  }

  const featureChecker: IFeatureChecker = {
    getOrEmpty(name: string) {
      return get(name)?.value ?? '';
    },

    isEnabled(featureNames: string | string[], requiresAll?: boolean) {
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
    },
  };

  return { featureChecker };
}
