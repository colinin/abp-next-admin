import { onMounted, onUnmounted, ref } from 'vue';
import { useI18n } from '/@/hooks/web/useI18n';
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
import errorAvatar from '/@/assets/icons/64x64/color-error.png';
import warningAvatar from '/@/assets/icons/64x64/color-warning.png';
import infoAvatar from '/@/assets/icons/64x64/color-info.png';
import successAvatar from '/@/assets/icons/64x64/color-success.png';

import emitter from '/@/utils/eventBus';

export function useNotifications() {
  const { t } = useI18n();
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
    signalR.on('get-notification', onNotifyReceived);
    signalR.onStart(refreshNotifer);
    signalR.start();
  });

  onUnmounted(() => {
    emitter.off(NotifyEventEnum.NOTIFICATIONS_READ, readNotifer);
    signalR.stop();
  });

  function onNotifyReceived(notificationInfo: NotificationInfo) {
    const { data } = notificationInfo;
    if (!data.extraProperties) {
      return;
    }
    if (data.extraProperties.L === true) {
      data.extraProperties.title = t(
        data.extraProperties.title.ResourceName + '.' + data.extraProperties.title.Name,
        data.extraProperties.message.Values as Recordable,
      );
      data.extraProperties.message = t(
        data.extraProperties.message.ResourceName + '.' + data.extraProperties.message.Name,
        data.extraProperties.message.Values as Recordable,
      );
      if (data.extraProperties.description) {
        data.extraProperties.description = t(
          data.extraProperties.description.ResourceName +
            '.' +
            data.extraProperties.description.Name,
          data.extraProperties.message.Values as Recordable,
        );
      }
    }
    const notifier: ListItem = {
      id: notificationInfo.id,
      avatar: data.extraProperties.avatar,
      title: data.extraProperties.title,
      description: data.extraProperties.message,
      extra: data.extraProperties.description,
      datetime: formatToDateTime(notificationInfo.creationTime, 'YYYY-MM-DD HH:mm:ss'),
      type: String(notificationInfo.type),
    };
    switch (notificationInfo.severity) {
      case NotificationSeverity.Error:
      case NotificationSeverity.Fatal:
        notifier.color = 'red';
        notifier.avatar = errorAvatar;
        break;
      case NotificationSeverity.Warn:
        notifier.color = 'gold';
        notifier.avatar = warningAvatar;
        break;
      case NotificationSeverity.Info:
        notifier.color = 'gold';
        notifier.avatar = infoAvatar;
        break;
      case NotificationSeverity.Success:
        notifier.color = 'green';
        notifier.avatar = successAvatar;
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
