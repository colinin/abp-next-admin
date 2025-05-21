import type { PagedResultDto } from '@abp/core';

import type {
  WebhookSendRecordDeleteManyInput,
  WebhookSendRecordDto,
  WebhookSendRecordGetListInput,
  WebhookSendRecordResendManyInput,
} from '../types/sendAttempts';

import { useRequest } from '@abp/request';

export function useSendAttemptsApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询发送记录
   * @param id 记录Id
   * @returns 发送记录Dto
   */
  function getApi(id: string): Promise<WebhookSendRecordDto> {
    return request<WebhookSendRecordDto>(`/api/webhooks/send-attempts/${id}`, {
      method: 'GET',
    });
  }
  /**
   * 删除发送记录
   * @param id 记录Id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/webhooks/send-attempts/${id}`, {
      method: 'DELETE',
    });
  }
  /**
   * 批量删除发送记录
   * @param input 参数
   */
  function bulkDeleteApi(
    input: WebhookSendRecordDeleteManyInput,
  ): Promise<void> {
    return request(`/api/webhooks/send-attempts/delete-many`, {
      data: input,
      method: 'DELETE',
    });
  }
  /**
   * 查询发送记录分页列表
   * @param input 过滤参数
   * @returns 发送记录Dto分页列表
   */
  function getPagedListApi(
    input: WebhookSendRecordGetListInput,
  ): Promise<PagedResultDto<WebhookSendRecordDto>> {
    return request<PagedResultDto<WebhookSendRecordDto>>(
      `/api/webhooks/send-attempts`,
      {
        method: 'GET',
        params: input,
      },
    );
  }
  /**
   * 重新发送
   * @param id 记录Id
   */
  function reSendApi(id: string): Promise<void> {
    return request(`/api/webhooks/send-attempts/${id}/resend`, {
      method: 'POST',
    });
  }
  /**
   * 批量重新发送
   * @param input 参数
   */
  function bulkReSendApi(
    input: WebhookSendRecordResendManyInput,
  ): Promise<void> {
    return request(`/api/webhooks/send-attempts/resend-many`, {
      data: input,
      method: 'POST',
    });
  }

  return {
    bulkDeleteApi,
    bulkReSendApi,
    cancel,
    deleteApi,
    getApi,
    getPagedListApi,
    reSendApi,
  };
}
