import { PagedAndSortedResultRequestDto } from '../../model/baseModel';
import { HttpStatusCode } from '/@/enums/httpEnum';

export interface WebhookEvent {
  tenantId?: string;
  webhookName: string;
  data: string;
  creationTime: Date;
}

export interface WebhookSendAttempt {
  id: string;
  tenantId?: string;
  webhookEventId: string;
  webhookSubscriptionId: string;
  response: string;
  responseStatusCode?: HttpStatusCode;
  creationTime: Date;
  lastModificationTime?: Date;
  sendExactSameData: boolean;
  requestHeaders: Record<string, string>;
  responseHeaders: Record<string, string>;
  webhookEvent: WebhookEvent;
}

export interface WebhookSendAttemptGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  webhookEventId?: string;
  subscriptionId?: string;
  responseStatusCode?: HttpStatusCode;
  beginCreationTime?: Date;
  endCreationTime?: Date;
}
