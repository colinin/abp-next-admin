import type {
  Dictionary,
  ExtensibleAuditedEntityDto,
  ExtensibleObject,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface OpenIddictScopeCreateOrUpdateDto extends ExtensibleObject {
  description?: string;
  descriptions?: Dictionary<string, string>;
  displayName?: string;
  displayNames?: Dictionary<string, string>;
  name: string;
  properties?: Dictionary<string, string>;
  resources?: string[];
}

type OpenIddictScopeCreateDto = OpenIddictScopeCreateOrUpdateDto;

interface OpenIddictScopeGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

type OpenIddictScopeUpdateDto = OpenIddictScopeCreateOrUpdateDto;

interface OpenIddictScopeDto extends ExtensibleAuditedEntityDto<string> {
  description?: string;
  descriptions?: Dictionary<string, string>;
  displayName?: string;
  displayNames?: Dictionary<string, string>;
  name: string;
  properties?: Dictionary<string, string>;
  resources?: string[];
}

export type {
  OpenIddictScopeCreateDto,
  OpenIddictScopeDto,
  OpenIddictScopeGetListInput,
  OpenIddictScopeUpdateDto,
};
