import type { Notification, NotificationInfo } from '../types/notifications';

import { Events, useEventBus } from '@abp/core';
import { useSignalR } from '@abp/signalr';
import { notification } from 'ant-design-vue';

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
    stop();
  }

  function _init() {
    onStart(() => on(Events.GetNotification, _onNotifyReceived));
    init({
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
    let message = notifier.description;
    switch (notifier.contentType) {
      case NotificationContentType.Html:
      case NotificationContentType.Json:
      case NotificationContentType.Markdown: {
        message = notifier.title;
        break;
      }
      case NotificationContentType.Text: {
        message = notifier.description;
      }
    }
    switch (notifier.severity) {
      case NotificationSeverity.Error:
      case NotificationSeverity.Fatal: {
        notification.error({
          description: message,
          message: notifier.title,
        });
        break;
      }
      case NotificationSeverity.Info: {
        notification.info({
          description: message,
          message: notifier.title,
        });
        break;
      }
      case NotificationSeverity.Success: {
        notification.success({
          description: message,
          message: notifier.title,
        });
        break;
      }
      case NotificationSeverity.Warn: {
        notification.warning({
          description: message,
          message: notifier.title,
        });
        break;
      }
    }
  }

  return {
    register,
    release,
  };
}
