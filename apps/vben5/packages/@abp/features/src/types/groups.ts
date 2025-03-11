import type { IHasConcurrencyStamp, IHasExtraProperties } from '@abp/core';

interface FeatureGroupDefinitionDto extends IHasExtraProperties {
  displayName: string;
  isStatic: boolean;
  name: string;
}

interface FeatureGroupDefinitionGetListInput {
  filter?: string;
}

interface FeatureGroupDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  displayName: string;
}

interface FeatureGroupDefinitionUpdateDto
  extends FeatureGroupDefinitionCreateOrUpdateDto,
    IHasConcurrencyStamp {}

interface FeatureGroupDefinitionCreateDto
  extends FeatureGroupDefinitionCreateOrUpdateDto {
  name: string;
}

export type {
  FeatureGroupDefinitionCreateDto,
  FeatureGroupDefinitionDto,
  FeatureGroupDefinitionGetListInput,
  FeatureGroupDefinitionUpdateDto,
};
