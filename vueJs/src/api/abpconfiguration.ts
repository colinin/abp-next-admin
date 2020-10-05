import ApiService from './serviceBase'
import { INameValue, Available } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class AbpConfigurationService {
  public static getAbpConfiguration() {
    const _url = '/api/abp/application-configuration'
    return ApiService.HttpRequest<AbpConfiguration>({
      url: _url,
      baseURL: serviceUrl,
      method: 'GET',
      timeout: 120000
    })
  }
}

/** 授权 */
export class Auth {
  /** 权限集合 */
  policies?: { [key: string]: boolean}
  /** 已授权集合 */
  grantedPolicies?: { [key: string]: boolean}
}

/** 当前租户 */
export class CurrentTenant {
  /** 标识 */
  id?: string
  /** 名称 */
  name?: string
  /** 是否可用 */
  isAvailable!: boolean

  public clear() {
    this.id = ''
    this.name = ''
    this.isAvailable = false
  }
}

/** 当前用户 */
export class CurrentUser {
  /** 标识 */
  id?: string
  /** 邮件地址 */
  email?: string
  /** 邮件已验证 */
  emailVerified!: boolean
  /** 手机号 */
  phoneNumber?: string
  /** 手机号已验证 */
  phoneNumberVerified!: boolean
  /** 名称 */
  name?: string
  /** 简称 */
  surName?: string
  /** 用户名 */
  userName?: string
  /** 所属租户 */
  tenantId?: string
  /** 是否已认证 */
  isAuthenticated!: boolean
  /** 所属角色列表 */
  roles!: string[]
}

/** 功能 */
export class Feature {
  /** 功能集合 */
  values?: { [key: string]: string}
}

/** 时区转换 */
export class DateTimeFormat {
  /** 日历算法 */
  calendarAlgorithmType!: string
  /** 日期分隔符 */
  dateSeparator!: string
  /** 日期时间格式 */
  dateTimeFormatLong!: string
  /** 完整日期时间格式 */
  fullDateTimePattern!: string
  /** 长时间格式 */
  longTimePattern!: string
  /** 短日期格式 */
  shortDatePattern!: string
  /** 短时间格式 */
  shortTimePattern!: string
}

/** 当前区域信息 */
export class CurrentCulture {
  /** 本地化名称 */
  cultureName!: string
  /** 显示名称 */
  displayName!: string
  /** 英文名称 */
  englishName!: string
  /** 是否从右到左 */
  isRightToLeft!: boolean
  /** 名称 */
  name!: string
  /** 本地名称 */
  nativeName!: string
  /** 三个字母的ISO名称 */
  threeLetterIsoLanguageName!: string
  /** 两个字母的ISO名称 */
  twoLetterIsoLanguageName!: string
  /** 日期时间格式 */
  dateTimeFormat!: DateTimeFormat
}

/** 语言 */
export class Language {
  /** 本地化名称 */
  cultureName!: string
  /** 显示名称 */
  displayName!: string
  /** 图标 */
  flagIcon?: string
  /** 用户界面本地化名称 */
  uiCultureName!: string
}

/** 本地化 */
export class Localization {
  /** 当前区域 */
  currentCulture!: CurrentCulture
  /** 默认本地化资源名称 */
  defaultResourceName?: string
  /** 支持的语言列表 */
  languages!: Language[]
  /** 本地化资源集合 */
  values!: {[key: string]: {[key: string]: string}}
  /** 语言映射集合 */
  languagesMap?: {[key: string]: INameValue<string>[]}
  /** 语言文档映射集合 */
  languageFilesMap?: {[key: string]: INameValue<string>[]}
}

/** 多租户配置 */
export class MultiTenancy {
  /** 是否启用多租户 */
  isEnabled = false
}

/** 全局设置 */
export class Setting {
  /** 设置集合 */
  values?: {[key: string]: any}
}

/** 实体查询属性扩展 */
export class ExtensionPropertyApiGet extends Available {
}

/** 实体创建属性扩展 */
export class ExtensionPropertyApiCreate extends Available {
}

/** 实体更新属性扩展 */
export class ExtensionPropertyApiUpdate extends Available {
}

/** 实体属性api定义 */
export class ExtensionPropertyApi {
  /** 查询时 */
  onGet!: ExtensionPropertyApiGet
  /** 创建时 */
  onCreate!: ExtensionPropertyApiCreate
  /** 更新时 */
  onUpdate!: ExtensionPropertyApiUpdate
}

export class ExtensionPropertyUiTable extends Available {
}

export class ExtensionPropertyUiForm extends Available {
}

export class ExtensionPropertyUi {
  onTable!: ExtensionPropertyUiTable
  onCreateForm!: ExtensionPropertyUiForm
  onEditForm!: ExtensionPropertyUiForm
}

export class LocalizableString {
  name!: string
  resource?: string
}

export class ExtensionPropertyAttribute {
  typeSimple?: string
  config?: {[key: string]: any}
}

export class ExtensionProperty {
  type!: string
  typeSimple!: string
  displayName?: LocalizableString
  api!: ExtensionPropertyApi
  ui!: ExtensionPropertyUi
  attributes!: ExtensionPropertyAttribute[]
  configuration!: {[key: string]: any}
  defaultValue!: any
}

export class EntityExtension {
  properties!: {[key: string]: ExtensionProperty}
  configuration!: {[key: string]: any}
}

export class ModuleExtension {
  entities!: {[key: string]: EntityExtension}
  configuration!: {[key: string]: any}
}

export class ExtensionEnumField {
  name!: string
  value!: any
}

export class ExtensionEnum {
  fields!: ExtensionEnumField[]
  localizationResource!: string
}

export class ObjectExtension {
  modules!: {[key: string]: ModuleExtension}
  enums!: {[key: string]: ExtensionEnum}
}

/** abp框架信息 */
export interface IAbpConfiguration {
  /** 授权 */
  auth: Auth
  /** 租户 */
  currentTenant: CurrentTenant
  /** 用户 */
  currentUser: CurrentUser
  /** 功能 */
  features: Feature
  /** 本地化 */
  localization: Localization
  /** 租户配置 */
  multiTenancy: MultiTenancy
  /** 对象扩展 */
  objectExtensions: ObjectExtension
  /** 设置 */
  setting: Setting
  /** 获取设置 */
  getSetting(key: string): string | undefined
}

export class AbpConfiguration implements IAbpConfiguration {
  auth!: Auth
  currentTenant!: CurrentTenant
  currentUser!: CurrentUser
  features!: Feature
  localization!: Localization
  multiTenancy!: MultiTenancy
  objectExtensions!: ObjectExtension
  setting!: Setting

  constructor() {
    this.auth = new Auth()
    this.setting = new Setting()
    this.features = new Feature()
    this.currentUser = new CurrentUser()
    this.localization = new Localization()
    this.multiTenancy = new MultiTenancy()
    this.currentTenant = new CurrentTenant()
    this.objectExtensions = new ObjectExtension()
  }

  public getSetting(key: string) {
    if (this.setting.values && this.setting.values[key]) {
      return this.setting.values[key]
    }
    return undefined
  }
}
