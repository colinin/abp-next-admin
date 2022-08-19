import { ExtensibleObject, ExtensibleAuditedEntity, PagedAndSortedResultRequestDto } from '../../../model/baseModel';

export interface OpenIddictScopeDto extends ExtensibleAuditedEntity<string> {
  name: string;
  displayName?: string;
  displayNames?: {[key: string]: string};
  description?: string;
  descriptions?: {[key: string]: string};
  properties?: {[key: string]: string};
  resources?: {[key: string]: string};
}

export interface OpenIddictScopeGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface OpenIddictScopeCreateOrUpdateDto extends ExtensibleObject {
  name: string;
  displayName?: string;
  displayNames?: {[key: string]: string};
  description?: string;
  descriptions?: {[key: string]: string};
  properties?: {[key: string]: string};
  resources?: {[key: string]: string};
}

export type OpenIddictScopeCreateDto = OpenIddictScopeCreateOrUpdateDto;

export type OpenIddictScopeUpdateDto = OpenIddictScopeCreateOrUpdateDto;
