import { defAbpHttp } from '/@/utils/http/abp';
import { FeatureGroupResult, UpdateFeatures, FeatureUpdateByProvider, FeatureGetByProvider } from './model';

export const GetByProvider = (provider: FeatureGetByProvider) => {
  return defAbpHttp.get<FeatureGroupResult>({
    url: '/api/feature-management/features',
    params: provider,
  });
};

export const UpdateByProvider = (
  provider: FeatureUpdateByProvider,
  input: UpdateFeatures
) => {
  return defAbpHttp.put<void>({
    url: '/api/feature-management/features',
    data: input,
    params: provider,
  });
};
