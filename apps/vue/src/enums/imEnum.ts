export enum ChatEventEnum {
  /** 用户发送消息 */
  USER_SEND_MESSAGE = 'chat:user_send_message',
  /** 用户上线 */
  USER_ONLINE = 'chat:user_online',
  /** 用户离线 */
  USER_OFFLINE = 'chat:user_offline',
  /** 新好友申请 */
  USER_REQUEST_ADD_FRIEND = 'chat:user_request_friend',
  /** 群组新用户申请 */
  USER_REQUEST_ADD_GROUP = 'chat:user_request_group',
  /** 用户退出群组 */
  USER_EXIT_GROUP = 'chat:user_exit_group',
  /** 新用户消息 */
  USER_MESSAGE_NEW = 'chat:user_new_message',
  /** 新群组消息 */
  USER_MESSAGE_GROUP_NEW = 'chat:user_new_group_message',
  /** 已读消息 */
  USER_MESSAGE_READ = 'chat:user_read_message',
  /** 撤回消息 */
  USER_MESSAGE_RECALL = 'chat:user_recall_message',
}

export enum NotifyEventEnum {
  /** 新通知 */
  NOTIFICATIONS_RECEVIED = 'notifications:recevied',
  /** 通知已读 */
  NOTIFICATIONS_READ = 'notifications:read',
}
