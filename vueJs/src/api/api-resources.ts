import ApiService from './serviceBase'
import { FullAuditedEntityDto, PagedAndSortedResultRequestDto, PagedResultDto, SecretBase } from './types'

const sourceUrl = '/api/identity-server/api-resources'
const serviceUrl = process.env.VUE_APP_BASE_API

export default class ApiResourceService {
  /**
   * 获取Api资源
   * @param id Api资源标识
   */
  public static getApiResourceById(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Get<ApiResource>(_url, serviceUrl)
  }

  /**
   * 获取Api资源列表
   * @param payload 查询参数
   */
  public static getApiResources(payload: ApiResourceGetByPaged) {
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
  public static createApiResource(payload: ApiResourceCreate) {
    return ApiService.Post<ApiResource>(sourceUrl, payload, serviceUrl)
  }

  /**
   * 变更Api资源
   * @param payload api资源参数
   */
  public static updateApiResource(id: string, payload: ApiResourceUpdate) {
    const _url = sourceUrl + '/' + id
    return ApiService.Put<ApiResource>(_url, payload, serviceUrl)
  }

  /**
   * 删除Api资源
   * @param id Api资源标识
   */
  public static deleteApiResource(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Delete(_url, serviceUrl)
  }
}

export enum HashType {
  Sha256,
  Sha512
}

export class ApiScope {
  name = ''
  displayName?: string = ''
  description?: string = ''
  required = false
  emphasize = false
  showInDiscoveryDocument = true
  userClaims = new Array<string>()
}

export class ApiSecret extends SecretBase {}

export class ApiSecretCreateOrUpdate extends SecretBase {
  hashType = HashType.Sha256
}

export class ApiResourceCreateOrUpdate {
  displayName?: string = ''
  description?: string = ''
  enabled = true
  userClaims = new Array<string>()
  scopes = new Array<ApiScope>()
  secrets = new Array<ApiSecretCreateOrUpdate>()
  properties: {[key: string]: string} = {}
}

export class ApiResourceCreate extends ApiResourceCreateOrUpdate {
  name = ''
}

export class ApiResourceUpdate extends ApiResourceCreateOrUpdate {}

export class ApiResource extends FullAuditedEntityDto {
  id!: string
  name!: string
  displayName?: string
  description?: string
  enabled!: boolean
  userClaims = new Array<string>()
  scopes = new Array<ApiScope>()
  secrets = new Array<ApiSecretCreateOrUpdate>()
  properties: {[key: string]: string} = {}
}

export class ApiResourceGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
}
