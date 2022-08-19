import { ExtensibleAuditedEntity, PagedAndSortedResultRequestDto } from '../../../model/baseModel';

export interface OpenIddictTokenDto extends ExtensibleAuditedEntity<string> {
  applicationId?: string;
  authorizationId?: string;
  creationDate?: Date;
  expirationDate?: Date;
  payload?: string;
  properties?: string;
  redemptionDate?: Date;
  referenceId?: string;
  type?: string;
  status?: string;
  subject?: string;
}

export interface OpenIddictTokenGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  clientId?: string;
  authorizationId?: string;
  subject?: string;
  status?: string;
  type?: string;
  referenceId?: string;
  beginCreationTime?: Date;
  endCreationTime?: string;
  beginExpirationDate?: Date;
  endExpirationDate?: Date;
}
