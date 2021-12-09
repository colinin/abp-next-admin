import {
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';

export enum NotificationLifetime {
  Persistent = 0,
  OnlyOne = 1,
}

export enum NotificationType {
  Application = 0,
  System = 10,
  User = 20,
}

export enum NotificationSeverity {
  Success = 0,
  Info = 10,
  Warn = 20,
  Error = 30,
  Fatal = 40,
}

export enum NotificationReadState {
  Read = 0,
  UnRead = 1,
}

export interface NotificationData {
  type: string;
  extraProperties: { [key: string]: any };
}

export interface NotificationInfo {
  // tenantId?: string;
  name: string;
  id: string;
  data: NotificationData;
  creationTime: Date;
  lifetime: NotificationLifetime;
  type: NotificationType;
  severity: NotificationSeverity;
}

export interface NotificationGroup {
  name: string;
  displayName: string;
  notifications: {
    name: string;
    displayName: string;
    description: string;
    type: NotificationType;
    lifetime: NotificationLifetime;
  }[];
}

export class GetNotificationPagedRequest extends PagedAndSortedResultRequestDto {
  reverse = true;
  readState = NotificationReadState.UnRead;
}

export class NotificationPagedResult extends PagedResultDto<NotificationInfo> {}

export class NotificationGroupListResult extends ListResultDto<NotificationGroup> {}
