export interface OpenIddictAuthorizationGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  subject?: string;
  clientId?: string;
  status?: string;
  type?: string;
  beginCreationTime?: string;
  endCreationTime?: string;
}

export interface OpenIddictAuthorizationDto extends ExtensibleAuditedEntityDto<string> {
  applicationId?: string;
  creationDate?: string;
  properties?: Dictionary<string, string>;
  scopes?: string[];
  status?: string;
  subject?: string;
  type?: string;
}

