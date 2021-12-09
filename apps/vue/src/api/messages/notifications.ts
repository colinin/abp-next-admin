import { defHttp } from '/@/utils/http/axios';
import {
  GetNotificationPagedRequest,
  NotificationPagedResult,
  NotificationGroupListResult,
} from './model/notificationsModel';

enum Api {
  GetList = '/api/my-notifilers',
  GetAssignableNotifiers = '/api/my-notifilers/assignables',
}

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
