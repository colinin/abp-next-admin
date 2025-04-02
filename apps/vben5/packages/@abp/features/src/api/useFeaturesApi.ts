import type {
  FeatureProvider,
  GetFeatureListResultDto,
  UpdateFeaturesDto,
} from '../types/features';

import { useRequest } from '@abp/request';

export function useFeaturesApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除功能
   * @param {FeatureProvider} provider 参数
   * @returns {Promise<void>}
   */
  function deleteApi(provider: FeatureProvider): Promise<void> {
    return request(`/api/feature-management/features`, {
      method: 'DELETE',
      params: provider,
    });
  }

  /**
   * 查询功能
   * @param {FeatureProvider} provider 参数
   * @returns {Promise<GetFeatureListResultDto>} 功能实体数据传输对象
   */
  function getApi(provider: FeatureProvider): Promise<GetFeatureListResultDto> {
    return request<GetFeatureListResultDto>(
      `/api/feature-management/features`,
      {
        method: 'GET',
        params: provider,
      },
    );
  }

  /**
   * 更新功能
   * @param {FeatureProvider} provider
   * @param {UpdateFeaturesDto} input 参数
   * @returns {Promise<void>}
   */
  function updateApi(
    provider: FeatureProvider,
    input: UpdateFeaturesDto,
  ): Promise<void> {
    return request(`/api/feature-management/features`, {
      data: input,
      method: 'PUT',
      params: provider,
    });
  }

  return {
    cancel,
    deleteApi,
    getApi,
    updateApi,
  };
}
