import type { FeatureValue, IFeatureChecker } from '../types/features';

import { ref, watch } from 'vue';

import { useAbpStore } from '../store/abp';

export function useFeatures(): IFeatureChecker {
  const abpStore = useAbpStore();

  const features = ref<FeatureValue[]>([]);

  watch(
    () => abpStore.application,
    (application) => {
      if (!application?.features.values) {
        features.value = [];
        return;
      }
      const featuresSet: FeatureValue[] = [];
      Object.keys(application.features.values).forEach((name) => {
        if (application.features.values[name]) {
          featuresSet.push({
            name,
            value: application.features.values[name],
          });
        }
      });
      features.value = featuresSet;
    },
    {
      deep: true,
      immediate: true,
    },
  );

  function get(name: string): FeatureValue | undefined {
    return features.value.find((feature) => name === feature.name);
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

  return featureChecker;
}
