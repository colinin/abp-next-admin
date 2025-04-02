import type { ListResultDto } from '@abp/core';

import type {
  FeatureGroupDefinitionCreateDto,
  FeatureGroupDefinitionDto,
  FeatureGroupDefinitionGetListInput,
  FeatureGroupDefinitionUpdateDto,
} from '../types/groups';

import { useRequest } from '@abp/request';

export function useFeatureGroupDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除功能定义
   * @param name 功能名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/feature-management/definitions/groups/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询功能定义
   * @param name 功能名称
   * @returns 功能定义数据传输对象
   */
  function getApi(name: string): Promise<FeatureGroupDefinitionDto> {
    return request<FeatureGroupDefinitionDto>(
      `/api/feature-management/definitions/groups/${name}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 查询功能定义列表
   * @param input 功能过滤条件
   * @returns 功能定义数据传输对象列表
   */
  function getListApi(
    input?: FeatureGroupDefinitionGetListInput,
  ): Promise<ListResultDto<FeatureGroupDefinitionDto>> {
    return request<ListResultDto<FeatureGroupDefinitionDto>>(
      `/api/feature-management/definitions/groups`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 创建功能定义
   * @param input 功能定义参数
   * @returns 功能定义数据传输对象
   */
  function createApi(
    input: FeatureGroupDefinitionCreateDto,
  ): Promise<FeatureGroupDefinitionDto> {
    return request<FeatureGroupDefinitionDto>(
      '/api/feature-management/definitions/groups',
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 更新功能定义
   * @param name 功能名称
   * @param input 功能定义参数
   * @returns 功能定义数据传输对象
   */
  function updateApi(
    name: string,
    input: FeatureGroupDefinitionUpdateDto,
  ): Promise<FeatureGroupDefinitionDto> {
    return request<FeatureGroupDefinitionDto>(
      `/api/feature-management/definitions/groups/${name}`,
      {
        data: input,
        method: 'PUT',
      },
    );
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getListApi,
    updateApi,
  };
}
