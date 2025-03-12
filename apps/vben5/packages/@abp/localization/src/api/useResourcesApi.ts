import type { ListResultDto } from '@abp/core';

import type {
  ResourceCreateDto,
  ResourceDto,
  ResourceGetListInput,
  ResourceUpdateDto,
} from '../types/resources';

import { useRequest } from '@abp/request';

export function useResourcesApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询资源列表
   * @param input 参数
   * @returns 资源列表
   */
  function getListApi(
    input?: ResourceGetListInput,
  ): Promise<ListResultDto<ResourceDto>> {
    return request<ListResultDto<ResourceDto>>(
      '/api/abp/localization/resources',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 查询资源
   * @param name 资源名称
   * @returns 查询的资源
   */
  function getApi(name: string): Promise<ResourceDto> {
    return request<ResourceDto>(`/api/localization/resources/${name}`, {
      method: 'GET',
    });
  }

  /**
   * 删除资源
   * @param name 资源名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/localization/resources/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 创建资源
   * @param input 参数
   * @returns 创建的资源
   */
  function createApi(input: ResourceCreateDto): Promise<ResourceDto> {
    return request<ResourceDto>(`/api/localization/resources`, {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 编辑资源
   * @param name 资源名称
   * @param input 参数
   * @returns 编辑的资源
   */
  function updateApi(
    name: string,
    input: ResourceUpdateDto,
  ): Promise<ResourceDto> {
    return request<ResourceDto>(`/api/localization/resources/${name}`, {
      data: input,
      method: 'PUT',
    });
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
