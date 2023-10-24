interface FeatureGroupDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
}

export interface FeatureGroupDefinitionCreateDto extends FeatureGroupDefinitionCreateOrUpdateDto {
  name: string;
}

export interface FeatureGroupDefinitionDto extends IHasExtraProperties {
  name: string;
  displayName: string;
  isStatic: boolean;
}

export interface FeatureGroupDefinitionGetListInput {
  filter?: string;
}

export type FeatureGroupDefinitionUpdateDto = FeatureGroupDefinitionCreateOrUpdateDto;
