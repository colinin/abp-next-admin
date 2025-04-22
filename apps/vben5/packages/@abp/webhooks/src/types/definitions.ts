import type { IHasConcurrencyStamp, IHasExtraProperties } from '@abp/core';

interface WebhookDefinitionDto extends IHasExtraProperties {
  description?: string;
  displayName: string;
  groupName: string;
  isEnabled: boolean;
  isStatic: boolean;
  name: string;
  requiredFeatures?: string[];
}

interface WebhookDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  description?: string;
  displayName: string;
  isEnabled: boolean;
  requiredFeatures?: string[];
}

interface WebhookDefinitionCreateDto
  extends WebhookDefinitionCreateOrUpdateDto {
  groupName: string;
  name: string;
}

interface WebhookDefinitionUpdateDto
  extends IHasConcurrencyStamp,
    WebhookDefinitionCreateOrUpdateDto {}

interface WebhookDefinitionGetListInput {
  filter?: string;
  groupName?: string;
}

export type {
  WebhookDefinitionCreateDto,
  WebhookDefinitionDto,
  WebhookDefinitionGetListInput,
  WebhookDefinitionUpdateDto,
};
