import type {
  CreationAuditedEntityDto,
  IHasConcurrencyStamp,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface WebhookSubscriptionDto
  extends CreationAuditedEntityDto<string>,
    IHasConcurrencyStamp {
  description?: string;
  headers?: Record<string, string>;
  isActive: boolean;
  secret?: string;
  tenantId?: string;
  timeoutDuration?: number;
  webhooks: string[];
  webhookUri: string;
}

interface WebhookSubscriptionCreateOrUpdateDto {
  description?: string;
  headers?: Record<string, string>;
  isActive: boolean;
  secret?: string;
  tenantId?: string;
  timeoutDuration?: number;
  webhooks: string[];
  webhookUri: string;
}

type WebhookSubscriptionCreateDto = WebhookSubscriptionCreateOrUpdateDto;

interface WebhookSubscriptionUpdateDto
  extends IHasConcurrencyStamp,
    WebhookSubscriptionCreateOrUpdateDto {}

interface WebhookSubscriptionDeleteManyInput {
  recordIds: string[];
}

interface WebhookSubscriptionGetListInput
  extends PagedAndSortedResultRequestDto {
  beginCreationTime?: Date;
  endCreationTime?: Date;
  filter?: string;
  isActive?: boolean;
  secret?: string;
  tenantId?: string;
  webhooks?: string;
  webhookUri?: string;
}

interface WebhookAvailableDto {
  description?: string;
  displayName: string;
  name: string;
}

interface WebhookAvailableGroupDto {
  displayName: string;
  name: string;
  webhooks: WebhookAvailableDto[];
}

export type {
  WebhookAvailableDto,
  WebhookAvailableGroupDto,
  WebhookSubscriptionCreateDto,
  WebhookSubscriptionDeleteManyInput,
  WebhookSubscriptionDto,
  WebhookSubscriptionGetListInput,
  WebhookSubscriptionUpdateDto,
};
