import { defAbpHttp } from '/@/utils/http/abp';
import { FeatureGroupResult, UpdateFeatures } from './model/featureModel';

/** 与 multi-tenancy中不同,此为管理tenant api */
enum Api {
  Get = '/api/feature-management/features',
  Update = '/api/feature-management/features',
}

export const get = (provider: { providerName: string; providerKey: string | null }) => {
  return defAbpHttp.get<FeatureGroupResult>({
    url: Api.Get,
    params: provider,
  });
};

export const update = (
  provider: { providerName: string; providerKey: string | null },
  input: UpdateFeatures
) => {
  return defAbpHttp.put<void>({
    url: Api.Update,
    data: input,
    params: provider,
  });
};
