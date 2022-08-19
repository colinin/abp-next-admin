import { ExtensibleObject, ExtensibleAuditedEntity, PagedAndSortedResultRequestDto } from '../../../model/baseModel';

export interface OpenIddictApplicationDto extends ExtensibleAuditedEntity<string> {
  clientId: string;
  clientSecret?: string;
  consentType?: string;
  displayName?: string;
  displayNames?: {[key: string]: string};
  endpoints?: string[];
  grantTypes?: string[];
  responseTypes?: string[];
  scopes?: string[];
  postLogoutRedirectUris?: string[];
  properties?: {[key: string]: string};
  redirectUris?: string[];
  requirements?: string[];
  type?: string;
  clientUri?: string;
  logoUri?: string;
}

export interface OpenIddictApplicationGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface OpenIddictApplicationCreateOrUpdateDto extends ExtensibleObject {
  clientId: string;
  clientSecret?: string;
  consentType?: string;
  displayName?: string;
  displayNames?: {[key: string]: string};
  endpoints?: string[];
  grantTypes?: string[];
  responseTypes?: string[];
  scopes?: string[];
  postLogoutRedirectUris?: string[];
  properties?: {[key: string]: string};
  redirectUris?: string[];
  requirements?: string[];
  type?: string;
  clientUri?: string;
  logoUri?: string;
}

export type OpenIddictApplicationCreateDto = OpenIddictApplicationCreateOrUpdateDto;

export type OpenIddictApplicationUpdateDto = OpenIddictApplicationCreateOrUpdateDto;
