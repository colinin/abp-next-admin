import ApiService from './serviceBase'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class FeatureManagementService {
  public static getFeatures(providerName: string, providerKey: string) {
    let _url = '/api/feature-management/features?'
    _url += 'providerName=' + providerName
    _url += '&providerKey=' + providerKey
    return ApiService.Get<FeatureGroups>(_url, serviceUrl)
  }

  public static updateFeatures(providerName: string, providerKey: string, features: Features) {
    let _url = '/api/feature-management/features?'
    _url += 'providerName=' + providerName
    _url += '&providerKey=' + providerKey
    return ApiService.Put<void>(_url, features, serviceUrl)
  }
}

export class Provider {
  Name!: string
  Key!: string
}

export class ValueType {
  name!: string
  properties!: any
  validator!: any
}

export class Feature {
  name!: string
  displayName?: string
  value!: any
  description?: string
  valueType?: ValueType
  depth?: number
  parentName?: string
  provider!: Provider

  constructor(
    name: string,
    value: any
  ) {
    this.name = name
    this.value = value
  }
}

export class Features {
  features!: Feature[]

  constructor() {
    this.features = new Array<Feature>()
  }
}

export class FeatureGroup {
  name!: string
  displayName?: string
  features = new Array<Feature>()
}

export class FeatureGroups {
  groups = new Array<FeatureGroup>()
}

/**
 * 适用于动态表单的功能节点
 */
export class FeatureItem {
  /** 功能名称 */
  name!: string
  /** 显示名称 */
  displayName?: string
  /** 当前值 */
  value!: any
  /** 说明 */
  description?: string
  /** 值类型 */
  valueType?: ValueType
  /** 深度 */
  depth?: number
  /** 子节点 */
  children!: FeatureItem[]

  /** 构造器 */
  constructor(
    name: string,
    value: any,
    displayName?: string,
    description?: string,
    valueType?: ValueType,
    depth?: number
  ) {
    this.name = name
    this.depth = depth
    this.valueType = valueType
    this.displayName = displayName
    this.description = description
    this.children = new Array<FeatureItem>()
    if (value !== null) {
      this.value = value.toLowerCase() === 'true' ? true // boolean类型
        : !isNaN(Number(value)) ? Number(value) // number类型
          : value
    }
  }
}
