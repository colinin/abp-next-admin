import { defHttp } from '/@/utils/http/axios';
import {
  WebhookSendAttempt,
  WebhookSendAttemptGetListInput,
  WebhookSendRecordDeleteManyInput,
  WebhookSendRecordResendManyInput,
} from './model';

export const GetAsyncById = (id: string) => {
  return defHttp.get<WebhookSendAttempt>({
    url: `/api/webhooks/send-attempts/${id}`,
  });
};

export const DeleteAsyncById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/webhooks/send-attempts/${id}`,
  });
};

export const DeleteManyAsyncByInput = (input: WebhookSendRecordDeleteManyInput) => {
  return defHttp.delete<void>({
    url: `/api/webhooks/send-attempts/delete-many`,
    data: input,
  });
};

export const GetListAsyncByInput = (input: WebhookSendAttemptGetListInput) => {
  return defHttp.get<PagedResultDto<WebhookSendAttempt>>({
    url: `/api/webhooks/send-attempts`,
    params: input,
  });
};

export const ResendAsyncById = (id: string) => {
  return defHttp.post<void>({
    url: `/api/webhooks/send-attempts/${id}/resend`,
  });
};

export const ResendManyAsyncByInput = (input: WebhookSendRecordResendManyInput) => {
  return defHttp.post<void>({
    url: `/api/webhooks/send-attempts/resend-many`,
    data: input,
  });
};
