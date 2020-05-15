import { pagerFormat } from '@/utils'
import { PagedResultDto, PagedAndSortedResultRequestDto } from './types'
import ApiService from './serviceBase'

const serviceUrl = process.env.VUE_APP_BASE_IDENTITY_SERVICE

export default class TenantService {
  public static getTenantByName(name: string) {
    let _url = '/api/abp/multi-tenancy/tenants/by-name/'
    _url += name
    return ApiService.Get<FindTenantResult>(_url, serviceUrl)
  }

  public static getTenants(payload: TenantGetRequestDto) {
    let _url = '/api/multi-tenancy/tenants'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + pagerFormat(payload.skipCount)
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<TenantDto>>(_url, serviceUrl)
  }
}

/** 租户查询过滤对象 */
export class TenantGetRequestDto extends PagedAndSortedResultRequestDto {
  /** 查询过滤字段 */
  filter: string | undefined

  constructor() {
    super()
    this.filter = ''
    this.sorting = ''
    this.skipCount = 1
    this.maxResultCount = 25
  }
}

/** 租户对象 */
export class TenantDto {
  /** 租户标识 */
  id!: string
  /** 租户名称 */
  name!: string
}

export class FindTenantResult {
  name!: string
  tenantId!: string
  success!: boolean
}
