import type {
  ExtensibleAuditedEntityDto,
  ExtensibleObject,
  IHasConcurrencyStamp,
  NameValue,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface AIToolDefinitionRecordDto
  extends ExtensibleAuditedEntityDto<string>, IHasConcurrencyStamp {
  description?: string;
  isEnabled: boolean;
  isGlobal: boolean;
  isSystem: boolean;
  name: string;
  provider: string;
  stateCheckers?: string;
}

interface AIToolDefinitionRecordCreateOrUpdateDto extends ExtensibleObject {
  description?: string;
  isEnabled: boolean;
  isGlobal: boolean;
  provider: string;
  stateCheckers?: string;
}

interface AIToolDefinitionRecordCreateDto extends AIToolDefinitionRecordCreateOrUpdateDto {
  name: string;
}

interface AIToolDefinitionRecordUpdateDto
  extends AIToolDefinitionRecordCreateOrUpdateDto, IHasConcurrencyStamp {}

interface AIToolDefinitionRecordGetPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  isEnabled?: boolean;
  provider?: string;
}

interface AIToolPropertyDescriptorDto {
  description?: string;
  displayName: string;
  name: string;
  options: NameValue<any>[];
  required: boolean;
  valueType: string;
}

interface AIToolProviderDto {
  name: string;
  properties: AIToolPropertyDescriptorDto[];
}

export type {
  AIToolDefinitionRecordCreateDto,
  AIToolDefinitionRecordDto,
  AIToolDefinitionRecordGetPagedListInput,
  AIToolDefinitionRecordUpdateDto,
  AIToolProviderDto,
};
