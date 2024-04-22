interface WebhookGroupDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
}

export interface WebhookGroupDefinitionCreateDto extends WebhookGroupDefinitionCreateOrUpdateDto {
  name: string;
}

export interface WebhookGroupDefinitionDto extends IHasExtraProperties {
  name: string;
  displayName: string;
  isStatic: boolean;
}

export interface WebhookGroupDefinitionGetListInput {
  filter?: string;
}

export type WebhookGroupDefinitionUpdateDto = WebhookGroupDefinitionCreateOrUpdateDto;
