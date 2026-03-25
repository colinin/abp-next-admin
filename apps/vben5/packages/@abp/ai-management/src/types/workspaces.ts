import type {
  ExtensibleAuditedEntityDto,
  ExtensibleObject,
  IHasConcurrencyStamp,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface WorkspaceDefinitionRecordDto
  extends ExtensibleAuditedEntityDto<string>, IHasConcurrencyStamp {
  apiBaseUrl?: string;
  description?: string;
  displayName: string;
  frequencyPenalty?: number;
  instructions?: string;
  isEnabled: boolean;
  maxOutputTokens?: number;
  modelName: string;
  name: string;
  presencePenalty?: number;
  provider: string;
  stateCheckers?: string;
  systemPrompt?: string;
  temperature?: number;
}

interface WorkspaceDefinitionRecordCreateOrUpdateDto extends ExtensibleObject {
  apiBaseUrl?: string;
  apiKey?: string;
  description?: string;
  displayName: string;
  frequencyPenalty?: number;
  instructions?: string;
  isEnabled: boolean;
  maxOutputTokens?: number;
  modelName: string;
  presencePenalty?: number;
  provider: string;
  stateCheckers?: string;
  systemPrompt?: string;
  temperature?: number;
}

interface WorkspaceDefinitionRecordCreateDto extends WorkspaceDefinitionRecordCreateOrUpdateDto {
  name: string;
}

type WorkspaceDefinitionRecordUpdateDto = IHasConcurrencyStamp &
  WorkspaceDefinitionRecordCreateOrUpdateDto;

interface WorkspaceDefinitionRecordGetPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  modelName?: string;
  provider?: string;
}

interface ChatClientProviderDto {
  models: string[];
  name: string;
}

export type {
  ChatClientProviderDto,
  WorkspaceDefinitionRecordCreateDto,
  WorkspaceDefinitionRecordDto,
  WorkspaceDefinitionRecordGetPagedListInput,
  WorkspaceDefinitionRecordUpdateDto,
};
