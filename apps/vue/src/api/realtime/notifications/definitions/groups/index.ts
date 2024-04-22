import { defHttp } from '/@/utils/http/axios';
import {
  NotificationGroupDefinitionDto,
  NotificationGroupDefinitionCreateDto,
  NotificationGroupDefinitionUpdateDto,
  NotificationGroupDefinitionGetListInput,
} from './model';

export const CreateAsyncByInput = (input: NotificationGroupDefinitionCreateDto) => {
  return defHttp.post<NotificationGroupDefinitionDto>({
    url: '/api/notifications/definitions/groups',
    data: input,
  });
};

export const DeleteAsyncByName = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/notifications/definitions/groups/${name}`,
  });
};

export const GetAsyncByName = (name: string) => {
  return defHttp.get<NotificationGroupDefinitionDto>({
    url: `/api/notifications/definitions/groups/${name}`,
  });
};

export const GetListAsyncByInput = (input: NotificationGroupDefinitionGetListInput) => {
  return defHttp.get<ListResultDto<NotificationGroupDefinitionDto>>({
    url: '/api/notifications/definitions/groups',
    params: input,
  });
};

export const UpdateAsyncByNameAndInput = (name: string, input: NotificationGroupDefinitionUpdateDto) => {
  return defHttp.put<NotificationGroupDefinitionDto>({
    url: `/api/notifications/definitions/groups/${name}`,
    data: input,
  });
};
