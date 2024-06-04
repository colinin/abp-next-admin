import { defHttp } from '/@/utils/http/axios';
import { FeatureGroupResult, UpdateFeatures, FeatureUpdateByProvider, FeatureGetByProvider } from './model';

export const get = (provider: FeatureGetByProvider) => {
  return defHttp.get<FeatureGroupResult>({
    url: '/api/feature-management/features',
    params: provider,
  });
};

export const update = (
  provider: FeatureUpdateByProvider,
  input: UpdateFeatures
) => {
  return defHttp.put<void>({
    url: '/api/feature-management/features',
    data: input,
    params: provider,
  });
};
