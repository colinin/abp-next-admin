import type { NotificationOptions } from '../types/notifications';
import type { ToastOptions } from '../types/toast';

import { NOTIFICATION_EVENT } from '../types/notifications';
import { TOAST_EVENT } from '../types/toast';
import { useEventBus } from './useEventBus';

type NotifyInput = Omit<NotificationOptions, 'type'> | string;
type ToastInput = Omit<ToastOptions, 'type'> | string;

export function useNotifications() {
  const { publish } = useEventBus();

  function normalizeNotify(
    input: NotifyInput,
    type: NotificationOptions['type'],
  ): NotificationOptions {
    if (typeof input === 'string') {
      return { title: input, type };
    }
    return { ...input, type };
  }

  function notify(options: NotificationOptions) {
    publish(NOTIFICATION_EVENT, {
      type: 'info',
      duration: 4500,
      closable: true,
      ...options,
    });
  }

  function normalizeToast(
    input: ToastInput,
    type: ToastOptions['type'],
  ): ToastOptions {
    if (typeof input === 'string') {
      return { message: input, type };
    }
    return { ...input, type };
  }

  function toast(options: ToastOptions) {
    publish(TOAST_EVENT, {
      type: 'info',
      duration: 3000,
      position: 'top',
      ...options,
    });
  }

  return {
    notify,
    notifySuccess: (input: NotifyInput) =>
      notify(normalizeNotify(input, 'success')),
    notifyInfo: (input: NotifyInput) => notify(normalizeNotify(input, 'info')),
    notifyWarning: (input: NotifyInput) =>
      notify(normalizeNotify(input, 'warning')),
    notifyError: (input: NotifyInput) =>
      notify(normalizeNotify(input, 'error')),

    toast,
    toastSuccess: (input: ToastInput) =>
      toast(normalizeToast(input, 'success')),
    toastInfo: (input: ToastInput) => toast(normalizeToast(input, 'info')),
    toastWarning: (input: ToastInput) =>
      toast(normalizeToast(input, 'warning')),
    toastError: (input: ToastInput) => toast(normalizeToast(input, 'error')),
    toastLoading: (input: ToastInput) =>
      toast(normalizeToast(input, 'loading')),
  };
}
