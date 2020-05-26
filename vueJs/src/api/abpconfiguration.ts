import ApiService from './serviceBase'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class AbpConfigurationService {
  public static getAbpConfiguration() {
    const _url = '/api/abp/application-configuration'
    return ApiService.Get<AbpConfiguration>(_url, serviceUrl)
  }
}

export class Auth {
  policies?: { [key: string]: boolean}
  grantedPolicies?: { [key: string]: boolean}
}

export class CurrentTenant {
  id?: string
  name?: string
  isAvailable!: boolean
}

export class CurrentUser {
  id?: string
  email?: string
  userName?: string
  tenantId?: string
  isAuthenticated!: boolean
}

export class Feature {
  values?: { [key: string]: string}
}

export class DateTimeFormat {
  calendarAlgorithmType!: string
  dateSeparator!: string
  dateTimeFormatLong!: string
  fullDateTimePattern!: string
  longTimePattern!: string
  shortDatePattern!: string
  shortTimePattern!: string
}

export class CurrentCulture {
  cultureName!: string
  displayName!: string
  englishName!: string
  isRightToLeft!: boolean
  name!: string
  nativeName!: string
  threeLetterIsoLanguageName!: string
  twoLetterIsoLanguageName!: string
  dateTimeFormat!: DateTimeFormat
}

export class Language {
  cultureName!: string
  displayName!: string
  flagIcon?: string
  uiCultureName!: string
}

export class Localization {
  currentCulture!: CurrentCulture
  defaultResourceName?: string
  languages!: Language[]
  values!: {[key:string]: {[key:string]: string}}
}

export class MultiTenancy {
  isEnabled!: boolean
}

export class Setting {
  values?: {[key:string]: string}
}

export interface IAbpConfiguration {
  auth: Auth
  currentTenant: CurrentTenant
  currentUser: CurrentUser
  features: Feature
  localization: Localization
  multiTenancy: MultiTenancy
  objectExtensions: any
  setting: Setting
}

export class AbpConfiguration implements IAbpConfiguration {
  auth!: Auth
  currentTenant!: CurrentTenant
  currentUser!: CurrentUser
  features!: Feature
  localization!: Localization
  multiTenancy!: MultiTenancy
  objectExtensions!: any
  setting!: Setting

  constructor() {
    this.auth = new Auth()
    this.setting = new Setting()
    this.features = new Feature()
    this.currentUser = new CurrentUser()
    this.localization = new Localization()
    this.multiTenancy = new MultiTenancy()
    this.currentTenant = new CurrentTenant()
  }
}