import type {
  Dictionary,
  ExtensibleAuditedEntityDto,
  ExtensibleObject,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface OpenIddictApplicationGetListInput
  extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface OpenIddictApplicationCreateOrUpdateDto extends ExtensibleObject {
  applicationType?: string;
  clientId: string;
  clientSecret?: string;
  clientType?: string;
  clientUri?: string;
  consentType?: string;
  displayName?: string;
  displayNames?: Dictionary<string, string>;
  endpoints?: string[];
  grantTypes?: string[];
  logoUri?: string;
  postLogoutRedirectUris?: string[];
  properties?: Dictionary<string, string>;
  redirectUris?: string[];
  requirements?: string[];
  responseTypes?: string[];
  scopes?: string[];
}

type OpenIddictApplicationCreateDto = OpenIddictApplicationCreateOrUpdateDto;

type OpenIddictApplicationUpdateDto = OpenIddictApplicationCreateOrUpdateDto;

interface OpenIddictApplicationDto extends ExtensibleAuditedEntityDto<string> {
  applicationType?: string;
  clientId: string;
  clientSecret?: string;
  clientType?: string;
  clientUri?: string;
  consentType?: string;
  displayName?: string;
  displayNames?: Dictionary<string, string>;
  endpoints?: string[];
  grantTypes?: string[];
  logoUri?: string;
  postLogoutRedirectUris?: string[];
  properties?: Dictionary<string, string>;
  redirectUris?: string[];
  requirements?: string[];
  responseTypes?: string[];
  scopes?: string[];
}

export type {
  OpenIddictApplicationCreateDto,
  OpenIddictApplicationDto,
  OpenIddictApplicationGetListInput,
  OpenIddictApplicationUpdateDto,
};
