import { defAbpHttp } from '/@/utils/http/abp';
import {
  WebhookSendAttempt,
  WebhookSendAttemptGetListInput,
  WebhookSendRecordDeleteManyInput,
  WebhookSendRecordResendManyInput,
} from './model';

export const GetAsyncById = (id: string) => {
  return defAbpHttp.get<WebhookSendAttempt>({
    url: `/api/webhooks/send-attempts/${id}`,
  });
};

export const DeleteAsyncById = (id: string) => {
  return defAbpHttp.delete<void>({
    url: `/api/webhooks/send-attempts/${id}`,
  });
};

export const DeleteManyAsyncByInput = (input: WebhookSendRecordDeleteManyInput) => {
  return defAbpHttp.delete<void>({
    url: `/api/webhooks/send-attempts/delete-many`,
    data: input,
  });
};

export const GetListAsyncByInput = (input: WebhookSendAttemptGetListInput) => {
  return defAbpHttp.get<PagedResultDto<WebhookSendAttempt>>({
    url: `/api/webhooks/send-attempts`,
    params: input,
  });
};

export const ResendAsyncById = (id: string) => {
  return defAbpHttp.post<void>({
    url: `/api/webhooks/send-attempts/${id}/resend`,
  });
};

export const ResendManyAsyncByInput = (input: WebhookSendRecordResendManyInput) => {
  return defAbpHttp.post<void>({
    url: `/api/webhooks/send-attempts/resend-many`,
    data: input,
  });
};
