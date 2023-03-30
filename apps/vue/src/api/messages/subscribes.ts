import { defHttp } from '/@/utils/http/axios';
import { UserSubscreNotificationListResult } from './model/subscribesModel';
import { format } from '/@/utils/strings';

enum Api {
  GetAll = '/api/notifications/my-subscribes/all',
  Subscribe = '/api/notifications/my-subscribes',
  UnSubscribe = '/api/notifications/my-subscribes?name={name}',
}

export const getAll = () => {
  return defHttp.get<UserSubscreNotificationListResult>({
    url: Api.GetAll,
  });
};

export const subscribe = (name: string) => {
  return defHttp.post<void>({
    url: Api.Subscribe,
    data: {
      name: name,
    },
  });
};

export const unSubscribe = (name: string) => {
  return defHttp.delete<void>({
    url: format(Api.UnSubscribe, { name: name }),
  });
};
