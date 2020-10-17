import ApiService from './serviceBase'
import { FullAuditedEntityDto, PagedAndSortedResultRequestDto, PagedResultDto, SecretBase, Claim } from './types'

const sourceUrl = '/api/identity-server/clients'
const serviceUrl = process.env.VUE_APP_BASE_API

export default class ClientService {
  public static getClientById(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Get<Client>(_url, serviceUrl)
  }

  public static getClients(payload: ClientGetByPaged) {
    let _url = sourceUrl + '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<Client>>(_url, serviceUrl)
  }

  public static createClient(payload: ClientCreate) {
    return ApiService.Post<Client>(sourceUrl, payload, serviceUrl)
  }

  public static cloneClient(id: string, payload: ClientClone) {
    const _url = sourceUrl + '/' + id + '/clone'
    return ApiService.Post<Client>(_url, payload, serviceUrl)
  }

  public static updateClient(id: string, payload: ClientUpdate) {
    const _url = sourceUrl + '/' + id
    return ApiService.Put<Client>(_url, payload, serviceUrl)
  }

  public static deleteClient(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Delete(_url, serviceUrl)
  }
}

export enum HashType {
  Sha256 = 0,
	Sha512 = 1
}

export class SecretCreateOrUpdate extends SecretBase {
  hashType = HashType.Sha256
}

export class ClientGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
}

export class ClientClaim extends Claim {}

export class ClientClone {
  sourceClientId = ''
  clientId = ''
  clientName = ''
  description? = ''
  copyAllowedGrantType = true
  copyRedirectUri = true
  copyAllowedScope = true
  copyClaim = true
  copyAllowedCorsOrigin = true
  copyPostLogoutRedirectUri = true
  copyPropertie = true
  copyIdentityProviderRestriction = true

  public static empty() {
    return new ClientClone()
  }
}

export class Client extends FullAuditedEntityDto {
  id!: string
  clientId!: string
  clientName!: string
  description?: string
  concurrencyStamp!: string
  clientUri?: string
  logoUri?: string
  enabled!: boolean
  protocolType!: string
  requireClientSecret!: boolean
  requireConsent!: boolean
  allowRememberConsent!: boolean
  alwaysIncludeUserClaimsInIdToken!: boolean
  requirePkce!: boolean
  allowPlainTextPkce!: boolean
  allowAccessTokensViaBrowser!: boolean
  frontChannelLogoutUri?: string
  frontChannelLogoutSessionRequired!: boolean
  backChannelLogoutUri?: string
  backChannelLogoutSessionRequired!: boolean
  allowOfflineAccess!: boolean
  identityTokenLifetime!: number
  accessTokenLifetime!: number
  authorizationCodeLifetime!: number
  consentLifetime?: number
  absoluteRefreshTokenLifetime!: number
  slidingRefreshTokenLifetime!: number
  refreshTokenUsage!: number
  updateAccessTokenClaimsOnRefresh!: boolean
  refreshTokenExpiration!: number
  accessTokenType!: number
  enableLocalLogin!: boolean
  includeJwtId!: boolean
  alwaysSendClientClaims!: boolean
  clientClaimsPrefix?: string
  pairWiseSubjectSalt?: string
  userSsoLifetime!: number
  userCodeType?: string
  deviceCodeLifetime!: number
  allowedScopes = new Array<string>()
  clientSecrets = new Array<string>()
  allowedGrantTypes = new Array<string>()
  allowedCorsOrigins = new Array<string>()
  redirectUris = new Array<string>()
  postLogoutRedirectUris = new Array<string>()
  identityProviderRestrictions = new Array<string>()
  claims = new Array<string>()
  properties = new Array<string>()
}

export class ClientCreateOrUpdate {
  clientId = ''
  clientName = ''
  description? = ''
  allowedGrantTypes = new Array<string>()
}

export class ClientCreate extends ClientCreateOrUpdate {}

export class ClientUpdate extends ClientCreateOrUpdate {
  clientUri? = ''
  logoUri? = ''
  enabled = true
  protocolType = 'oidc'
  requireClientSecret = true
  requireConsent = true
  allowRememberConsent = true
  alwaysIncludeUserClaimsInIdToken = false
  requirePkce = false
  allowPlainTextPkce = false
  allowAccessTokensViaBrowser = false
  frontChannelLogoutUri? = ''
  frontChannelLogoutSessionRequired = true
  backChannelLogoutUri? = ''
  backChannelLogoutSessionRequired = true
  allowOfflineAccess = true
  identityTokenLifetime = 300
  accessTokenLifetime = 3600
  authorizationCodeLifetime = 300
  consentLifetime?: number
  absoluteRefreshTokenLifetime = 2592000
  slidingRefreshTokenLifetime = 1296000
  refreshTokenUsage = 1
  updateAccessTokenClaimsOnRefresh = false
  refreshTokenExpiration = 1
  accessTokenType = 0
  enableLocalLogin = true
  includeJwtId = false
  alwaysSendClientClaims = false
  clientClaimsPrefix? = 'client_'
  pairWiseSubjectSalt? = ''
  userSsoLifetime!: number
  userCodeType? = ''
  deviceCodeLifetime = 300
  apiResources = new Array<string>()
  identityResources = new Array<string>()
  allowedCorsOrigins = new Array<string>()
  redirectUris = new Array<string>()
  postLogoutRedirectUris = new Array<string>()
  identityProviderRestrictions = new Array<string>()
  properties: {[key: string]: string} = {}
  secrets = new Array<SecretCreateOrUpdate>()
  claims = new Array<ClientClaim>()
}
