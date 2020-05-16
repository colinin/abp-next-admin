
import ApiService from './serviceBase'
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
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<Client>>(_url, serviceUrl)
  }
}

export class ClientGetById {
  id!: string
}

export class ClientGetByPaged extends PagedAndSortedResultRequestDto {
  filter?: string
}

export class ClientSecret {
  type!: string
  value!: string
  description: string | undefined
  expiration: Date | undefined
}

export class ClientRedirectUri {
  redirectUri!: string
}

export class ClientClaim {
  type!: string
  value!: string
}

export class ClientCorsOrigin {
  origin!: string
}

export class ClientGrantType {
  grantType!: string
}

export class ClientIdPRestriction {
  provider!: string
}

export class ClientPostLogoutRedirectUri {
  postLogoutRedirectUri!: string
}

export class ClientProperty {
  key!: string
  value!: string
}

export class ClientScope {
  scope!: string
}

export class Client extends FullAuditedEntityDto {
  id!: string
  clientId!: string
  clientName!: string
  description?: string
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
  identityTokenLifetime?: number
  accessTokenLifetime?: number
  authorizationCodeLifetime?: number
  consentLifetime?: number
  absoluteRefreshTokenLifetime?: number
  slidingRefreshTokenLifetime?: number
  refreshTokenUsage?: number
  updateAccessTokenClaimsOnRefresh!: boolean
  refreshTokenExpiration?: number
  accessTokenType?: number
  enableLocalLogin!: boolean
  includeJwtId!: boolean
  alwaysSendClientClaims!: boolean
  clientClaimsPrefix?: string
  pairWiseSubjectSalt?: string
  userSsoLifetime?: number
  userCodeType?: string
  deviceCodeLifetime?: number
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
}
