import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  AIToolDefinitionRecordCreateDto,
  AIToolDefinitionRecordDto,
  AIToolDefinitionRecordGetPagedListInput,
  AIToolDefinitionRecordUpdateDto,
  AIToolProviderDto,
} from '../types/tools';

import { useRequest } from '@abp/request';

export function useAIToolsApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取AI工具详情
   * @param id AI工具id
   * @returns AI工具详情
   */
  function getApi(id: string): Promise<AIToolDefinitionRecordDto> {
    return request<AIToolDefinitionRecordDto>(
      `/api/ai-management/tools/${id}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 获取AI工具分页列表
   * @param input 查询过滤参数
   * @returns AI工具分页列表
   */
  function getPagedListApi(
    input?: AIToolDefinitionRecordGetPagedListInput,
  ): Promise<PagedResultDto<AIToolDefinitionRecordDto>> {
    return request<PagedResultDto<AIToolDefinitionRecordDto>>(
      '/api/ai-management/tools',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 获取可用AI工具提供者列表
   * @returns AI工具提供者列表
   */
  function getAvailableProviderListApi(): Promise<
    ListResultDto<AIToolProviderDto>
  > {
    return request<ListResultDto<AIToolProviderDto>>(
      '/api/ai-management/tools/available-providers',
      {
        method: 'GET',
      },
    );
  }

  /**
   * 删除AI工具
   * @param id AI工具id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/ai-management/tools/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 新增AI工具
   * @param input 参数
   * @returns AI工具详情
   */
  function createApi(
    input: AIToolDefinitionRecordCreateDto,
  ): Promise<AIToolDefinitionRecordDto> {
    return request<AIToolDefinitionRecordDto>(`/api/ai-management/tools`, {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 编辑AI工具
   * @param id AI工具id
   * @param input 参数
   * @returns AI工具详情
   */
  function updateApi(
    id: string,
    input: AIToolDefinitionRecordUpdateDto,
  ): Promise<AIToolDefinitionRecordDto> {
    return request<AIToolDefinitionRecordDto>(
      `/api/ai-management/tools/${id}`,
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
    getAvailableProviderListApi,
    getPagedListApi,
    updateApi,
  };
}
