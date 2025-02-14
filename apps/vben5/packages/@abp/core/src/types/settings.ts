import type { NameValue } from './global';

type SettingValue = NameValue<string>;
/**
 * 设置接口
 */
interface ISettingProvider {
  /**
   * 获取设定值结合
   * @param names 过滤的设置名称
   */
  getAll(...names: string[]): SettingValue[];
  /**
   * 查询 number 类型设定值
   * @param name 设置名称
   * @returns 返回类型为 number 的设定值, 默认0
   */
  getNumber(name: string): number;
  /**
   * 获取设定值,如果为空返回空字符串
   * @param name 设置名称
   */
  getOrEmpty(name: string): string;
  /**
   * 查询 boolean 类型设定值
   * @param name 设置名称
   */
  isTrue(name: string): boolean;
}

export type { ISettingProvider, SettingValue };
