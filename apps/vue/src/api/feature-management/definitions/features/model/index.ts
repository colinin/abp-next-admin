interface FeatureDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
  parentName?: string;
  description?: string;
  defaultValue?: string;
  valueType: string;
  isVisibleToClients: boolean;
  isAvailableToHost: boolean;
  allowedProviders: string[];
}

export interface FeatureDefinitionCreateDto extends FeatureDefinitionCreateOrUpdateDto {
  name: string;
  groupName: string;
}

export interface FeatureDefinitionDto extends IHasExtraProperties {
  name: string;
  groupName: string;
  displayName: string;
  parentName?: string;
  description?: string;
  defaultValue?: string;
  valueType: string;
  isStatic: boolean;
  isVisibleToClients: boolean;
  isAvailableToHost: boolean;
  allowedProviders: string[];
}

export interface FeatureDefinitionGetListInput {
  filter?: string;
  groupName?: string;
}

export type FeatureDefinitionUpdateDto = FeatureDefinitionCreateOrUpdateDto;
