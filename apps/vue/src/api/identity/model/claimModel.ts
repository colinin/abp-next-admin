export enum ValueType {
  String = 0,
  Int = 1,
  Boolean = 2,
  DateTime = 3,
}

export interface IdentityClaimType extends ExtensibleEntityDto<string> {
  name: string;
  required: boolean;
  isStatic: boolean;
  regex?: string;
  regexDescription?: string;
  description?: string;
  valueType: ValueType;
}

export interface CreateOrUpdateIdentityClaimType extends ExtensibleObject {
  required: boolean;
  regex?: string;
  regexDescription?: string;
  description?: string;
}

export interface CreateIdentityClaimType extends CreateOrUpdateIdentityClaimType {
  name: string;
  isStatic: boolean;
  valueType: ValueType;
}

export interface UpdateIdentityClaimType extends CreateOrUpdateIdentityClaimType {}

export interface GetIdentityClaimTypePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface IdentityClaimTypeListResult extends ListResultDto<IdentityClaimType> {}

export interface IdentityClaimTypePagedResult extends PagedResultDto<IdentityClaimType> {}

export interface IdentityClaim {
  claimType: string;
  claimValue: string;
}

export interface DeleteIdentityClaim {
  claimType: string;
  claimValue: string;
}

export interface CreateIdentityClaim extends IdentityClaim {}

export interface UpdateIdentityClaim extends IdentityClaim {
  newClaimValue: string;
}
