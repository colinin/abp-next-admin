import type { Notification as NotificationInfo } from '@abp/notifications';

import { onMounted, onUnmounted } from 'vue';

import { useAbpStore, useEventBus } from '@abp/core';
import { NotificationNames, useNotifications } from '@abp/notifications';
import { Modal } from 'ant-design-vue';

import { useAuthStore } from '#/store';

export function useSessions() {
  const authStore = useAuthStore();
  const abpStore = useAbpStore();
  const { subscribe, unSubscribe } = useEventBus();
  const { register, release } = useNotifications();

  function _register() {
    subscribe(NotificationNames.SessionExpiration, _onSessionExpired);
    register();
  }

  function _release() {
    unSubscribe(NotificationNames.SessionExpiration, _onSessionExpired);
    release();
  }

  /** 处理会话过期事件 */
  function _onSessionExpired(event?: NotificationInfo) {
    if (!event) {
      return;
    }
    const { data, title, message } = event;
    const sessionId = data.SessionId;
    if (sessionId === abpStore.application?.currentUser?.sessionId) {
      _release();
      Modal.confirm({
        iconType: 'warning',
        title,
        content: message,
        centered: true,
        afterClose: () => {
          authStore.logout();
        },
      });
    }
  }

  onMounted(_register);
  onUnmounted(_release);
}
