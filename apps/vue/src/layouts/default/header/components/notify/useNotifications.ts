import { onMounted, onUnmounted, ref } from 'vue';
import { notification } from 'ant-design-vue';
import { getList } from '/@/api/messages/notifications';
import {
  NotificationType,
  NotificationInfo,
  NotificationSeverity,
  NotificationReadState,
  NotificationContentType,
} from '/@/api/messages/notifications/model';
import { formatToDateTime } from '/@/utils/dateUtil';
import { TabItem, ListItem as Notification } from './data';
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
    if (notificationInfo.type === NotificationType.ServiceCallback) {
      emitter.emit(NotifyEventEnum.NOTIFICATIONS_SERVICE_CALLBACK, notificationInfo);
      return;
    } 
    const { data } = notificationInfo;
    let title = data.extraProperties.title;
    let message = data.extraProperties.message;
    let description = data.extraProperties.description;
    if (!data.extraProperties) {
      return;
    }
    if (data.extraProperties.L === true || data.extraProperties.L === 'true') {
      // TODO: 后端统一序列化格式
      const { L } = useLocalization(
        [data.extraProperties.title.resourceName ?? data.extraProperties.title.ResourceName,
        data.extraProperties.message.resourceName ?? data.extraProperties.message.ResourceName,
        data.extraProperties.description?.resourceName ?? data.extraProperties.description?.ResourceName ?? "AbpUi"]);
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
    const notifier: Notification = {
      id: notificationInfo.id,
      avatar: data.extraProperties.avatar,
      title: title,
      description: message,
      extra: description,
      datetime: formatToDateTime(notificationInfo.creationTime, 'YYYY-MM-DD HH:mm:ss'),
      type: String(notificationInfo.type),
      contentType: notificationInfo.contentType,
    };

    notifer && _notification(notifier, notificationInfo.severity);
    emitter.emit(NotifyEventEnum.NOTIFICATIONS_RECEVIED, notificationInfo);
    notifierRef.value.list.push(notifier);
  }

  
  function _notification(notifier: Notification, severity: NotificationSeverity) {
    let message = notifier.description;
    switch (notifier.contentType) {
      default:
      case NotificationContentType.Text:
        message = notifier.description;
      case NotificationContentType.Html:
      case NotificationContentType.Json:
      case NotificationContentType.Markdown:
        message = notifier.title;
    }
    switch (severity) {
      case NotificationSeverity.Error:
      case NotificationSeverity.Fatal:
        notifier.color = 'red';
        notifier.avatar = errorAvatar;
        notification['error']({
          message: notifier.title,
          description: message,
        });
        break;
      case NotificationSeverity.Warn:
        notifier.color = 'gold';
        notifier.avatar = warningAvatar;
        notification['warning']({
          message: notifier.title,
          description: message,
        });
        break;
      case NotificationSeverity.Info:
        notifier.color = 'gold';
        notifier.avatar = infoAvatar;
        notification['info']({
          message: notifier.title,
          description: message,
        });
        break;
      case NotificationSeverity.Success:
        notifier.color = 'green';
        notifier.avatar = successAvatar;
        notification['success']({
          message: notifier.title,
          description: message,
        });
        break;
    }
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

  function readNotifer(notifier: Notification) {
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
