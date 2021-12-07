import { computed } from 'vue';
import { useAbpStoreWithOut } from '/@/store/modules/abp';

type FeatureValue = NameValue<string>;

/**
 * 特性检查接口
 */
interface IFeatureChecker {
  /**
   * 是否启用特性
   * @param name 特性名称
   */
  isEnabled(name: string): boolean;
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

  const featureChecker: IFeatureChecker = {
    getOrEmpty(name: string) {
      return get(name)?.value ?? '';
    },
    isEnabled(name: string) {
      var setting = get(name);
      return setting?.value.toLowerCase() === 'true';
    },
  };

  return { featureChecker };
}
