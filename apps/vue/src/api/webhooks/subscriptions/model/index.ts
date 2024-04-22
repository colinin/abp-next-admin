export interface WebhookSubscription extends CreationAuditedEntityDto<string>, IHasConcurrencyStamp {
  tenantId?: string;
  webhookUri: string;
  description?: string;
  secret: string;
  isActive: boolean;
  webhooks: string[];
  headers: Dictionary<string, string>;
}

export interface WebhookSubscriptionCreateOrUpdate {
  webhookUri: string;
  description?: string;
  secret: string;
  isActive: boolean;
  webhooks: string[];
  headers: Dictionary<string, string>;
}

export type CreateWebhookSubscription = WebhookSubscriptionCreateOrUpdate;

export interface UpdateWebhookSubscription extends WebhookSubscriptionCreateOrUpdate , IHasConcurrencyStamp {};

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

export interface WebhookSubscriptionDeleteManyInput {
  recordIds: string[];
}
