import type { PagedAndSortedResultRequestDto } from '@abp/core';

import type {
  NotificationContentType,
  NotificationData,
  NotificationLifetime,
  NotificationReadState,
  NotificationSeverity,
  NotificationType,
} from './notifications';

interface GetMyNotifilerPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  /** 已读状态 */
  readState?: NotificationReadState;
}

interface MarkReadStateInput {
  idList: string[];
  state: NotificationReadState;
}

interface UserNotificationDto {
  /** 内容类型 */
  contentType: NotificationContentType;
  /** 创建时间 */
  creationTime: Date;
  /** 数据 */
  data: NotificationData;
  /** Id */
  id: string;
  /** 生命周期 */
  lifetime: NotificationLifetime;
  /** 名称 */
  name: string;
  /** 紧急程度 */
  severity: NotificationSeverity;
  /** 阅读状态 */
  state: NotificationReadState;
  /** 类型 */
  type: NotificationType;
}

export type {
  GetMyNotifilerPagedListInput,
  MarkReadStateInput,
  UserNotificationDto,
};
