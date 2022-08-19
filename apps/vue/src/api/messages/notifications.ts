import { defHttp } from '/@/utils/http/axios';
import {
  GetNotificationPagedRequest,
  NotificationPagedResult,
  NotificationGroupListResult,
  NotificationReadState,
} from './model/notificationsModel';
import { format } from '/@/utils/strings';

enum Api {
  GetById = '/api/my-notifilers/{id}',
  GetList = '/api/my-notifilers',
  GetAssignableNotifiers = '/api/my-notifilers/assignables',
  Read = '/api/my-notifilers/{id}/read',
  MarkReadState = '/api/my-notifilers/mark-read-state',
}

export const markReadState = (ids: string[], state: NotificationReadState = NotificationReadState.Read) => {
  return defHttp.put<void>({
    url: Api.MarkReadState,
    data: {
      idList: ids,
      state: state,
    },
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: format(Api.GetById, { id: id }),
  });
};

export const getList = (input: GetNotificationPagedRequest) => {
  return defHttp.get<NotificationPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const getAssignableNotifiers = () => {
  return defHttp.get<NotificationGroupListResult>({
    url: Api.GetAssignableNotifiers,
  });
};
