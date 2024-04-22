import { computed } from 'vue';
import { useAbpStoreWithOut } from '/@/store/modules/abp';
import { isNullOrWhiteSpace } from '/@/utils/strings';

export interface IGlobalFeatureChecker {
  isEnabled(featureNames: string | string[], requiresAll?: boolean): boolean;
}

export function useGlobalFeatures() {
  const getGlobalFeatures = computed(() => {
    const abpStore = useAbpStoreWithOut();
    const enabledFeatures = abpStore.getApplication.globalFeatures.enabledFeatures ?? [];
    return enabledFeatures;
  });

  function get(name: string): string | undefined {
    return getGlobalFeatures.value.find((feature) => name === feature);
  }

  function _isEnabled(name: string): boolean {
    var feature = get(name);
    return !isNullOrWhiteSpace(feature);
  }

  function isEnabled(featureNames: string | string[], requiresAll?: boolean): boolean {
    if (Array.isArray(featureNames)) {
      if (featureNames.length === 0) return true;
      if (requiresAll === undefined || requiresAll === true) {
        for (let index = 0; index < featureNames.length; index++) {
          if (!_isEnabled(featureNames[index])) return false;
        }
        return true;
      }

      for (let index = 0; index < featureNames.length; index++) {
        if (_isEnabled(featureNames[index])) return true;
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