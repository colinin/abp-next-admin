export interface OpenIddictTokenGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  clientId?: string;
  authorizationId?: string;
  subject?: string;
  status?: string;
  type?: string;
  referenceId?: string;
  beginExpirationDate?: string;
  endExpirationDate?: string;
  beginCreationTime?: string;
  endCreationTime?: string;
}

export interface OpenIddictTokenDto extends ExtensibleAuditedEntityDto<string> {
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

