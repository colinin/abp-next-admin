import type { IHasConcurrencyStamp, IHasExtraProperties } from '@abp/core';

interface FeatureDefinitionDto extends IHasExtraProperties {
  allowedProviders: string[];
  defaultValue?: string;
  description?: string;
  displayName: string;
  groupName: string;
  isAvailableToHost: boolean;
  isStatic: boolean;
  isVisibleToClients: boolean;
  name: string;
  parentName?: string;
  valueType: string;
}

interface FeatureDefinitionGetListInput {
  filter?: string;
  groupName?: string;
}

interface FeatureDefinitionCreateOrUpdateDto extends IHasExtraProperties {
  allowedProviders: string[];
  defaultValue?: string;
  description?: string;
  displayName: string;
  isAvailableToHost: boolean;
  isVisibleToClients: boolean;
  parentName?: string;
  valueType: string;
}

interface FeatureDefinitionUpdateDto
  extends FeatureDefinitionCreateOrUpdateDto,
    IHasConcurrencyStamp {}
interface FeatureDefinitionCreateDto
  extends FeatureDefinitionCreateOrUpdateDto {
  groupName: string;
  name: string;
}

export type {
  FeatureDefinitionCreateDto,
  FeatureDefinitionDto,
  FeatureDefinitionGetListInput,
  FeatureDefinitionUpdateDto,
};
