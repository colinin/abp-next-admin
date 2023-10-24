import { computed } from 'vue';
import { useAbpStoreWithOut } from '/@/store/modules/abp';

type FeatureValue = NameValue<string>;

/**
 * 特性检查接口
 */
export interface IFeatureChecker {
  /**
   * 是否启用特性
   * @param featureNames 特性名称
   * @param requiresAll 是否全部符合
   */
  isEnabled(featureNames: string | string[], requiresAll?: boolean): boolean;
  /**
   * 获取特性值
   * @param name 特性名称
   * @returns 返回设定的特性值,不存在则为空字符串
   */
  getOrEmpty(name: string): string;
}

export function useFeatures() {
  const getFeatures = computed(() => {
    const abpStore = useAbpStoreWithOut();
    const fetures = abpStore.getApplication.features.values ?? {};
    const fetureValues = Object.keys(fetures).map((key): FeatureValue => {
      return {
        name: key,
        value: fetures[key],
      };
    });
    return fetureValues;
  });

  function get(name: string): FeatureValue | undefined {
    return getFeatures.value.find((feature) => name === feature.name);
  }

  function _isEnabled(name: string): boolean {
    var setting = get(name);
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
    },
  };

  return { featureChecker };
}
