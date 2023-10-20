export class SettingBase {
  /** 名称 */
  name!: string;
  /** 当前设置值 */
  value!: any;
}

/** 配置变更对象 */
export class SettingUpdate extends SettingBase {}

/** 配置变更集合对象 */
export class SettingsUpdate {
  /** 配置集合 */
  settings: SettingUpdate[];

  constructor() {
    this.settings = new Array<SettingUpdate>();
  }
}

export enum ValueType {
  String = 0,
  Number = 1,
  Boolean = 2,
  Date = 3,
  Array = 4,
  Option = 5,
  Object = 10,
}

export class Option {
  name!: string;
  value!: string;
}

export class SettingDetail {
  name!: string;
  displayName!: string;
  description?: string;
  value?: string;
  defaultValue!: string;
  valueType!: ValueType;
  isEncrypted = false;
  slot?: string;
  options = new Array<Option>();
}

export class Setting {
  displayName!: string;
  description?: string;
  details = new Array<SettingDetail>();
}

export class SettingGroup {
  displayName!: string;
  description?: string;
  settings = new Array<Setting>();
}

export interface SettingGroupResult {
  items: SettingGroup[];
}
