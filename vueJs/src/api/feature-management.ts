import ApiService from './serviceBase'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class FeatureManagementService {
  public static getFeatures(providerName: string, providerKey: string) {
    let _url = '/api/feature-management/features?'
    _url += 'providerName=' + providerName
    _url += '&providerKey=' + providerKey
    return ApiService.Get<Features>(_url, serviceUrl)
  }

  public static updateFeatures(providerName: string, providerKey: string, features: Features) {
    let _url = '/api/feature-management/features?'
    _url += 'providerName=' + providerName
    _url += '&providerKey=' + providerKey
    return ApiService.Put<void>(_url, features, serviceUrl)
  }
}

export class Provider {
  providerName!: string
  providerKey!: string
}

export class ValueType {
  name!: string
  properties!: any
  validator!: any
}

export class Feature {
  name!: string
  displayName?: string
  value!: string
  description?: string
  valueType?: ValueType
  depth?: number
  parentName?: string

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
