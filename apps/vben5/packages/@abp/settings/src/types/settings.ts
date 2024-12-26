interface SettingBase {
  /** 名称 */
  name: string;
  /** 当前设置值 */
  value: string;
}

/** 配置变更对象 */
type SettingUpdateInput = SettingBase;

/** 配置变更集合对象 */
interface SettingsUpdateInput {
  /** 配置集合 */
  settings: SettingUpdateInput[];
}

export enum ValueType {
  Array = 4,
  Boolean = 2,
  Date = 3,
  Number = 1,
  Object = 10,
  Option = 5,
  String = 0,
}

interface Option {
  name: string;
  value: string;
}

interface SettingDetail {
  defaultValue: string;
  description?: string;
  displayName: string;
  isEncrypted: boolean;
  name: string;
  options: Option[];
  slot?: string;
  value?: string;
  valueType: ValueType;
}

interface Setting {
  description?: string;
  details: SettingDetail[];
  displayName: string;
}

interface SettingGroup {
  description: string;
  displayName: string;
  settings: Setting[];
}

export type { Setting, SettingDetail, SettingGroup, SettingsUpdateInput };
