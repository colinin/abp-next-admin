
import ApiService from './serviceBase'
import { pagerFormat } from '@/utils/index'
import { FullAuditedEntityDto, PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class ClientService {
  public static getClientById(id: string) {
    let _url = '/api/IdentityServer/Clients/'
    _url += id
    return ApiService.Get<Client>(_url, serviceUrl)
  }

  public static getClients(payload: ClientGetByPaged) {
    let _url = '/api/IdentityServer/Clients'
    _url += '?sorting=' + payload.sorting
    _url += '&skipCount=' + pagerFormat(payload.skipCount)
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<Client>>(_url, serviceUrl)
  }

  public static createClient(payload: ClientCreate) {
    const _url = '/api/IdentityServer/Clients'
    return ApiService.Post<Client>(_url, payload, serviceUrl)
  }

  public static cloneClient(payload: ClientClone) {
    const _url = '/api/IdentityServer/Clients/Clone'
    return ApiService.Post<Client>(_url, payload, serviceUrl)
  }

  public static updateClient(payload: ClientUpdate) {
    const _url = '/api/IdentityServer/Clients'
    return ApiService.Put<Client>(_url, payload, serviceUrl)
  }

  public static deleteClient(id: string) {
    const _url = '/api/IdentityServer/Clients/' + id
    return ApiService.Delete(_url, serviceUrl)
  }

  public static addClientSecret(payload: ClientSecretCreate) {
    const _url = '/api/IdentityServer/Clients/Secrets'
    return ApiService.Post<ClientSecret>(_url, payload, serviceUrl)
  }

  public static deleteClientSecret(clientId: string, type: string, value: string) {
    let _url = '/api/IdentityServer/Clients/Secrets'
    _url += '?clientId=' + clientId
    _url += '&type=' + type
    _url += '&value=' + value
    return ApiService.Delete(_url, serviceUrl)
  }

  public static addClientProperty(payload: ClientPropertyCreate) {
    const _url = '/api/IdentityServer/Clients/Properties'
    return ApiService.Post<ClientProperty>(_url, payload, serviceUrl)
  }

  public static deleteClientProperty(clientId: string, key: string, value: string) {
    let _url = '/api/IdentityServer/Clients/Properties'
    _url += '?clientId=' + clientId
    _url += '&key=' + key
    _url += '&value=' + value
    return ApiService.Delete(_url, serviceUrl)
  }

  public static addClientClaim(payload: ClientClaimCreate) {
    const _url = '/api/IdentityServer/Clients/Claims'
    return ApiService.Post<ClientClaim>(_url, payload, serviceUrl)
  }

  public static deleteClientClaim(clientId: string, type: string, value: string) {
    let _url = '/api/IdentityServer/Clients/Claims'
    _url += '?clientId=' + clientId
    _url += '&type=' + type
    _url += '&value=' + value
    return ApiService.Delete(_url, serviceUrl)
  }
}

export class ClientGetByPaged extends PagedAndSortedResultRequestDto {
  filter?: string
}

export enum HashType {
  Sha256,
  Sha512
}

export class ClientSecret {
  type = ''
  value = ''
  hashType = HashType.Sha256
  description? = ''
  expiration? = undefined
}

export class ClientRedirectUri {
  redirectUri = ''
}

export class ClientClaim {
  type = ''
  value = ''
}

export class ClientCorsOrigin {
  origin = ''
}

export class ClientGrantType {
  grantType = ''
}

export class ClientIdPRestriction {
  provider = ''
}

export class ClientPostLogoutRedirectUri {
  postLogoutRedirectUri = ''
}

export class ClientProperty {
  key = ''
  value = ''
}

export class ClientScope {
  scope = ''
}

export class ClientSecretCreate extends ClientSecret {
  clientId!: string

  constructor() {
    super()
    this.type = 'SharedSecret'
    this.hashType = HashType.Sha256
  }
}

export class ClientClaimCreate extends ClientClaim {
  clientId!: string
}

export class ClientPropertyCreate extends ClientProperty {
  clientId!: string
}

export class ClientCreate {
  clientId = ''
  clientName = ''
  description? = ''
  allowedGrantTypes?: ClientGrantType[]

  constructor() {
    this.allowedGrantTypes = new Array<ClientGrantType>()
  }
}

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
  allowedScopes!: ClientScope[]
  clientSecrets!: ClientSecret[]
  allowedGrantTypes!: ClientGrantType[]
  allowedCorsOrigins!: ClientCorsOrigin[]
  redirectUris!: ClientRedirectUri[]
  postLogoutRedirectUris!: ClientPostLogoutRedirectUri[]
  identityProviderRestrictions!: ClientIdPRestriction[]
  claims!: ClientClaim[]
  properties!: ClientProperty[]

  constructor() {
    super()
    this.allowedScopes = new Array<ClientScope>()
    this.clientSecrets = new Array<ClientSecret>()
    this.allowedGrantTypes = new Array<ClientGrantType>()
    this.allowedCorsOrigins = new Array<ClientCorsOrigin>()
    this.redirectUris = new Array<ClientRedirectUri>()
    this.postLogoutRedirectUris = new Array<ClientPostLogoutRedirectUri>()
    this.identityProviderRestrictions = new Array<ClientIdPRestriction>()
    this.claims = new Array<ClientClaim>()
    this.properties = new Array<ClientProperty>()
  }

  public static empty() {
    return new Client()
  }
}

export class ClientUpdateData {
  concurrencyStamp!: string
  clientId = ''
  clientName = ''
  description? = ''
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
  allowOfflineAccess = false
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
  allowedScopes!: ClientScope[]
  allowedGrantTypes!: ClientGrantType[]
  allowedCorsOrigins!: ClientCorsOrigin[]
  redirectUris!: ClientRedirectUri[]
  postLogoutRedirectUris!: ClientPostLogoutRedirectUri[]
  identityProviderRestrictions!: ClientIdPRestriction[]

  constructor() {
    this.allowedScopes = new Array<ClientScope>()
    this.allowedGrantTypes = new Array<ClientGrantType>()
    this.allowedCorsOrigins = new Array<ClientCorsOrigin>()
    this.redirectUris = new Array<ClientRedirectUri>()
    this.postLogoutRedirectUris = new Array<ClientPostLogoutRedirectUri>()
    this.identityProviderRestrictions = new Array<ClientIdPRestriction>()
  }

  public setClient(client: Client) {
    this.absoluteRefreshTokenLifetime = client.absoluteRefreshTokenLifetime
    this.accessTokenLifetime = client.accessTokenLifetime
    this.accessTokenType = client.accessTokenType
    this.allowAccessTokensViaBrowser = client.allowAccessTokensViaBrowser
    this.allowOfflineAccess = client.allowOfflineAccess
    this.allowPlainTextPkce = client.allowPlainTextPkce
    this.allowRememberConsent = client.allowRememberConsent
    this.allowedCorsOrigins = client.allowedCorsOrigins
    this.allowedGrantTypes = client.allowedGrantTypes
    this.allowedScopes = client.allowedScopes
    this.alwaysIncludeUserClaimsInIdToken = client.alwaysIncludeUserClaimsInIdToken
    this.alwaysSendClientClaims = client.alwaysSendClientClaims
    this.authorizationCodeLifetime = client.authorizationCodeLifetime
    this.backChannelLogoutSessionRequired = client.backChannelLogoutSessionRequired
    this.backChannelLogoutUri = client.backChannelLogoutUri
    this.clientClaimsPrefix = client.clientClaimsPrefix
    this.clientId = client.clientId
    this.clientName = client.clientName
    this.clientUri = client.clientUri
    this.concurrencyStamp = client.concurrencyStamp
    this.consentLifetime = client.consentLifetime
    this.description = client.description
    this.deviceCodeLifetime = client.deviceCodeLifetime
    this.enableLocalLogin = client.enableLocalLogin
    this.enabled = client.enabled
    this.frontChannelLogoutSessionRequired = client.frontChannelLogoutSessionRequired
    this.frontChannelLogoutUri = client.frontChannelLogoutUri
    this.identityProviderRestrictions = client.identityProviderRestrictions
    this.identityTokenLifetime = client.identityTokenLifetime
    this.includeJwtId = client.includeJwtId
    this.logoUri = client.logoUri
    this.pairWiseSubjectSalt = client.pairWiseSubjectSalt
    this.postLogoutRedirectUris = client.postLogoutRedirectUris
    this.protocolType = client.protocolType
    this.redirectUris = client.redirectUris
    this.refreshTokenExpiration = client.refreshTokenExpiration
    this.refreshTokenUsage = client.refreshTokenUsage
    this.requireClientSecret = client.requireClientSecret
    this.requireConsent = client.requireConsent
    this.requirePkce = client.requirePkce
    this.slidingRefreshTokenLifetime = client.slidingRefreshTokenLifetime
    this.updateAccessTokenClaimsOnRefresh = client.updateAccessTokenClaimsOnRefresh
    this.userCodeType = client.userCodeType
    this.userSsoLifetime = client.userSsoLifetime
  }
}

export class ClientUpdate {
  id!: string
  client!: ClientUpdateData

  constructor() {
    this.client = new ClientUpdateData()
  }
}
