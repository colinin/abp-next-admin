import { defAbpHttp } from '/@/utils/http/abp';
import { PagedResultDto } from '../model/baseModel';
import { WebhookSendAttempt, WebhookSendAttemptGetListInput } from './model/sendAttemptsModel';

const remoteServiceName = 'WebhooksManagement';
const controllerName = 'WebhookSendRecord';

export const getById = (id: string) => {
  return defAbpHttp.request<WebhookSendAttempt>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    params: {
      id: id,
    },
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.request<WebhookSendAttempt>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteAsync',
    params: {
      id: id,
    },
  });
}

export const getList = (input: WebhookSendAttemptGetListInput) => {
  return defAbpHttp.request<PagedResultDto<WebhookSendAttempt>>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    params: {
      input: input,
    },
  });
};

export const resend = (id: string) => {
  return defAbpHttp.request<void>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'ResendAsync',
    params: {
      id: id,
    },
  });
}