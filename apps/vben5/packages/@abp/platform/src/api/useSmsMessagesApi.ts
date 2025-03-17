import type { PagedResultDto } from '@abp/core';

import type { SmsMessageDto, SmsMessageGetListInput } from '../types';

import { useRequest } from '@abp/request';

export function useSmsMessagesApi() {
  const { cancel, request } = useRequest();
  /**
   * 获取短信消息分页列表
   * @param {EmailMessageGetListInput} input 参数
   * @returns {Promise<PagedResultDto<EmailMessageDto>>} 短信消息列表
   */
  function getPagedListApi(
    input?: SmsMessageGetListInput,
  ): Promise<PagedResultDto<SmsMessageDto>> {
    return request<PagedResultDto<SmsMessageDto>>(
      '/api/platform/messages/sms',
      {
        method: 'GET',
        params: input,
      },
    );
  }
  /**
   * 获取短信消息
   * @param id Id
   * @returns {SmsMessageDto} 短信消息
   */
  function getApi(id: string): Promise<SmsMessageDto> {
    return request<SmsMessageDto>(`/api/platform/messages/sms/${id}`, {
      method: 'GET',
    });
  }
  /**
   * 删除短信消息
   * @param id Id
   * @returns {void}
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/platform/messages/sms/${id}`, {
      method: 'DELETE',
    });
  }
  /**
   * 发送短信消息
   * @param id Id
   * @returns {void}
   */
  function sendApi(id: string): Promise<void> {
    return request(`/api/platform/messages/sms/${id}/send`, {
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
