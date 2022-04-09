import { defAbpHttp } from '/@/utils/http/abp';
import {
  WebhookSubscription,
  WebhookAvailableGroup,
  CreateWebhookSubscription,
  UpdateWebhookSubscription,
  WebhookSubscriptionGetListInput,
} from './model/subscriptionsModel';
import { ListResultDto, PagedResultDto } from '../model/baseModel';

const remoteServiceName = 'WebhooksManagement';
const controllerName = 'WebhookSubscription';

export const create = (input: CreateWebhookSubscription) => {
  return defAbpHttp.request<WebhookSubscription>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'CreateAsync',
    data: input,
  });
};

export const update = (id: string, input: UpdateWebhookSubscription) => {
  return defAbpHttp.request<WebhookSubscription>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'UpdateAsync',
    data: input,
    params: {
      id: id,
    },
  });
};

export const getById = (id: string) => {
  return defAbpHttp.request<WebhookSubscription>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAsync',
    params: {
      id: id,
    },
  });
};

export const deleteById = (id: string) => {
  return defAbpHttp.request<WebhookSubscription>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'DeleteAsync',
    params: {
      id: id,
    },
  });
};

export const getList = (input: WebhookSubscriptionGetListInput) => {
  return defAbpHttp.request<PagedResultDto<WebhookSubscription>>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetListAsync',
    params: {
      input: input,
    },
  });
};

export const getAllAvailableWebhooks = () => {
  return defAbpHttp.request<ListResultDto<WebhookAvailableGroup>>({
    service: remoteServiceName,
    controller: controllerName,
    action: 'GetAllAvailableWebhooksAsync',
  });
}; 
