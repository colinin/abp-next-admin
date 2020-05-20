import ApiService from './serviceBase'
import { pagerFormat } from '@/utils/index'
import { FullAuditedEntityDto, PagedResultDto, PagedAndSortedResultRequestDto } from './types'

/** 远程服务地址 */
const serviceUrl = process.env.VUE_APP_BASE_API

/** 身份资源Api接口 */
export default class IdentityResourceService {
  /** 
   * 获取身份资源
   * @param id 身份资源标识
   * @returns 返回类型为 IdentityResource 的对象
   */
  public static getIdentityResourceById(id: string) {
    let _url = '/api/IdentityServer/IdentityResources/'
    _url += id
    return ApiService.Get<IdentityResource>(_url, serviceUrl)
  }

  /**
   * 获取身份资源列表
   * @param payload 分页查询过滤对象
   * @returns 返回类型为 IdentityResource 的对象列表
   */
  public static getIdentityResources(payload: IdentityResourceGetByPaged) {
    let _url = '/api/IdentityServer/IdentityResources'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + pagerFormat(payload.skipCount)
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<IdentityResource>>(_url, serviceUrl)
  }

  /**
   * 创建身份资源
   * @param payload 类型为 IdentityResourceCreate 的对象
   * @returns 返回类型为 IdentityResource 的对象
   */
  public static createIdentityResource(payload: IdentityResourceCreate) {
    const _url = '/api/IdentityServer/IdentityResources'
    return ApiService.Post<IdentityResource>(_url, payload, serviceUrl)
  }

  /**
   * 变更身份资源
   * @param payload 类型为 IdentityResourceUpdate 的对象
   * @returns 返回类型为 IdentityResource 的对象
   */
  public static updateIdentityResource(payload: IdentityResourceUpdate) {
    const _url = '/api/IdentityServer/IdentityResources'
    return ApiService.Put<IdentityResource>(_url, payload, serviceUrl)
  }

  /**
   * 删除身份资源
   * @param id 身份资源标识
   */
  public static deleteIdentityResource(id: string) {
    let _url = '/api/IdentityServer/IdentityResources'
    _url += '?id=' + id
    return ApiService.Delete(_url, serviceUrl)
  }

  /**
   * 创建身份资源属性
   * @param payload 类型为 IdentityPropertyCreate 的对象
   * @returns 返回类型为 IdentityProperty 的对象
   */
  public static createProperty(payload: IdentityPropertyCreate) {
    const _url = '/api/IdentityServer/IdentityResources/Properties'
    return ApiService.Post<IdentityProperty>(_url, payload, serviceUrl)
  }

  /**
   * 删除身份资源属性
   * @param identityResourceId 身份资源标识
   * @param key 身份资源属性键
   */
  public static deleteProperty(identityResourceId: string, key: string) {
    let _url = '/api/IdentityServer/IdentityResources/Properties'
    _url += '?identityResourceId=' + identityResourceId
    _url += '&key=' + key
    return ApiService.Delete(_url, serviceUrl)
  }
}

/** 身份资源用户声明 */
export class IdentityClaim {
  /** 用户声明 */
  type = ''
}

/** 身份资源属性 */
export class IdentityProperty {
  /** 键 */
  key = ''
  /** 值 */
  value = ''
}

/** 身份资源属性创建对象 */
export class IdentityPropertyCreate {
  /** 身份资源标识 */
  identityResourceId = ''
  /** 键 */
  key = ''
  /** 值 */
  value = ''
  /** 并发令牌 */
  concurrencyStamp = ''

  /** 返回一个空对象 */
  public static empty() {
    return new IdentityPropertyCreate()
  }
}

/** 身份资源分页查询对象 */
export class IdentityResourceGetByPaged extends PagedAndSortedResultRequestDto {
  /** 过滤参数 */
  filter = ''

  /** 返回一个空对象 */
  public static empty() {
    return new IdentityResourceGetByPaged()
  }
}

/** 身份资源创建对象 */
export class IdentityResourceCreate {
  /** 名称 */
  name = ''
  /** 显示名称 */
  displayName? = ''
  /** 说明 */
  description? = ''
  /** 启用 */
  enabled = true
  /** 必须 */
  required = false
  /** 强调 */
  emphasize = false
  /** 在发现文档显示 */
  showInDiscoveryDocument = false
  /** 用户声明 */
  userClaims = new Array<IdentityClaim>()

  /** 返回一个空对象 */
  public static empty() {
    return new IdentityResourceCreate()
  }

  /** 创建身份资源 */
  public static create(identityResource: IdentityResource) {
    let resource = new IdentityResourceCreate()
    resource.description = identityResource.description
    resource.displayName = identityResource.displayName
    resource.emphasize = identityResource.emphasize
    resource.enabled = identityResource.enabled
    resource.name = identityResource.name
    resource.required = identityResource.required
    resource.showInDiscoveryDocument = identityResource.showInDiscoveryDocument
    resource.userClaims = identityResource.userClaims
    return resource
  }
}

/** 身份资源变更对象 */
export class IdentityResourceUpdate {
  /** 身份资源标识 */
  id = ''
  /** 名称 */
  name = ''
  /** 显示名称 */
  displayName? = ''
  /** 说明 */
  description? = ''
  /** 启用 */
  enabled = true
  /** 必须 */
  required = false
  /** 强调 */
  emphasize = false
  /** 在发现文档显示 */
  showInDiscoveryDocument = false
  /** 并发令牌 */
  concurrencyStamp = ''
  /** 用户声明 */
  userClaims = new Array<IdentityClaim>()

  /** 返回一个空对象 */
  public static empty() {
    return new IdentityResourceUpdate()
  }

  /** 创建身份资源 */
  public static create(identityResource: IdentityResource) {
    let resource = new IdentityResourceUpdate()
    resource.concurrencyStamp = identityResource.concurrencyStamp
    resource.description = identityResource.description
    resource.displayName = identityResource.displayName
    resource.emphasize = identityResource.emphasize
    resource.enabled = identityResource.enabled
    resource.id = identityResource.id
    resource.name = identityResource.name
    resource.required = identityResource.required
    resource.showInDiscoveryDocument = identityResource.showInDiscoveryDocument
    resource.userClaims = identityResource.userClaims
    return resource
  }
}

/** 身份资源对象 */
export class IdentityResource extends FullAuditedEntityDto {
  /** 身份资源标识 */
  id!: string
  /** 名称 */
  name!: string
  /** 显示名称 */
  displayName?: string
 /** 说明 */
  description?: string
  /** 并发令牌 */
  concurrencyStamp!: string
 /** 启用 */
  enabled!: boolean
  /** 必须 */
  required!: boolean
  /** 强调 */
  emphasize!: boolean
  /** 在发现文档显示 */
  showInDiscoveryDocument!: boolean
  /** 用户声明 */
  userClaims!: IdentityClaim[]
  /** 属性 */
  properties!: IdentityProperty[]

  /** 返回一个空对象 */
  public static empty() {
    const resource =  new IdentityResource()
    resource.enabled = true
    return resource
  }
}
