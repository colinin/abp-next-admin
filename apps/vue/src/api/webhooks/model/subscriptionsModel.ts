import { CreationAuditedEntityDto, PagedAndSortedResultRequestDto } from '../../model/baseModel';

export interface WebhookSubscription extends CreationAuditedEntityDto {
  id: string;
  tenantId?: string;
  webhookUri: string;
  secret: string;
  isActive: boolean;
  webhooks: string[];
  headers: { [key: string]: string };
}

export interface WebhookSubscriptionCreateOrUpdate {
  webhookUri: string;
  secret: string;
  isActive: boolean;
  webhooks: string[];
  headers: { [key: string]: string };
}

export type CreateWebhookSubscription = WebhookSubscriptionCreateOrUpdate;

export type UpdateWebhookSubscription = WebhookSubscriptionCreateOrUpdate;

export interface WebhookAvailable {
  name: string;
  displayName: string;
  description: string;
}

export interface WebhookAvailableGroup {
  name: string;
  displayName: string;
  webhooks: WebhookAvailable[];
}

export interface WebhookSubscriptionGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  tenantId?: string;
  webhookUri?: string;
  secret?: string;
  isActive?: boolean;
  webhooks?: string;
  beginCreationTime?: Date;
  endCreationTime?: Date;
}
