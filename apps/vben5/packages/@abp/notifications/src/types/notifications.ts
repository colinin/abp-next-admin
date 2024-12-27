import type { Dictionary } from '@abp/core';

export enum NotificationLifetime {
  OnlyOne = 1,
  Persistent = 0,
}

export enum NotificationType {
  Application = 0,
  ServiceCallback = 30,
  System = 10,
  User = 20,
}

export enum NotificationContentType {
  Html = 1,
  Json = 3,
  Markdown = 2,
  Text = 0,
}

export enum NotificationSeverity {
  Error = 30,
  Fatal = 40,
  Info = 10,
  Success = 0,
  Warn = 20,
}

export enum NotificationReadState {
  Read = 0,
  UnRead = 1,
}

interface NotificationData {
  extraProperties: { [key: string]: any };
  type: string;
}

interface UserIdentifier {
  userId: string;
  userName?: string;
}

interface NotificationSendInput {
  culture?: string;
  data: Dictionary<string, any>;
  name: string;
  severity?: NotificationSeverity;
  toUsers?: UserIdentifier[];
}

interface NotificationInfo {
  contentType: NotificationContentType;
  creationTime: Date;
  data: NotificationData;
  id: string;
  lifetime: NotificationLifetime;
  name: string;
  severity: NotificationSeverity;
  type: NotificationType;
}

interface NotificationDto {
  description: string;
  displayName: string;
  lifetime: NotificationLifetime;
  name: string;
  type: NotificationType;
}

interface NotificationGroupDto {
  displayName: string;
  name: string;
  notifications: NotificationDto[];
}

export type {
  NotificationData,
  NotificationDto,
  NotificationGroupDto,
  NotificationInfo,
  NotificationSendInput,
};
