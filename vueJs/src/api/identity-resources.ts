import ApiService from './serviceBase'
import { Property, UserClaim } from './identity-server4'
import { ExtensibleAuditedEntity, PagedResultDto, PagedAndSortedResultRequestDto } from './types'

const sourceUrl = '/api/identity-server/identity-resources'
/** 远程服务地址 */
const serviceUrl = process.env.VUE_APP_BASE_API

/** 身份资源Api接口 */
export default class IdentityResourceService {
  /**
   * 获取身份资源
   * @param id 身份资源标识
   * @returns 返回类型为 IdentityResource 的对象
   */
  public static get(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Get<IdentityResource>(_url, serviceUrl)
  }

  /**
   * 获取身份资源列表
   * @param payload 分页查询过滤对象
   * @returns 返回类型为 IdentityResource 的对象列表
   */
  public static getList(payload: IdentityResourceGetByPaged) {
    let _url = sourceUrl + '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<IdentityResource>>(_url, serviceUrl)
  }

  /**
   * 创建身份资源
   * @param payload 类型为 IdentityResourceCreate 的对象
   * @returns 返回类型为 IdentityResource 的对象
   */
  public static create(payload: IdentityResourceCreateOrUpdate) {
    return ApiService.Post<IdentityResource>(sourceUrl, payload, serviceUrl)
  }

  /**
   * 变更身份资源
   * @param payload 类型为 IdentityResourceUpdate 的对象
   * @returns 返回类型为 IdentityResource 的对象
   */
  public static update(id: string, payload: IdentityResourceCreateOrUpdate) {
    const _url = sourceUrl + '/' + id
    return ApiService.Put<IdentityResource>(_url, payload, serviceUrl)
  }

  /**
   * 删除身份资源
   * @param id 身份资源标识
   */
  public static delete(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Delete(_url, serviceUrl)
  }
}

export class IdentityUserClaim extends UserClaim {}

export class IdentityResourceProperty extends Property {}

export class IdentityResource extends ExtensibleAuditedEntity<string> {
  name = ''
  displayName?: string = ''
  description?: string = ''
  enabled = true
  required = false
  emphasize = false
  showInDiscoveryDocument = true
  userClaims = new Array<IdentityUserClaim>()
  properties = new Array<IdentityResourceProperty>()
}

export class IdentityResourceGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
}

export class IdentityResourceCreateOrUpdate {
  name = ''
  displayName?: string = ''
  description?: string = ''
  enabled = true
  required = false
  emphasize = false
  showInDiscoveryDocument = true
  userClaims = new Array<IdentityUserClaim>()
  properties = new Array<IdentityResourceProperty>()
}
