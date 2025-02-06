import type { NotificationLifetime, NotificationType } from './notifications';

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

interface NotificationTemplateDto {
  content?: string;
  culture?: string;
  description?: string;
  name: string;
  title: string;
}

export type { NotificationDto, NotificationGroupDto, NotificationTemplateDto };
