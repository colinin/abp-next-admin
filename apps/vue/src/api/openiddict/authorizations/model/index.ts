import { ExtensibleAuditedEntity, PagedAndSortedResultRequestDto } from '../../../model/baseModel';

export interface OpenIddictAuthorizationDto extends ExtensibleAuditedEntity<string> {
  applicationId?: string;
  creationDate?: Date;
  properties?: {[key: string]: string};
  scopes?: string[];
  type?: string;
  status?: string;
  subject?: string;
}

export interface OpenIddictAuthorizationGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  subject?: string;
  clientId?: string;
  status?: string;
  type?: string;
  beginCreationTime?: Date;
  endCreationTime?: string;
}
