import ApiService from './serviceBase'
import { UserClaim, Property } from './identity-server4'
import { ExtensibleAuditedEntity, PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const sourceUrl = '/api/identity-server/api-scopes'
const serviceUrl = process.env.VUE_APP_BASE_API

export default class ApiScopeService {
  /**
   * 获取Api范围
   * @param id Api资源标识
   */
  public static get(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Get<ApiScope>(_url, serviceUrl)
  }

  /**
   * 获取Api范围列表
   * @param payload 查询参数
   */
  public static getPagedList(payload: ApiScopeGetByPaged) {
    let _url = sourceUrl + '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<ApiScope>>(_url, serviceUrl)
  }

  /**
   * 创建Api范围
   * @param payload
   */
  public static create(payload: ApiScopeCreate) {
    return ApiService.Post<ApiScope>(sourceUrl, payload, serviceUrl)
  }

  /**
   * 变更Api范围
   * @param payload
   */
  public static update(id: string, payload: ApiScopeUpdate) {
    const _url = sourceUrl + '/' + id
    return ApiService.Put<ApiScope>(_url, payload, serviceUrl)
  }

  /**
   * 删除Api范围
   * @param id
   */
  public static delete(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Delete(_url, serviceUrl)
  }
}

export class ApiScopeClaim extends UserClaim {}

export class ApiScopeProperty extends Property {}

export class ApiScope extends ExtensibleAuditedEntity<string> {
  name!: string
  displayName?: string
  description?: string
  enabled!: boolean
  required!: boolean
  emphasize!: boolean
  showInDiscoveryDocument!: boolean
  userClaims = new Array<ApiScopeClaim>()
  properties = new Array<ApiScopeProperty>()
}

export class ApiScopeCreateOrUpdate {
  enabled = true
  displayName?: string = ''
  description?: string = ''
  required = false
  emphasize = false
  showInDiscoveryDocument = true
  userClaims = new Array<ApiScopeClaim>()
  properties = new Array<ApiScopeProperty>()
}

export class ApiScopeUpdate extends ApiScopeCreateOrUpdate {}

export class ApiScopeCreate extends ApiScopeCreateOrUpdate {
  name = ''
}

export class ApiScopeGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
}
