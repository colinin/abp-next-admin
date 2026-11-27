import type { Notification, NotificationInfo } from '../types/notifications';

import {
  Events,
  useNotifications as useAppNotifications,
  useEventBus,
} from '@abp/core';
import { useSignalR } from '@abp/signalr';

import {
  NotificationContentType,
  NotificationSeverity,
  NotificationType,
} from '../types/notifications';
import { useNotificationSerializer } from './useNotificationSerializer';

export function useNotifications() {
  const { deserialize } = useNotificationSerializer();
  const { publish, subscribe, unSubscribe } = useEventBus();
  const { init, off, on, onStart, stop } = useSignalR();

  /** 注册通知 */
  function register() {
    _registerEvents();
    _init();
  }

  /** 释放通知 */
  function release() {
    _releaseEvents();
    void stop();
  }

  function _init() {
    onStart(() => on(Events.GetNotification, _onNotifyReceived));
    void init({
      autoStart: true,
      serverUrl: '/signalr-hubs/notifications',
      useAccessToken: true,
    });
  }

  /** 注册通知事件 */
  function _registerEvents() {
    subscribe(Events.UserLogout, stop);
  }

  /** 释放通知事件 */
  function _releaseEvents() {
    unSubscribe(Events.UserLogout, stop);
    off(Events.GetNotification, _onNotifyReceived);
  }

  /** 接收通知回调 */
  function _onNotifyReceived(notificationInfo: NotificationInfo) {
    const notification = deserialize(notificationInfo);
    if (notification.type === NotificationType.ServiceCallback) {
      publish(notification.name, notification);
      return;
    }
    publish(Events.NotificationRecevied, notification);
    _notification(notification);
  }

  /** 通知推送 */
  function _notification(notifier: Notification) {
    let message = notifier.message;
    switch (notifier.contentType) {
      case NotificationContentType.Text: {
        if (notifier.description) {
          message = notifier.description;
        }
      }
    }
    const notify = useAppNotifications();
    switch (notifier.severity) {
      case NotificationSeverity.Error:
      case NotificationSeverity.Fatal: {
        notify.notifyError({ content: message, title: notifier.title });
        break;
      }
      case NotificationSeverity.Info: {
        notify.notifyInfo({ content: message, title: notifier.title });
        break;
      }
      case NotificationSeverity.Success: {
        notify.notifySuccess({ content: message, title: notifier.title });
        break;
      }
      case NotificationSeverity.Warn: {
        notify.notifyWarning({ content: message, title: notifier.title });
        break;
      }
    }
  }

  return {
    register,
    release,
  };
}
