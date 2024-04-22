import { NotificationContentType, NotificationLifetime, NotificationType } from "../../../types";

interface NotificationDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
  description?: string;
  template?: string;
  allowSubscriptionToClients: boolean;
  notificationType: NotificationType;
  contentType: NotificationContentType;
  notificationLifetime: NotificationLifetime;
  providers: string[];
}

export interface NotificationDefinitionCreateDto extends NotificationDefinitionCreateOrUpdateDto {
  name: string;
  groupName: string;
}

export interface NotificationDefinitionDto extends ExtensibleObject {
  name: string;
  groupName: string;
  isStatic: boolean;
  displayName: string;
  description?: string;
  template?: string;
  allowSubscriptionToClients: boolean;
  notificationType: NotificationType;
  contentType: NotificationContentType;
  notificationLifetime: NotificationLifetime;
  providers: string[];
}

export interface NotificationDefinitionGetListInput {
  filter?: string;
  groupName?: string;
  template?: string;
  allowSubscriptionToClients?: boolean;
  notificationType?: NotificationType;
  contentType?: NotificationContentType;
  notificationLifetime?: NotificationLifetime;
}

export type NotificationDefinitionUpdateDto = NotificationDefinitionCreateOrUpdateDto;
