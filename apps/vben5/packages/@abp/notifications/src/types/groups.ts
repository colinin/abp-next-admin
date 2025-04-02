import type { ExtensibleObject, IHasExtraProperties } from '@abp/core';

interface NotificationGroupDefinitionDto extends ExtensibleObject {
  allowSubscriptionToClients: boolean;
  description?: string;
  displayName: string;
  isStatic: boolean;
  name: string;
}

interface NotificationGroupDefinitionGetListInput {
  filter?: string;
}

interface NotificationGroupDefinitionCreateOrUpdateDto
  extends IHasExtraProperties {
  allowSubscriptionToClients: boolean;
  description?: string;
  displayName: string;
}

interface NotificationGroupDefinitionCreateDto
  extends NotificationGroupDefinitionCreateOrUpdateDto {
  name: string;
}

type NotificationGroupDefinitionUpdateDto =
  NotificationGroupDefinitionCreateOrUpdateDto;

export type {
  NotificationGroupDefinitionCreateDto,
  NotificationGroupDefinitionDto,
  NotificationGroupDefinitionGetListInput,
  NotificationGroupDefinitionUpdateDto,
};
