import type {
  ExtensibleEntityDto,
  ExtensibleObject,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

export enum ValueType {
  Boolean = 2,
  DateTime = 3,
  Int = 1,
  String = 0,
}

interface IdentityClaimTypeDto extends ExtensibleEntityDto<string> {
  description?: string;
  isStatic: boolean;
  name: string;
  regex?: string;
  regexDescription?: string;
  required: boolean;
  valueType: ValueType;
}

interface IdentityClaimTypeCreateOrUpdateDto extends ExtensibleObject {
  description?: string;
  regex?: string;
  regexDescription?: string;
  required: boolean;
}

interface IdentityClaimTypeCreateDto
  extends IdentityClaimTypeCreateOrUpdateDto {
  isStatic: boolean;
  name: string;
  valueType: ValueType;
}

type IdentityClaimTypeUpdateDto = IdentityClaimTypeCreateOrUpdateDto;

interface GetIdentityClaimTypePagedListInput
  extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export type {
  GetIdentityClaimTypePagedListInput,
  IdentityClaimTypeCreateDto,
  IdentityClaimTypeDto,
  IdentityClaimTypeUpdateDto,
};
