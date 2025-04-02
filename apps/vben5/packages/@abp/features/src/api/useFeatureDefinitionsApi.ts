import type { ListResultDto } from '@abp/core';

import type {
  FeatureDefinitionCreateDto,
  FeatureDefinitionDto,
  FeatureDefinitionGetListInput,
  FeatureDefinitionUpdateDto,
} from '../types/definitions';

import { useRequest } from '@abp/request';

export function useFeatureDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除功能定义
   * @param name 功能名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/feature-management/definitions/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询功能定义
   * @param name 功能名称
   * @returns 功能定义数据传输对象
   */
  function getApi(name: string): Promise<FeatureDefinitionDto> {
    return request<FeatureDefinitionDto>(
      `/api/feature-management/definitions/${name}`,
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
    input?: FeatureDefinitionGetListInput,
  ): Promise<ListResultDto<FeatureDefinitionDto>> {
    return request<ListResultDto<FeatureDefinitionDto>>(
      `/api/feature-management/definitions`,
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
    input: FeatureDefinitionCreateDto,
  ): Promise<FeatureDefinitionDto> {
    return request<FeatureDefinitionDto>(
      '/api/feature-management/definitions',
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
    input: FeatureDefinitionUpdateDto,
  ): Promise<FeatureDefinitionDto> {
    return request<FeatureDefinitionDto>(
      `/api/feature-management/definitions/${name}`,
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
