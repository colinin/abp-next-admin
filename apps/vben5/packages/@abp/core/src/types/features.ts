import type { NameValue } from './global';

type FeatureValue = NameValue<string>;

/**
 * 特性检查接口
 */
interface IFeatureChecker {
  /**
   * 获取特性值
   * @param name 特性名称
   * @returns 返回设定的特性值,不存在则为空字符串
   */
  getOrEmpty(name: string): string;
  /**
   * 是否启用特性
   * @param featureNames 特性名称
   * @param requiresAll 是否全部符合
   */
  isEnabled(featureNames: string | string[], requiresAll?: boolean): boolean;
}

interface IGlobalFeatureChecker {
  isEnabled(featureNames: string | string[], requiresAll?: boolean): boolean;
}

export type { FeatureValue, IFeatureChecker, IGlobalFeatureChecker };
