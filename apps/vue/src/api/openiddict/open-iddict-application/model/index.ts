export interface OpenIddictApplicationGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface OpenIddictApplicationCreateDto extends OpenIddictApplicationCreateOrUpdateDto {
}

export interface OpenIddictApplicationUpdateDto extends OpenIddictApplicationCreateOrUpdateDto {
}

export interface OpenIddictApplicationDto extends ExtensibleAuditedEntityDto<string> {
  clientId?: string;
  clientSecret?: string;
  consentType?: string;
  displayName?: string;
  displayNames?: Dictionary<string, string>;
  endpoints?: string[];
  grantTypes?: string[];
  responseTypes?: string[];
  scopes?: string[];
  postLogoutRedirectUris?: string[];
  properties?: Dictionary<string, string>;
  redirectUris?: string[];
  requirements?: string[];
  type?: string;
  clientUri?: string;
  logoUri?: string;
}

export interface OpenIddictApplicationCreateOrUpdateDto extends ExtensibleObject {
  clientId: string;
  clientSecret?: string;
  consentType?: string;
  displayName?: string;
  displayNames?: Dictionary<string, string>;
  endpoints?: string[];
  grantTypes?: string[];
  responseTypes?: string[];
  scopes?: string[];
  postLogoutRedirectUris?: string[];
  properties?: Dictionary<string, string>;
  redirectUris?: string[];
  requirements?: string[];
  type?: string;
  clientUri?: string;
  logoUri?: string;
}

