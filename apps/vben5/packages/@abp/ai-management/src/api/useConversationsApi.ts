import type { PagedResultDto } from '@abp/core';

import type {
  ConversationCreateDto,
  ConversationDto,
  ConversationGetPagedListInput,
  ConversationUpdateDto,
} from '../types/conversations';

import { useRequest } from '@abp/request';

export function useConversationsApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取会话详情
   * @param id 会话id
   * @returns 会话详情
   */
  function getApi(id: string): Promise<ConversationDto> {
    return request<ConversationDto>(
      `/api/ai-management/chats/conversations/${id}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 获取会话分页列表
   * @param input 查询过滤参数
   * @returns 会话分页列表
   */
  function getPagedListApi(
    input?: ConversationGetPagedListInput,
  ): Promise<PagedResultDto<ConversationDto>> {
    return request<PagedResultDto<ConversationDto>>(
      '/api/ai-management/chats/conversations',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 删除会话
   * @param id 会话id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/ai-management/chats/conversations/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 新增会话
   * @param input 参数
   * @returns 会话详情
   */
  function createApi(input: ConversationCreateDto): Promise<ConversationDto> {
    return request<ConversationDto>(`/api/ai-management/chats/conversations`, {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 编辑会话
   * @param id 会话id
   * @param input 参数
   * @returns 会话详情
   */
  function updateApi(
    id: string,
    input: ConversationUpdateDto,
  ): Promise<ConversationDto> {
    return request<ConversationDto>(`/api/ai-management/conversations/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getPagedListApi,
    updateApi,
  };
}
