import { defHttp } from '/@/utils/http/axios';
import { UserSubscreNotification } from './model';

export const getAll = () => {
  return defHttp.get<ListResultDto<UserSubscreNotification>>({
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
