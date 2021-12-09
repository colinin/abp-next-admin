import { INameValue, Available } from '../../model/baseModel';

/** 授权 */
export class Auth {
  /** 权限集合 */
  policies?: { [key: string]: boolean } = {};
  /** 已授权集合 */
  grantedPolicies?: { [key: string]: boolean } = {};
}

/** 当前租户 */
export class CurrentTenant {
  /** 标识 */
  id? = '';
  /** 名称 */
  name? = '';
  /** 是否可用 */
  isAvailable = false;

  public clear() {
    this.id = '';
    this.name = '';
    this.isAvailable = false;
  }
}

/** 当前用户 */
export class CurrentUser {
  /** 标识 */
  id? = '';
  /** 邮件地址 */
  email? = '';
  /** 邮件已验证 */
  emailVerified = false;
  /** 手机号 */
  phoneNumber? = '';
  /** 手机号已验证 */
  phoneNumberVerified = false;
  /** 名称 */
  name? = '';
  /** 简称 */
  surName? = '';
  /** 用户名 */
  userName? = '';
  /** 所属租户 */
  tenantId? = '';
  /** 是否已认证 */
  isAuthenticated = false;
  /** 所属角色列表 */
  roles = new Array<string>();
}

/** 功能 */
export class Feature {
  /** 功能集合 */
  values?: { [key: string]: string } = {};
}

/** 时区转换 */
export class DateTimeFormat {
  /** 日历算法 */
  calendarAlgorithmType = '';
  /** 日期分隔符 */
  dateSeparator = '';
  /** 日期时间格式 */
  dateTimeFormatLong = '';
  /** 完整日期时间格式 */
  fullDateTimePattern = '';
  /** 长时间格式 */
  longTimePattern = '';
  /** 短日期格式 */
  shortDatePattern = '';
  /** 短时间格式 */
  shortTimePattern = '';
}

/** 当前区域信息 */
export class CurrentCulture {
  /** 本地化名称 */
  cultureName = '';
  /** 显示名称 */
  displayName = '';
  /** 英文名称 */
  englishName = '';
  /** 是否从右到左 */
  isRightToLeft = false;
  /** 名称 */
  name = '';
  /** 本地名称 */
  nativeName = '';
  /** 三个字母的ISO名称 */
  threeLetterIsoLanguageName = '';
  /** 两个字母的ISO名称 */
  twoLetterIsoLanguageName = '';
  /** 日期时间格式 */
  dateTimeFormat = new DateTimeFormat();
}

/** 语言 */
export class Language {
  /** 本地化名称 */
  cultureName = '';
  /** 显示名称 */
  displayName = '';
  /** 图标 */
  flagIcon = '';
  /** 用户界面本地化名称 */
  uiCultureName = '';
}

/** 本地化 */
export class Localization {
  /** 当前区域 */
  currentCulture = new CurrentCulture();
  /** 默认本地化资源名称 */
  defaultResourceName = '';
  /** 支持的语言列表 */
  languages = new Array<Language>();
  /** 本地化资源集合 */
  values: { [key: string]: { [key: string]: string } } = {};
  /** 语言映射集合 */
  languagesMap: { [key: string]: INameValue<string>[] } = {};
  /** 语言文档映射集合 */
  languageFilesMap: { [key: string]: INameValue<string>[] } = {};
}

/** 多租户配置 */
export class MultiTenancy {
  /** 是否启用多租户 */
  isEnabled = false;
}

/** 全局设置 */
export class Setting {
  /** 设置集合 */
  values: { [key: string]: string } = {};
}

/** 实体查询属性扩展 */
export class ExtensionPropertyApiGet extends Available {}

/** 实体创建属性扩展 */
export class ExtensionPropertyApiCreate extends Available {}

/** 实体更新属性扩展 */
export class ExtensionPropertyApiUpdate extends Available {}

/** 实体属性api定义 */
export class ExtensionPropertyApi {
  /** 查询时 */
  onGet = new ExtensionPropertyApiGet();
  /** 创建时 */
  onCreate = new ExtensionPropertyApiCreate();
  /** 更新时 */
  onUpdate = new ExtensionPropertyApiUpdate();
}

export class ExtensionPropertyUiTable extends Available {}

export class ExtensionPropertyUiForm extends Available {}

export class ExtensionPropertyUi {
  onTable = new ExtensionPropertyUiTable();
  onCreateForm = new ExtensionPropertyUiForm();
  onEditForm = new ExtensionPropertyUiForm();
}

export class LocalizableString {
  name = '';
  resource = '';
}

export class ExtensionPropertyAttribute {
  typeSimple = '';
  config: { [key: string]: any } = {};
}

export class ExtensionProperty {
  type = '';
  typeSimple = '';
  displayName = new LocalizableString();
  api = new ExtensionPropertyApi();
  ui = new ExtensionPropertyUi();
  attributes = new Array<ExtensionPropertyAttribute>();
  configuration: { [key: string]: any } = {};
  defaultValue: any = '';
}

export class EntityExtension {
  properties: { [key: string]: ExtensionProperty } = {};
  configuration: { [key: string]: any } = {};
}

export class ModuleExtension {
  entities: { [key: string]: EntityExtension } = {};
  configuration: { [key: string]: any } = {};
}

export class ExtensionEnumField {
  name = '';
  value: any = '';
}

export class ExtensionEnum {
  fields = new Array<ExtensionEnumField>();
  localizationResource = '';
}

export class ObjectExtension {
  modules: { [key: string]: ModuleExtension } = {};
  enums: { [key: string]: ExtensionEnum } = {};
}

/** abp框架信息 */
export interface IApplicationConfiguration {
  /** 授权 */
  auth: Auth;
  /** 租户 */
  currentTenant: CurrentTenant;
  /** 用户 */
  currentUser: CurrentUser;
  /** 功能 */
  features: Feature;
  /** 本地化 */
  localization: Localization;
  /** 租户配置 */
  multiTenancy: MultiTenancy;
  /** 对象扩展 */
  objectExtensions: ObjectExtension;
  /** 设置 */
  setting: Setting;
}

export class ApplicationConfiguration implements IApplicationConfiguration {
  auth = new Auth();
  currentTenant = new CurrentTenant();
  currentUser = new CurrentUser();
  features = new Feature();
  localization = new Localization();
  multiTenancy = new MultiTenancy();
  objectExtensions = new ObjectExtension();
  setting = new Setting();
}
