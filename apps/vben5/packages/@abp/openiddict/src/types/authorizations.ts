import type {
  Dictionary,
  ExtensibleAuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface OpenIddictAuthorizationDto
  extends ExtensibleAuditedEntityDto<string> {
  applicationId?: string;
  creationDate?: string;
  properties?: Dictionary<string, string>;
  scopes?: string[];
  status?: string;
  subject?: string;
  type?: string;
}

interface OpenIddictAuthorizationGetListInput
  extends PagedAndSortedResultRequestDto {
  beginCreationTime?: string;
  clientId?: string;
  endCreationTime?: string;
  filter?: string;
  status?: string;
  subject?: string;
  type?: string;
}

export type { OpenIddictAuthorizationDto, OpenIddictAuthorizationGetListInput };
