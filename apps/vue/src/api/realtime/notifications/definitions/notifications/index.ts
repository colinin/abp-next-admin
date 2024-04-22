import { defHttp } from '/@/utils/http/axios';
import {
  NotificationDefinitionDto,
  NotificationDefinitionCreateDto,
  NotificationDefinitionUpdateDto,
  NotificationDefinitionGetListInput,
} from './model';

export const CreateAsyncByInput = (input: NotificationDefinitionCreateDto) => {
  return defHttp.post<NotificationDefinitionDto>({
    url: '/api/notifications/definitions/notifications',
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/notifications/definitions/notifications/${name}`,
  });
};

export const GetAsyncByName = (name: string) => {
  return defHttp.get<NotificationDefinitionDto>({
    url: `/api/notifications/definitions/notifications/${name}`,
  });
};

export const GetListAsyncByInput = (input: NotificationDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<NotificationDefinitionDto>>({
    url: '/api/notifications/definitions/notifications',
    params: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: NotificationDefinitionUpdateDto) => {
  return defHttp.put<NotificationDefinitionDto>({
    url: `/api/notifications/definitions/notifications/${name}`,
    data: input,
  });
};
