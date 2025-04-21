import type { IHasConcurrencyStamp, IHasExtraProperties } from '@abp/core';

interface WebhookGroupDefinitionDto extends IHasExtraProperties {
  displayName: string;
  isStatic: boolean;
  name: string;
}

interface WebhookGroupDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
}

interface WebhookGroupDefinitionCreateDto
  extends WebhookGroupDefinitionCreateOrUpdateDto {
  name: string;
}

interface WebhookGroupDefinitionUpdateDto
  extends IHasConcurrencyStamp,
    WebhookGroupDefinitionCreateOrUpdateDto {}

interface WebhookGroupDefinitionGetListInput {
  filter?: string;
}

export type {
  WebhookGroupDefinitionCreateDto,
  WebhookGroupDefinitionDto,
  WebhookGroupDefinitionGetListInput,
  WebhookGroupDefinitionUpdateDto,
};
