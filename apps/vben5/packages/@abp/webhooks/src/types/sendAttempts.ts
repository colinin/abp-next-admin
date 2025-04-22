import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/core';

import { HttpStatusCode } from '@abp/request';

interface WebhookEventRecordDto extends EntityDto<string> {
  creationTime: string;
  data?: string;
  tenantId?: string;
  webhookName: string;
}

interface WebhookSendRecordDto extends EntityDto<string> {
  creationTime: string;
  lastModificationTime?: string;
  requestHeaders?: Record<string, string>;
  response?: string;
  responseHeaders?: Record<string, string>;
  responseStatusCode?: HttpStatusCode;
  sendExactSameData: boolean;
  tenantId?: string;
  webhookEvent: WebhookEventRecordDto;
  webhookEventId: string;
  webhookSubscriptionId: string;
}

interface WebhookSendRecordDeleteManyInput {
  recordIds: string[];
}

interface WebhookSendRecordResendManyInput {
  recordIds: string[];
}

interface WebhookSendRecordGetListInput extends PagedAndSortedResultRequestDto {
  beginCreationTime?: Date;
  endCreationTime?: Date;
  filter?: string;
  responseStatusCode?: HttpStatusCode;
  state?: boolean;
  subscriptionId?: string;
  tenantId?: string;
  webhookEventId?: string;
}

export type {
  WebhookEventRecordDto,
  WebhookSendRecordDeleteManyInput,
  WebhookSendRecordDto,
  WebhookSendRecordGetListInput,
  WebhookSendRecordResendManyInput,
};
