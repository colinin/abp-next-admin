import { defHttp } from '/@/utils/http/axios';
import { UserSubscreNotificationListResult } from './model';

export const getAll = () => {
  return defHttp.get<UserSubscreNotificationListResult>({
    url: '/api/notifications/my-subscribes/all',
  });
};

export const subscribe = (name: string) => {
  return defHttp.post<void>({
    url: '/api/notifications/my-subscribes',
    data: {
      name: name,
    },
  });
};

export const unSubscribe = (name: string) => {
  return defHttp.delete<void>({
    url: `/api/notifications/my-subscribes?name=${name}`,
  });
};
