import type { Dictionary, NameValue } from '@abp/core';

interface FeatureProvider {
  providerKey?: string;
  providerName: string;
}

interface IValueValidator {
  [key: string]: any;
  isValid(value?: any): boolean;
  name: string;
  properties: Dictionary<string, any>;
}

interface IStringValueType {
  [key: string]: any;
  name: string;
  properties: Dictionary<string, any>;
  validator: IValueValidator;
}

interface FeatureProviderDto {
  key: string;
  name: string;
}

interface FeatureDto {
  depth: number;
  description?: string;
  displayName: string;
  name: string;
  parentName?: string;
  provider: FeatureProviderDto;
  value?: any;
  valueType: IStringValueType;
}

interface FeatureGroupDto {
  displayName: string;
  features: FeatureDto[];
  name: string;
}

interface GetFeatureListResultDto {
  groups: FeatureGroupDto[];
}

type UpdateFeatureDto = NameValue<string>;

interface UpdateFeaturesDto {
  features: UpdateFeatureDto[];
}

export type {
  FeatureDto,
  FeatureGroupDto,
  FeatureProvider,
  GetFeatureListResultDto,
  IStringValueType,
  IValueValidator,
  UpdateFeatureDto,
  UpdateFeaturesDto,
};
