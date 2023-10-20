import { defHttp } from '/@/utils/http/axios';
import {
  WebhookDefinitionDto,
  WebhookDefinitionCreateDto,
  WebhookDefinitionUpdateDto,
  WebhookDefinitionGetListInput,
 } from './model';


export const GetAsyncByName = (name: string) => {
  return defHttp.get<WebhookDefinitionDto>({
    url: `/api/webhooks/definitions/${name}`,
  });
};

export const GetListAsyncByInput = (input: WebhookDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<WebhookDefinitionDto>>({
    url: `/api/webhooks/definitions`,
    params: input,
  });
}

export const CreateAsyncByInput = (input: WebhookDefinitionCreateDto) => {
  return defHttp.post<WebhookDefinitionDto>({
    url: `/api/webhooks/definitions`,
    data: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: WebhookDefinitionUpdateDto) => {
  return defHttp.put<WebhookDefinitionDto>({
    url: `/api/webhooks/definitions/${name}`,
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/webhooks/definitions/${name}`,
  });
};
