import type { NotificationOptions, ToastOptions } from '@abp/core';

import { NOTIFICATION_EVENT, TOAST_EVENT, useEventBus } from '@abp/core';
import { message, notification } from 'ant-design-vue';

const NOTIFY_TYPE_MAP = {
  success: 'success',
  info: 'info',
  warning: 'warning',
  error: 'error',
} as const;

type AntdNotifyType = keyof typeof NOTIFY_TYPE_MAP;

const MSG_TYPE_MAP = {
  success: 'success',
  info: 'info',
  warning: 'warning',
  error: 'error',
} as const;

type AntdMsgType = keyof typeof MSG_TYPE_MAP;

const PLACEMENT_MAP = {
  top: 'top',
  'top-left': 'topLeft',
  'top-right': 'topRight',
  bottom: 'bottom',
  'bottom-left': 'bottomLeft',
  'bottom-right': 'bottomRight',
  center: 'top',
} as const;

function isNonEmptyString(v: unknown): v is string {
  return typeof v === 'string' && v.trim().length > 0;
}

export function setupFeedbackBridge() {
  const { subscribe, unSubscribe } = useEventBus();

  const openedNotifyKeys = new Set<string>();

  const notifyHandler = (payload?: NotificationOptions) => {
    if (!payload || !isNonEmptyString(payload.title)) return;

    const type = NOTIFY_TYPE_MAP[payload.type as AntdNotifyType] ?? 'info';
    const durationMs =
      typeof payload.duration === 'number' ? payload.duration : 4500;

    const duration = durationMs > 0 ? durationMs / 1000 : null;

    const key =
      (payload.data?.key as string | undefined) ??
      `notify-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`;
    openedNotifyKeys.add(key);

    const placement =
      PLACEMENT_MAP[
        (payload.data?.position as keyof typeof PLACEMENT_MAP) ?? 'top-right'
      ];

    notification[type]({
      key,
      message: payload.title,
      description: payload.content,
      duration,
      placement,
      ...((payload.closable ?? true) ? {} : { closeIcon: null }),
      onClick: payload.data?.onClick,
      onClose: () => {
        openedNotifyKeys.delete(key);
        payload.data?.onClose?.();
      },
    });
  };

  let closeLoading: (() => void) | null = null;

  const toastHandler = (payload?: ToastOptions) => {
    if (!payload || !isNonEmptyString(payload.message)) return;

    const durationMs =
      typeof payload.duration === 'number' ? payload.duration : 3000;
    const duration = durationMs > 0 ? durationMs / 1000 : 0;

    if (payload.type === 'loading') {
      const instance = message.open({
        type: 'loading',
        content: payload.message,
        duration,
        key: payload.data?.key,
      });
      closeLoading = () => instance();
      return;
    }

    const type = MSG_TYPE_MAP[payload.type as AntdMsgType] ?? 'info';

    message[type]({
      content: payload.message,
      duration,
      key: payload.data?.key,
      onClick: payload.data?.onClick,
      onClose: payload.data?.onClose,
    });
  };

  subscribe(NOTIFICATION_EVENT, notifyHandler);
  subscribe(TOAST_EVENT, toastHandler);

  let disposed = false;
  return () => {
    if (disposed) return;
    disposed = true;

    unSubscribe(NOTIFICATION_EVENT, notifyHandler);
    unSubscribe(TOAST_EVENT, toastHandler);

    closeLoading?.();
    closeLoading = null;

    openedNotifyKeys.forEach((key) => notification.close(key));
    openedNotifyKeys.clear();
  };
}
