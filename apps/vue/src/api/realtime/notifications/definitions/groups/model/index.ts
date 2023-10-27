interface NotificationGroupDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
  description?: string;
  allowSubscriptionToClients: boolean;
}

export interface NotificationGroupDefinitionCreateDto extends NotificationGroupDefinitionCreateOrUpdateDto {
  name: string;
}

export interface NotificationGroupDefinitionDto extends ExtensibleObject {
  name: string;
  displayName: string;
  description?: string;
  isStatic: boolean;
  allowSubscriptionToClients: boolean;
}

export interface NotificationGroupDefinitionGetListInput {
  filter?: string;
}

export type NotificationGroupDefinitionUpdateDto = NotificationGroupDefinitionCreateOrUpdateDto;
