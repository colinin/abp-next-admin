import { ListResultDto, PagedResultDto, PagedAndSortedResultRequestDto, FullAuditedEntityDto } from './types'
import ApiService from './serviceBase'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class TenantService {
  public static findTenantByName(name: string) {
    let _url = '/api/abp/multi-tenancy/tenants/by-name/'
    _url += name
    return ApiService.Get<FindTenantResult>(_url, serviceUrl)
  }

  public static findTenantById(id: string) {
    let _url = '/api/abp/multi-tenancy/tenants/by-id/'
    _url += id
    return ApiService.Get<FindTenantResult>(_url, serviceUrl)
  }

  public static getTenantById(id: string) {
    let _url = '/api/tenant-management/tenants/'
    _url += id
    return ApiService.Get<TenantDto>(_url, serviceUrl)
  }

  public static getTenants(payload: TenantGetByPaged) {
    let _url = '/api/tenant-management/tenants'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    // 因为abp设计的原因, 需要前端组合页面
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<TenantDto>>(_url, serviceUrl)
  }

  public static createTenant(payload: TenantCreateOrEdit) {
    const _url = '/api/tenant-management/tenants'
    return ApiService.Post<TenantDto>(_url, payload, serviceUrl)
  }

  public static changeTenantName(id: string, name: string) {
    let _url = '/api/tenant-management/tenants/'
    _url += id
    return ApiService.Put<TenantDto>(_url, { name: name }, serviceUrl)
  }

  public static deleteTenant(id: string) {
    let _url = '/api/tenant-management/tenants/'
    _url += id
    return ApiService.Delete(_url, serviceUrl)
  }

  public static getTenantConnections(id: string) {
    let _url = '/api/tenant-management/tenants/'
    _url += id + '/connection-string'
    return ApiService.Get<ListResultDto<TenantConnectionString>>(_url, serviceUrl)
  }

  public static getTenantConnectionByName(id: string, name: string) {
    let _url = '/api/tenant-management/tenants/'
    _url += id + '/connection-string/'
    _url += name
    return ApiService.Get<TenantConnectionString>(_url, serviceUrl)
  }

  public static setTenantConnection(id: string, payload: TenantConnectionString) {
    let _url = '/api/tenant-management/tenants/'
    _url += id + '/connection-string'
    return ApiService.Put<TenantConnectionString>(_url, payload, serviceUrl)
  }

  public static deleteTenantConnectionByName(id: string, name: string) {
    let _url = '/api/tenant-management/tenants/'
    _url += id + '/connection-string/'
    _url += name
    return ApiService.Delete(_url, serviceUrl)
  }
}

/** 租户查询过滤对象 */
export class TenantGetByPaged extends PagedAndSortedResultRequestDto {
  /** 查询过滤字段 */
  filter: string | undefined

  constructor() {
    super()
    this.filter = ''
    this.sorting = ''
    this.skipCount = 1
  }
}

/** 租户创建对象 */
export class TenantCreateOrEdit {
  /** 管理员邮件地址 */
  adminEmailAddress = ''
  /** 管理员密码 */
  adminPassword = ''
  /** 租户名称 */
  name = ''

  public static empty() {
    return new TenantCreateOrEdit()
  }
}

/** 租户对象 */
export class TenantDto extends FullAuditedEntityDto {
  /** 租户标识 */
  id!: string
  /** 租户名称 */
  name!: string
}

/** 租户连接字符串 */
export class TenantConnectionString {
  /** 名称 */
  name!: string
  /** 值 */
  value!: string

  public static empty() {
    const tenantConnection = new TenantConnectionString()
    tenantConnection.name = ''
    tenantConnection.value = ''
    return tenantConnection
  }
}

export class FindTenantResult {
  name!: string
  tenantId!: string
  success!: boolean
}
