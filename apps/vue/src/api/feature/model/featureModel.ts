export interface Provider {
  name: string;
  key: string;
}

export interface Feature {
  name: string;
  displayName: string;
  value: any;
  provider: Provider;
  description?: string;
  valueType: ValueType;
  depth: number;
  parentName?: string;
}

export interface FeatureGroup {
  name: string;
  displayName: string;
  features: Feature[];
}

export class FeatureGroupResult {
  groups!: FeatureGroup[];
}

export interface UpdateFeature {
  name: string;
  value: string;
}

export interface UpdateFeatures {
  features: UpdateFeature[];
}
