import { onMounted, onUnmounted, ref } from 'vue';
import { notification } from 'ant-design-vue';
import { getList } from '/@/api/messages/notifications';
import {
  NotificationInfo,
  NotificationSeverity,
  NotificationReadState,
} from '/@/api/messages/model/notificationsModel';
import { formatToDateTime } from '/@/utils/dateUtil';
import { TabItem, ListItem } from './data';
import { formatPagedRequest } from '/@/utils/http/abp/helper';
import { NotifyEventEnum } from '/@/enums/imEnum';
import { useSignalR } from '/@/hooks/web/useSignalR';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import errorAvatar from '/@/assets/icons/64x64/color-error.png';
import warningAvatar from '/@/assets/icons/64x64/color-warning.png';
import infoAvatar from '/@/assets/icons/64x64/color-info.png';
import successAvatar from '/@/assets/icons/64x64/color-success.png';

import emitter from '/@/utils/eventBus';

export function useNotifications() {
  const notifierRef = ref<TabItem>({
    key: '1',
    name: '通知',
    list: [],
  });
  const signalR = useSignalR({
    serverUrl: '/signalr-hubs/signalr-hubs/notifications',
  });

  onMounted(() => {
    emitter.on(NotifyEventEnum.NOTIFICATIONS_READ, readNotifer);
    signalR.on('get-notification', (data) => onNotifyReceived(data, true));
    signalR.onStart(refreshNotifer);
    signalR.start();
  });

  onUnmounted(() => {
    emitter.off(NotifyEventEnum.NOTIFICATIONS_READ, readNotifer);
    signalR.stop();
  });

  function onNotifyReceived(notificationInfo: NotificationInfo, notifer?: boolean) {
    const { data } = notificationInfo;
    let title = data.extraProperties.title;
    let message = data.extraProperties.message;
    let description = data.extraProperties.description;
    if (!data.extraProperties) {
      return;
    }
    if (data.extraProperties.L === true) {
      // TODO: 后端统一序列化格式
      const { L } = useLocalization(
        data.extraProperties.title.resourceName ?? data.extraProperties.title.ResourceName,
        data.extraProperties.message.resourceName ?? data.extraProperties.message.ResourceName,
        data.extraProperties.description?.resourceName ?? data.extraProperties.description?.ResourceName ?? "AbpUi");
      title = L(
        data.extraProperties.title.name ?? data.extraProperties.title.Name,
        data.extraProperties.title.values ?? data.extraProperties.title.Values,
      );
      message = L(
        data.extraProperties.message.name ?? data.extraProperties.message.Name,
        data.extraProperties.message.values ?? data.extraProperties.message.Values,
      );
      if (description) {
        description = L(
          data.extraProperties.description.name ?? data.extraProperties.description.Name,
          data.extraProperties.description.values ?? data.extraProperties.description.Values,
        );
      }
    }
    const notifier: ListItem = {
      id: notificationInfo.id,
      avatar: data.extraProperties.avatar,
      title: title,
      description: message,
      extra: description,
      datetime: formatToDateTime(notificationInfo.creationTime, 'YYYY-MM-DD HH:mm:ss'),
      type: String(notificationInfo.type),
    };
    switch (notificationInfo.severity) {
      case NotificationSeverity.Error:
      case NotificationSeverity.Fatal:
        notifier.color = 'red';
        notifier.avatar = errorAvatar;
        if (notifer) {
          notification['error']({
            message: notifier.title,
            description: notifier.description,
          });
        }
        break;
      case NotificationSeverity.Warn:
        notifier.color = 'gold';
        notifier.avatar = warningAvatar;
        if (notifer) {
          notification['warning']({
            message: notifier.title,
            description: notifier.description,
          });
        }
        break;
      case NotificationSeverity.Info:
        notifier.color = 'gold';
        notifier.avatar = infoAvatar;
        if (notifer) {
          notification['info']({
            message: notifier.title,
            description: notifier.description,
          });
        }
        break;
      case NotificationSeverity.Success:
        notifier.color = 'green';
        notifier.avatar = successAvatar;
        if (notifer) {
          notification['success']({
            message: notifier.title,
            description: notifier.description,
          });
        }
        break;
    }
    emitter.emit(NotifyEventEnum.NOTIFICATIONS_RECEVIED, notificationInfo);
    notifierRef.value.list.push(notifier);
  }

  function refreshNotifer(page = 1, pageSize = 10) {
    const request = {
      skipCount: page,
      maxResultCount: pageSize,
    };
    formatPagedRequest(request);
    getList({
      skipCount: request.skipCount,
      maxResultCount: request.maxResultCount,
      reverse: false,
      sorting: '',
      readState: NotificationReadState.UnRead,
    }).then((res) => {
      res.items.map((notifier) => {
        onNotifyReceived(notifier);
      });
    });
  }

  function readNotifer(notifier: ListItem) {
    signalR.invoke('change-state', notifier.id, NotificationReadState.Read).then(() => {
      notifierRef.value.list = [];
      refreshNotifer();
    });
  }

  return {
    notifierRef,
    readNotifer,
    refreshNotifer,
  };
}
