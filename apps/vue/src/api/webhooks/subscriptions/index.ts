import { defHttp } from '/@/utils/http/axios';
import {
  WebhookSubscription,
  WebhookAvailableGroup,
  CreateWebhookSubscription,
  UpdateWebhookSubscription,
  WebhookSubscriptionGetListInput,
  WebhookSubscriptionDeleteManyInput,
} from './model';

export const CreateAsyncByInput = (input: CreateWebhookSubscription) => {
  return defHttp.post<WebhookSubscription>({
    url: `/api/webhooks/subscriptions`,
    data: input,
  });
};

export const UpdateAsyncByIdAndInput = (id: string, input: UpdateWebhookSubscription) => {
  return defHttp.put<WebhookSubscription>({
    url: `/api/webhooks/subscriptions/${id}`,
    data: input,
  });
};

export const GetAsyncById = (id: string) => {
  return defHttp.get<WebhookSubscription>({
    url: `/api/webhooks/subscriptions/${id}`,
  });
};

export const DeleteAsyncById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/webhooks/subscriptions/${id}`,
  });
};

export const DeleteManyAsyncByInput = (input: WebhookSubscriptionDeleteManyInput) => {
  return defHttp.delete<void>({
    url: `/api/webhooks/subscriptions/delete-many`,
    data: input,
  });
};

export const GetListAsyncByInput = (input: WebhookSubscriptionGetListInput) => {
  return defHttp.get<PagedResultDto<WebhookSubscription>>({
    url: `/api/webhooks/subscriptions`,
    params: input,
  });
};

export const GetAllAvailableWebhooksAsync = () => {
  return defHttp.get<ListResultDto<WebhookAvailableGroup>>({
    url: `/api/webhooks/subscriptions/availables`,
  });
};
