import type { ExtensibleObject, IHasExtraProperties } from '@abp/core';

import type {
  NotificationContentType,
  NotificationLifetime,
  NotificationType,
} from './notifications';

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

interface NotificationProviderDto {
  name: string;
}

interface NotificationTemplateDto {
  content?: string;
  culture?: string;
  description?: string;
  name: string;
  title: string;
}

interface NotificationDefinitionDto extends ExtensibleObject {
  allowSubscriptionToClients: boolean;
  contentType: NotificationContentType;
  description?: string;
  displayName: string;
  groupName: string;
  isStatic: boolean;
  name: string;
  notificationLifetime: NotificationLifetime;
  notificationType: NotificationType;
  providers?: string[];
  template?: string;
}

interface NotificationDefinitionGetListInput {
  allowSubscriptionToClients?: boolean;
  contentType?: NotificationContentType;
  filter?: string;
  groupName?: string;
  notificationLifetime?: NotificationLifetime;
  notificationType?: NotificationType;
  template?: string;
}

interface NotificationDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  allowSubscriptionToClients?: boolean;
  contentType?: NotificationContentType;
  description?: string;
  displayName: string;
  notificationLifetime?: NotificationLifetime;
  notificationType?: NotificationType;
  providers?: string[];
  template?: string;
}

type NotificationDefinitionUpdateDto = NotificationDefinitionCreateOrUpdateDto;

interface NotificationDefinitionCreateDto
  extends NotificationDefinitionCreateOrUpdateDto {
  groupName?: string;
  name: string;
}

export type {
  NotificationDefinitionCreateDto,
  NotificationDefinitionDto,
  NotificationDefinitionGetListInput,
  NotificationDefinitionUpdateDto,
  NotificationDto,
  NotificationGroupDto,
  NotificationProviderDto,
  NotificationTemplateDto,
};
