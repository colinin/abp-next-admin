import { defHttp } from '/@/utils/http/axios';
import { 
  NotificationInfo,
  NotificationGroup,
  GetNotificationPagedRequest,
  NotificationReadState,
} from './model';

export const markReadState = (
  ids: string[],
  state: NotificationReadState = NotificationReadState.Read,
) => {
  return defHttp.put<void>({
    url: '/api/notifications/my-notifilers/mark-read-state',
    data: {
      idList: ids,
      state: state,
    },
  });
};

export const deleteById = (id: string) => {
  return defHttp.delete<void>({
    url: `/api/notifications/my-notifilers/${id}`,
  });
};

export const getList = (input: GetNotificationPagedRequest) => {
  return defHttp.get<PagedResultDto<NotificationInfo>>({
    url: '/api/notifications/my-notifilers',
    params: input,
  });
};

export const getAssignableNotifiers = () => {
  return defHttp.get<ListResultDto<NotificationGroup>>({
    url: '/api/notifications/assignables',
  });
};
