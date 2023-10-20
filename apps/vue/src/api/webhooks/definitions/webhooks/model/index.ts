interface WebhookDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
  description?: string;
  isEnabled: boolean;
  requiredFeatures: string[];
}

export interface WebhookDefinitionDto extends IHasExtraProperties {
  groupName: string;
  name: string;
  displayName: string;
  description?: string;
  isEnabled: boolean;
  isStatic: boolean;
  requiredFeatures: string[];
}

export interface WebhookDefinitionCreateDto extends WebhookDefinitionCreateOrUpdateDto {
  groupName: string;
  name: string;
}

export interface WebhookDefinitionGetListInput {
  filter?: string;
  groupName?: string;
}

export type WebhookDefinitionUpdateDto = WebhookDefinitionCreateOrUpdateDto;