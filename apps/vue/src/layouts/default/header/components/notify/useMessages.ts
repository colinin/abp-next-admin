import { onMounted, onUnmounted, ref } from 'vue';
import { useUserStoreWithOut } from '/@/store/modules/user';
import { create } from '/@/api/messages/friends';
import { getLastMessages } from '/@/api/messages/messages';
import {
  ChatMessage,
  MessageSourceTye,
  MessageState,
  MessageType,
} from '/@/api/messages/model/messagesModel';
import { formatToDateTime } from '/@/utils/dateUtil';
import { TabItem, ListItem } from './data';
import { ChatEventEnum, NotifyEventEnum } from '/@/enums/imEnum';
import emitter from '/@/utils/eventBus';
import { NotificationInfo } from '/@/api/messages/model/notificationsModel';
import { useMessage } from '/@/hooks/web/useMessage';
import { useI18n } from '/@/hooks/web/useI18n';
import { useSignalR } from '/@/hooks/web/useSignalR';

export function useMessages() {
  const messageRef = ref<TabItem>({
    key: '2',
    name: '消息',
    list: [],
  });
  const { t } = useI18n();
  const userStore = useUserStoreWithOut();
  const sender = userStore.getUserInfo;
  const { createConfirm, createMessage } = useMessage();
  const signalR = useSignalR({
    autoStart: false,
    serverUrl: '/signalr-hubs/signalr-hubs/messages',
  });

  onMounted(() => {
    signalR.beforeStart(_registerIMEventHandler);
    signalR.on('get-chat-message', onMessageReceived);
    signalR.on('recall-chat-message', (message: ChatMessage) =>
      emitter.emit(ChatEventEnum.USER_MESSAGE_RECALL, message),
    );
    signalR.on('on-user-onlined', (_, userId: string) =>
      emitter.emit(ChatEventEnum.USER_ONLINE, userId),
    );
    signalR.on('on-user-offlined', (_, userId: string) =>
      emitter.emit(ChatEventEnum.USER_OFFLINE, userId),
    );
    signalR.onStart(refreshLastMessages);
    signalR.start();
  });

  onUnmounted(() => {
    signalR.beforeStop(_releaseIMEventHandler);
    signalR.stop();
  });

  function clearMessage() {
    messageRef.value.list.length = 0;
  }

  function refreshLastMessages(maxResultCount: number = 10) {
    getLastMessages({
      sorting: '',
      state: MessageState.Send,
      maxResultCount: maxResultCount,
    }).then((res) => {
      messageRef.value.list = res.items.map((message) => _transferMessage(message));
    });
  }

  function onMessageReceived(message: ChatMessage) {
    // 处理需要本地化的系统消息
    if (message.source === MessageSourceTye.System && message.extraProperties.L === true) {
      message.content = t(
        message.extraProperties.content.ResourceName + '.' + message.extraProperties.content.Name,
        message.extraProperties.content.Values as Recordable,
      );
    }
    if (message.groupId) {
      emitter.emit(ChatEventEnum.USER_MESSAGE_GROUP_NEW, message);
    } else {
      emitter.emit(ChatEventEnum.USER_MESSAGE_NEW, message);
    }
    if (message.toUserId === sender.userId) {
      messageRef.value.list.push(_transferMessage(message));
    }
  }

  function _transferMessage(message) {
    const messageItem: ListItem = {
      id: message.messageId,
      avatar: message.avatar,
      title: message.formUserName,
      description: message.content,
      datetime: formatToDateTime(message.sendTime, 'YYYY-MM-DD HH:mm:ss'),
      type: String(message.messageType),
    };
    return messageItem;
  }

  function _sendMessage(data: {
    receivedUserId: string;
    groupId?: string;
    content: string;
    type: MessageType;
    isAnonymous?: boolean;
  }) {
    const chatMessage: ChatMessage = {
      messageId: '',
      sendTime: new Date(),
      toUserId: data.receivedUserId,
      formUserId: String(sender.userId),
      formUserName: sender.realName ?? sender.username,
      groupId: data.groupId,
      content: data.content,
      messageType: data.type,
      extraProperties: {},
      source: MessageSourceTye.User,
      isAnonymous: data.isAnonymous ?? false,
    };
    signalR.invoke('send', chatMessage).then(() => {
      !chatMessage.groupId && onMessageReceived(chatMessage);
    });
  }

  function _recall(message: ChatMessage) {
    if (message.source !== MessageSourceTye.System) {
      signalR.invoke('recall', message);
    }
  }

  function _read(message: ChatMessage) {
    signalR.invoke('read', message);
  }

  function _mapChatNotifyEvent(notifier: NotificationInfo) {
    switch (notifier.name) {
      case 'LINGYUN.Abp.Messages.IM.FriendValidation':
        createConfirm({
          title: notifier.data.extraProperties.title,
          content: notifier.data.extraProperties.message,
          iconType: 'info',
          onOk: () => {
            create({
              friendId: notifier.data.extraProperties.userId,
            }).then(() => {
              createMessage.success(t('AbpUi.Successful'));
              emitter.emit(NotifyEventEnum.NOTIFICATIONS_READ, notifier);
            });
          },
        });
        break;
    }
  }
  /** 注册IM事件处理器 */
  function _registerIMEventHandler() {
    // 读取消息事件
    emitter.on(ChatEventEnum.USER_MESSAGE_READ, _read);
    // 读取消息事件
    emitter.on(ChatEventEnum.USER_MESSAGE_RECALL, _recall);
    // 订阅发送消息事件
    emitter.on(ChatEventEnum.USER_SEND_MESSAGE, _sendMessage);
    // 订阅通知事件,处理IM相关消息
    emitter.on(NotifyEventEnum.NOTIFICATIONS_RECEVIED, _mapChatNotifyEvent);
  }
  /** 释放IM事件处理器 */
  function _releaseIMEventHandler() {
    emitter.off(ChatEventEnum.USER_MESSAGE_READ, _read);
    emitter.off(ChatEventEnum.USER_MESSAGE_RECALL, _recall);
    emitter.off(ChatEventEnum.USER_SEND_MESSAGE, _sendMessage);
    emitter.off(NotifyEventEnum.NOTIFICATIONS_RECEVIED, _mapChatNotifyEvent);
  }

  return {
    messageRef,
    clearMessage,
  };
}
