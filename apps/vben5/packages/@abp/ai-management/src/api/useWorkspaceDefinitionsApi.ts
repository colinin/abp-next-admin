import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  ChatClientProviderDto,
  WorkspaceDefinitionRecordCreateDto,
  WorkspaceDefinitionRecordDto,
  WorkspaceDefinitionRecordGetPagedListInput,
  WorkspaceDefinitionRecordUpdateDto,
} from '../types/workspaces';

import { useRequest } from '@abp/request';

export function useWorkspaceDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取工作区详情
   * @param id 工作区id
   * @returns 工作区详情
   */
  function getApi(id: string): Promise<WorkspaceDefinitionRecordDto> {
    return request<WorkspaceDefinitionRecordDto>(
      `/api/ai-management/workspaces/${id}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 获取工作区分页列表
   * @param input 查询过滤参数
   * @returns 工作区分页列表
   */
  function getPagedListApi(
    input?: WorkspaceDefinitionRecordGetPagedListInput,
  ): Promise<PagedResultDto<WorkspaceDefinitionRecordDto>> {
    return request<PagedResultDto<WorkspaceDefinitionRecordDto>>(
      '/api/ai-management/workspaces',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 获取可用模型提供者列表
   * @returns 模型提供者列表
   */
  function getAvailableProviderListApi(): Promise<
    ListResultDto<ChatClientProviderDto>
  > {
    return request<ListResultDto<ChatClientProviderDto>>(
      '/api/ai-management/workspaces/available-providers',
      {
        method: 'GET',
      },
    );
  }

  /**
   * 删除工作区
   * @param id 工作区id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/ai-management/workspaces/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 新增工作区
   * @param input 参数
   * @returns 工作区详情
   */
  function createApi(
    input: WorkspaceDefinitionRecordCreateDto,
  ): Promise<WorkspaceDefinitionRecordDto> {
    return request<WorkspaceDefinitionRecordDto>(
      `/api/ai-management/workspaces`,
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 新增工作区
   * @param id 工作区id
   * @param input 参数
   * @returns 工作区详情
   */
  function updateApi(
    id: string,
    input: WorkspaceDefinitionRecordUpdateDto,
  ): Promise<WorkspaceDefinitionRecordDto> {
    return request<WorkspaceDefinitionRecordDto>(
      `/api/ai-management/workspaces/${id}`,
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
