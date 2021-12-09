import {
  ExtensibleAuditedEntity,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
  SecretBase,
} from '/@/api/model/baseModel';
import { UserClaim, Property } from './basicModel';

export interface ClientClaim extends UserClaim {
  value: string;
}

export interface ClientScope {
  scope: string;
}

export interface ClientGrantType {
  grantType: string;
}

export interface ClientCorsOrigin {
  origin: string;
}

export interface ClientRedirectUri {
  redirectUri: string;
}

export interface ClientPostLogoutRedirectUri {
  postLogoutRedirectUri: string;
}

export interface ClientIdPRestriction {
  provider: string;
}

export type ClientSecret = SecretBase;

export type ClientProperty = Property;

export interface Client extends ExtensibleAuditedEntity<string> {
  clientId: string;
  clientName: string;
  description: string;
  clientUri: string;
  logoUri: string;
  enabled: boolean;
  protocolType: string;
  requireClientSecret: boolean;
  requireConsent: boolean;
  allowRememberConsent: boolean;
  requireRequestObject: boolean;
  allowedIdentityTokenSigningAlgorithms: string;
  alwaysIncludeUserClaimsInIdToken: boolean;
  requirePkce: boolean;
  allowPlainTextPkce: boolean;
  allowAccessTokensViaBrowser: boolean;
  frontChannelLogoutUri: string;
  frontChannelLogoutSessionRequired: boolean;
  backChannelLogoutUri: string;
  backChannelLogoutSessionRequired: boolean;
  allowOfflineAccess: boolean;
  identityTokenLifetime: number;
  accessTokenLifetime: number;
  authorizationCodeLifetime: number;
  consentLifetime?: number;
  absoluteRefreshTokenLifetime: number;
  slidingRefreshTokenLifetime: number;
  refreshTokenUsage: number;
  updateAccessTokenClaimsOnRefresh: boolean;
  refreshTokenExpiration: number;
  accessTokenType: number;
  enableLocalLogin: boolean;
  includeJwtId: boolean;
  alwaysSendClientClaims: boolean;
  clientClaimsPrefix: string;
  pairWiseSubjectSalt: string;
  userSsoLifetime?: number;
  userCodeType: string;
  deviceCodeLifetime: number;
  concurrencyStamp: string;
  allowedScopes: ClientScope[];
  clientSecrets: ClientSecret[];
  allowedGrantTypes: ClientGrantType[];
  allowedCorsOrigins: ClientCorsOrigin[];
  redirectUris: ClientRedirectUri[];
  postLogoutRedirectUris: ClientPostLogoutRedirectUri[];
  identityProviderRestrictions: ClientIdPRestriction[];
  claims: ClientClaim[];
  properties: ClientProperty[];
}

export interface ClientCreateOrUpdate {
  clientId: string;
  clientName: string;
  description: string;
  allowedGrantTypes: ClientGrantType[];
}

export type ClientCreate = ClientCreateOrUpdate;

export interface ClientUpdate extends ClientCreateOrUpdate {
  clientUri: string;
  logoUri: string;
  enabled: boolean;
  protocolType: string;
  requireClientSecret: boolean;
  allowedIdentityTokenSigningAlgorithms: string;
  requireConsent: boolean;
  requireRequestObject: boolean;
  allowRememberConsent: boolean;
  alwaysIncludeUserClaimsInIdToken: boolean;
  requirePkce: boolean;
  allowPlainTextPkce: boolean;
  allowAccessTokensViaBrowser: boolean;
  frontChannelLogoutUri: string;
  frontChannelLogoutSessionRequired: boolean;
  backChannelLogoutUri: string;
  backChannelLogoutSessionRequired: boolean;
  allowOfflineAccess: boolean;
  identityTokenLifetime: number;
  accessTokenLifetime: number;
  authorizationCodeLifetime: number;
  consentLifetime?: number;
  absoluteRefreshTokenLifetime: number;
  slidingRefreshTokenLifetime: number;
  refreshTokenUsage: number;
  updateAccessTokenClaimsOnRefresh: boolean;
  refreshTokenExpiration: number;
  accessTokenType: number;
  enableLocalLogin: boolean;
  includeJwtId: boolean;
  alwaysSendClientClaims: boolean;
  clientClaimsPrefix: string;
  pairWiseSubjectSalt: string;
  userSsoLifetime?: number;
  userCodeType: string;
  deviceCodeLifetime: number;
  allowedScopes: ClientScope[];
  clientSecrets: ClientSecret[];
  allowedCorsOrigins: ClientCorsOrigin[];
  redirectUris: ClientRedirectUri[];
  postLogoutRedirectUris: ClientPostLogoutRedirectUri[];
  identityProviderRestrictions: ClientIdPRestriction[];
  properties: ClientProperty[];
  claims: ClientClaim[];
}

export interface ClientClone {
  clientId: string;
  clientName: string;
  description: string;
  CopyAllowedGrantType: boolean;
  CopyRedirectUri: boolean;
  CopyAllowedScope: boolean;
  CopyClaim: boolean;
  CopySecret: boolean;
  CopyAllowedCorsOrigin: boolean;
  CopyPostLogoutRedirectUri: boolean;
  CopyPropertie: boolean;
  CopyIdentityProviderRestriction: boolean;
}

export class ClientPagedResult extends PagedResultDto<Client> {}

export class GetClientPagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}
