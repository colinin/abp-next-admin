import type { PagedResultDto } from '@abp/core';

import type {
  EmailMessageDto,
  EmailMessageGetListInput,
} from '../types/messages';

import { useRequest } from '@abp/request';

export function useEmailMessagesApi() {
  const { cancel, request } = useRequest();
  /**
   * 获取邮件消息分页列表
   * @param {EmailMessageGetListInput} input 参数
   * @returns {Promise<PagedResultDto<EmailMessageDto>>} 邮件消息列表
   */
  function getPagedListApi(
    input?: EmailMessageGetListInput,
  ): Promise<PagedResultDto<EmailMessageDto>> {
    return request<PagedResultDto<EmailMessageDto>>(
      '/api/platform/messages/email',
      {
        method: 'GET',
        params: input,
      },
    );
  }
  /**
   * 获取邮件消息
   * @param id Id
   * @returns {EmailMessageDto} 邮件消息
   */
  function getApi(id: string): Promise<EmailMessageDto> {
    return request<EmailMessageDto>(`/api/platform/messages/email/${id}`, {
      method: 'GET',
    });
  }
  /**
   * 删除邮件消息
   * @param id Id
   * @returns {void}
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/platform/messages/email/${id}`, {
      method: 'DELETE',
    });
  }
  /**
   * 发送邮件消息
   * @param id Id
   * @returns {void}
   */
  function sendApi(id: string): Promise<void> {
    return request(`/api/platform/messages/email/${id}/send`, {
      method: 'POST',
    });
  }

  return {
    cancel,
    deleteApi,
    getApi,
    getPagedListApi,
    sendApi,
  };
}
