import ApiService from './serviceBase'
import { Secret, Scope, UserClaim, Property } from './identity-server4'
import { AuditedEntityDto, PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const sourceUrl = '/api/identity-server/api-resources'
const serviceUrl = process.env.VUE_APP_BASE_API

export default class ApiResourceService {
  /**
   * 获取Api资源
   * @param id Api资源标识
   */
  public static get(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Get<ApiResource>(_url, serviceUrl)
  }

  /**
   * 获取Api资源列表
   * @param payload 查询参数
   */
  public static getList(payload: ApiResourceGetByPaged) {
    let _url = sourceUrl + '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<ApiResource>>(_url, serviceUrl)
  }

  /**
   * 创建Api资源
   * @param payload api资源参数
   */
  public static create(payload: ApiResourceCreate) {
    return ApiService.Post<ApiResource>(sourceUrl, payload, serviceUrl)
  }

  /**
   * 变更Api资源
   * @param payload api资源参数
   */
  public static update(id: string, payload: ApiResourceUpdate) {
    const _url = sourceUrl + '/' + id
    return ApiService.Put<ApiResource>(_url, payload, serviceUrl)
  }

  /**
   * 删除Api资源
   * @param id Api资源标识
   */
  public static delete(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Delete(_url, serviceUrl)
  }
}

export enum HashType {
  Sha256,
  Sha512
}

export class ApiResourceSecret extends Secret {}

export class ApiResourceSecretCreateOrUpdate extends Secret {
  hashType = HashType.Sha256
}

export class ApiResourceScope extends Scope {}

export class ApiResourceClaim extends UserClaim {}

export class ApiResourceProperty extends Property {}

export class ApiResource extends AuditedEntityDto {
  id!: string
  name!: string
  displayName?: string
  description?: string
  enabled!: boolean
  userClaims = new Array<ApiResourceClaim>()
  scopes = new Array<ApiResourceScope>()
  secrets = new Array<ApiResourceSecret>()
  properties = new Array<ApiResourceProperty>()
  /** 允许访问令牌签名算法 */
  allowedAccessTokenSigningAlgorithms?: string
  /** 在发现文档中显示 */
  showInDiscoveryDocument!: boolean
}

export class ApiResourceCreateOrUpdate {
  enabled = true
  displayName?: string = ''
  description?: string = ''
  showInDiscoveryDocument = false
  allowedAccessTokenSigningAlgorithms?: string = ''
  userClaims = new Array<ApiResourceClaim>()
  scopes = new Array<ApiResourceScope>()
  secrets = new Array<ApiResourceSecret>()
  properties = new Array<ApiResourceProperty>()
}

export class ApiResourceCreate extends ApiResourceCreateOrUpdate {
  name = ''
}

export class ApiResourceUpdate extends ApiResourceCreateOrUpdate {}

export class ApiResourceGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
}
