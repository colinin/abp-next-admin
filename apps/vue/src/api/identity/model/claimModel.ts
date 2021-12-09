import {
  ExtensibleEntity,
  ExtensibleObject,
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';

export enum ValueType {
  String = 0,
  Int = 1,
  Boolean = 2,
  DateTime = 3,
}

export class IdentityClaimType extends ExtensibleEntity<string> {
  name!: string;
  required!: boolean;
  isStatic!: boolean;
  regex?: string;
  regexDescription?: string;
  description?: string;
  valueType!: ValueType;
}

export class CreateOrUpdateIdentityClaimType extends ExtensibleObject {
  required = false;
  regex?: string = '';
  regexDescription?: string = '';
  description?: string = '';
}

export class CreateIdentityClaimType extends CreateOrUpdateIdentityClaimType {
  name!: string;
  isStatic!: boolean;
  valueType!: ValueType;
}

export class UpdateIdentityClaimType extends CreateOrUpdateIdentityClaimType {}

export class GetIdentityClaimTypePagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}

export class IdentityClaimTypeListResult extends ListResultDto<IdentityClaimType> {}

export class IdentityClaimTypePagedResult extends PagedResultDto<IdentityClaimType> {}
