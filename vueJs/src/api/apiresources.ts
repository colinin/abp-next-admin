import ApiService from './serviceBase'
import { pagerFormat } from '@/utils/index'
import { FullAuditedEntityDto, PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class ApiResourceService {
  /**
   * 获取Api资源
   * @param id Api资源标识
   */
  public static getApiResourceById(id: string) {
    let _url = '/api/IdentityServer/ApiResources/'
    _url += id
    return ApiService.Get<ApiResource>(_url, serviceUrl)
  }

  /**
   * 获取Api资源列表
   * @param payload 查询参数
   */
  public static getApiResources(payload: ApiResourceGetByPaged) {
    let _url = '/api/IdentityServer/ApiResources'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    // abp设计原因,需要前端计算分页
    _url += '&skipCount=' + pagerFormat(payload.skipCount) * payload.maxResultCount
    // _url += '&skipCount=' + pagerFormat(payload.skipCount)
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<ApiResource>>(_url, serviceUrl)
  }

  /**
   * 创建Api资源
   * @param payload api资源参数
   */
  public static createApiResource(payload: ApiResourceCreate) {
    const _url = '/api/IdentityServer/ApiResources'
    return ApiService.Post<ApiResource>(_url, payload, serviceUrl)
  }

  /**
   * 变更Api资源
   * @param payload api资源参数
   */
  public static updateApiResource(payload: ApiResourceUpdate) {
    const _url = '/api/IdentityServer/ApiResources'
    return ApiService.Put<ApiResource>(_url, payload, serviceUrl)
  }

  /**
   * 删除Api资源
   * @param id Api资源标识
   */
  public static deleteApiResource(id: string) {
    let _url = '/api/IdentityServer/ApiResources/'
    _url += id
    return ApiService.Delete(_url, serviceUrl)
  }

  /**
   * 增加Api密钥
   * @param payload Api密钥参数
   */
  public static addApiSecret(payload: ApiSecretCreate) {
    const _url = '/api/IdentityServer/ApiResources/Secrets'
    return ApiService.Post<ApiSecret>(_url, payload, serviceUrl)
  }

  /**
   * 删除Api密钥
   * @param apiResourceId 资源标识
   * @param type 密钥类型
   * @param value 密钥值
   */
  public static deleteApiSecret(apiResourceId: string, type: string, value: string) {
    let _url = '/api/IdentityServer/ApiResources/Secrets/'
    _url += '?apiResourceId=' + apiResourceId
    _url += '&type=' + type
    _url += '&value=' + value
    return ApiService.Delete(_url, serviceUrl)
  }

  /**
   * 增加Api授权范围
   * @param payload api授权范围参数
   */
  public static addApiScope(payload: ApiScopeCreate) {
    const _url = '/api/IdentityServer/ApiResources/Scopes'
    return ApiService.Post<ApiScope>(_url, payload, serviceUrl)
  }

  /**
   * 删除Api授权范围
   * @param apiResourceId api资源标识
   * @param name 授权范围名称
   */
  public static deleteApiScope(apiResourceId: string, name: string) {
    let _url = '/api/IdentityServer/ApiResources/Scopes'
    _url += '?apiResourceId=' + apiResourceId
    _url += '&name=' + name
    return ApiService.Delete(_url, serviceUrl)
  }
}

export enum HashType {
  Sha256,
  Sha512
}

export class ApiSecret {
  type!: string
  value!: string
  hashType?: HashType
  description?: string
  expiration?: Date
}

export class ApiScopeClaim {
  type!: string
}

export class ApiResourceClaim {
  type!: string
}

export class ApiScope {
  name!: string
  displayName?: string
  description?: string
  required!: boolean
  emphasize!: boolean
  showInDiscoveryDocument!: boolean
  userClaims : ApiScopeClaim[]

  constructor() {
    this.userClaims = new Array<ApiScopeClaim>()
  }
}

export class ApiScopeCreate {
  apiResourceId!: string
  name!: string
  displayName?: string
  description?: string
  required!: boolean
  emphasize!: boolean
  showInDiscoveryDocument!: boolean
  userClaims : ApiScopeClaim[]

  constructor() {
    this.apiResourceId = ''
    this.name = ''
    this.displayName = ''
    this.description = ''
    this.required = false
    this.emphasize = false
    this.showInDiscoveryDocument = false
    this.userClaims = new Array<ApiScopeClaim>()
  }

  public static empty() {
    return new ApiScopeCreate()
  }
}

export class ApiSecretCreate {
  apiResourceId!: string
  type!: string
  value!: string
  hashType?: HashType
  description?: string
  expiration?: Date

  constructor() {
    this.type = 'SharedSecret'
    this.value = ''
    this.hashType = HashType.Sha256
    this.description = ''
    this.expiration = undefined
  }

  public static empty() {
    return new ApiSecretCreate()
  }
}

export class ApiResourceCreate {
  name!: string
  displayName?: string
  description?: string
  enabled!: boolean
  userClaims!: ApiResourceClaim[]

  constructor() {
    this.name = ''
    this.displayName = ''
    this.description = ''
    this.enabled = true
    this.userClaims = new Array<ApiResourceClaim>()
  }

  public static empty() {
    return new ApiResourceCreate()
  }

  public static create(apiResource: ApiResource) {
    const resource = ApiResourceCreate.empty()
    resource.name = apiResource.name
    resource.displayName = apiResource.displayName
    resource.description = apiResource.description
    resource.enabled = apiResource.enabled
    resource.userClaims = apiResource.userClaims
    return resource
  }
}

export class ApiResourceUpdate {
  id!: string
  displayName?: string
  description?: string
  enabled!: boolean
  userClaims!: ApiResourceClaim[]

  constructor() {
    this.id = ''
    this.displayName = ''
    this.description = ''
    this.enabled = true
    this.userClaims = new Array<ApiResourceClaim>()
  }

  public static empty() {
    return new ApiResourceUpdate()
  }

  public static create(apiResource: ApiResource) {
    const resource = ApiResourceUpdate.empty()
    resource.id = apiResource.id
    resource.displayName = apiResource.displayName
    resource.description = apiResource.description
    resource.enabled = apiResource.enabled
    resource.userClaims = apiResource.userClaims
    return resource
  }
}

export class ApiResource extends FullAuditedEntityDto {
  id!: string
  name!: string
  displayName?: string
  description?: string
  enabled!: boolean
  secrets!: ApiSecret[]
  scopes!: ApiScope[]
  userClaims!: ApiResourceClaim[]

  constructor() {
    super()
    this.id = ''
    this.name = ''
    this.displayName = ''
    this.description = ''
    this.enabled = true
    this.scopes = new Array<ApiScope>()
    this.secrets = new Array<ApiSecret>()
    this.userClaims = new Array<ApiResourceClaim>()
  }

  public static empty() {
    return new ApiResource()
  }
}

export class ApiResourceGetByPaged extends PagedAndSortedResultRequestDto {
  filter!: string

  constructor() {
    super()
    this.filter = ''
  }
}