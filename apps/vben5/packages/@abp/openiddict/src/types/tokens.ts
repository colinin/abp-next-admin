import type {
  ExtensibleAuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface OpenIddictTokenGetListInput extends PagedAndSortedResultRequestDto {
  authorizationId?: string;
  beginCreationTime?: string;
  beginExpirationDate?: string;
  clientId?: string;
  endCreationTime?: string;
  endExpirationDate?: string;
  filter?: string;
  referenceId?: string;
  status?: string;
  subject?: string;
  type?: string;
}

interface OpenIddictTokenDto extends ExtensibleAuditedEntityDto<string> {
  applicationId?: string;
  authorizationId?: string;
  creationDate?: string;
  expirationDate?: string;
  payload?: string;
  properties?: string;
  redemptionDate?: string;
  referenceId?: string;
  status?: string;
  subject?: string;
  type?: string;
}

export type { OpenIddictTokenDto, OpenIddictTokenGetListInput };
