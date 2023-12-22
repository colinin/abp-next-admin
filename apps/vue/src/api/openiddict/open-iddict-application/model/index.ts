export interface OpenIddictApplicationGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export type OpenIddictApplicationCreateDto = OpenIddictApplicationCreateOrUpdateDto;

export type OpenIddictApplicationUpdateDto = OpenIddictApplicationCreateOrUpdateDto;

export interface OpenIddictApplicationDto extends ExtensibleAuditedEntityDto<string> {
  clientId: string;
  clientSecret?: string;
  clientType?: string;
  applicationType?: string;
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
  clientUri?: string;
  logoUri?: string;
}

export interface OpenIddictApplicationCreateOrUpdateDto extends ExtensibleObject {
  clientId: string;
  clientSecret?: string;
  clientType?: string;
  applicationType?: string;
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
  clientUri?: string;
  logoUri?: string;
}
