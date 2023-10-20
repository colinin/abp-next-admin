import { defHttp } from '/@/utils/http/axios';
import {
  WebhookGroupDefinitionDto,
  WebhookGroupDefinitionCreateDto,
  WebhookGroupDefinitionUpdateDto,
  WebhookGroupDefinitionGetListInput,
 } from './model';


export const GetAsyncByName = (name: string) => {
  return defHttp.get<WebhookGroupDefinitionDto>({
    url: `/api/webhooks/definitions/groups/${name}`,
  });
};

export const GetListAsyncByInput = (input: WebhookGroupDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<WebhookGroupDefinitionDto>>({
    url: `/api/webhooks/definitions/groups`,
    params: input,
  });
}

export const CreateAsyncByInput = (input: WebhookGroupDefinitionCreateDto) => {
  return defHttp.post<WebhookGroupDefinitionDto>({
    url: `/api/webhooks/definitions/groups`,
    data: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: WebhookGroupDefinitionUpdateDto) => {
  return defHttp.put<WebhookGroupDefinitionDto>({
    url: `/api/webhooks/definitions/groups/${name}`,
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/webhooks/definitions/groups/${name}`,
  });
};
