import { defAbpHttp } from '/@/utils/http/abp';
import {
  WebhookSubscription,
  WebhookAvailableGroup,
  CreateWebhookSubscription,
  UpdateWebhookSubscription,
  WebhookSubscriptionGetListInput,
  WebhookSubscriptionDeleteManyInput,
} from './model';

export const CreateAsyncByInput = (input: CreateWebhookSubscription) => {
  return defAbpHttp.post<WebhookSubscription>({
    url: `/api/webhooks/subscriptions`,
    data: input,
  });
};

export const UpdateAsyncByIdAndInput = (id: string, input: UpdateWebhookSubscription) => {
  return defAbpHttp.put<WebhookSubscription>({
    url: `/api/webhooks/subscriptions/${id}`,
    data: input,
  });
};

export const GetAsyncById = (id: string) => {
  return defAbpHttp.get<WebhookSubscription>({
    url: `/api/webhooks/subscriptions/${id}`,
  });
};

export const DeleteAsyncById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: `/api/webhooks/subscriptions/${id}`,
  });
};

export const DeleteManyAsyncByInput = (input: WebhookSubscriptionDeleteManyInput) => {
  return defAbpHttp.delete<void>({
    url: `/api/webhooks/subscriptions/delete-many`,
    data: input,
  });
};

export const GetListAsyncByInput = (input: WebhookSubscriptionGetListInput) => {
  return defAbpHttp.get<PagedResultDto<WebhookSubscription>>({
    url: `/api/webhooks/subscriptions`,
    params: input,
  });
};

export const GetAllAvailableWebhooksAsync = () => {
  return defAbpHttp.get<ListResultDto<WebhookAvailableGroup>>({
    url: `/api/webhooks/subscriptions/availables`,
  });
};
