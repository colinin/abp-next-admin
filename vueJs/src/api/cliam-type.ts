import ApiService from './serviceBase'
import { ExtensibleEntity, ExtensibleObject, ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const IdentityServiceUrl = process.env.VUE_APP_BASE_API

export default class ClaimTypeApiService {
  public static createClaimType(payload: IdentityClaimTypeCreate) {
    const _url = '/api/identity/claim-types'
    return ApiService.Post<IdentityClaimType>(_url, payload, IdentityServiceUrl)
  }

  public static getActivedClaimTypes() {
    const _url = '/api/identity/claim-types/actived-list'
    return ApiService.Get<ListResultDto<IdentityClaimType>>(_url, IdentityServiceUrl)
  }

  public static getClaimTypes(payload: IdentityClaimTypeGetByPaged) {
    let _url = '/api/identity/claim-types?'
    _url += 'filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<IdentityClaimType>>(_url, IdentityServiceUrl)
  }

  public static getClaimType(id: string) {
    const _url = '/api/identity/claim-types/' + id
    return ApiService.Get<IdentityClaimType>(_url, IdentityServiceUrl)
  }

  public static updateClaimType(id: string, payload: IdentityClaimTypeUpdate) {
    const _url = '/api/identity/claim-types/' + id
    return ApiService.Put<IdentityClaimType>(_url, payload, IdentityServiceUrl)
  }

  public static deleteClaimType(id: string) {
    const _url = '/api/identity/claim-types/' + id
    return ApiService.Delete(_url, IdentityServiceUrl)
  }
}

export enum IdentityClaimValueType {
  String = 0,
  Int = 1,
  Boolean = 2,
  DateTime = 3
}

export class IdentityClaimType extends ExtensibleEntity<string> {
  name!: string
  required!: boolean
  isStatic!: boolean
  regex?: string
  regexDescription?: string
  description?: string
  valueType!: IdentityClaimValueType
}

export class IdentityClaimTypeCreateOrUpdateBase extends ExtensibleObject {
  required = false
  regex?: string = ''
  regexDescription?: string = ''
  description?: string = ''
}

export class IdentityClaimTypeCreate extends IdentityClaimTypeCreateOrUpdateBase {
  name!: string
  isStatic!: boolean
  valueType!: IdentityClaimValueType

  constructor(
    name: string,
    isStatic = false,
    valueType = IdentityClaimValueType.String
  ) {
    super()
    this.name = name
    this.isStatic = isStatic
    this.valueType = valueType
  }
}

export class IdentityClaimTypeUpdate extends IdentityClaimTypeCreateOrUpdateBase {
}

export class IdentityClaimTypeGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''

  constructor() {
    super()
    this.sorting = 'name'
  }
}
