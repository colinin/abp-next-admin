import type { PagedResultDto } from '@abp/core';

import type {
  NotificationSendRecordDto,
  NotificationSendRecordGetPagedListInput,
} from '../types/send-records';

import { useRequest } from '@abp/request';

export function useNotificationSendRecordsApi() {
  const { cancel, request } = useRequest();
  /**
   * 获取通知发送记录分页列表
   * @param input 查询过滤参数
   * @returns {Promise<PagedResultDto<NotificationSendRecordDto>>} 通知发送记录分页列表
   */
  function getPagedListApi(
    input?: NotificationSendRecordGetPagedListInput,
  ): Promise<PagedResultDto<NotificationSendRecordDto>> {
    return request<PagedResultDto<NotificationSendRecordDto>>(
      '/api/notifications/send-records',
      {
        method: 'GET',
        params: input,
      },
    );
  }
  /**
   * 删除发送记录
   * @param id 发送记录Id
   * @returns {Promise<void>}
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/notifications/send-records/${id}`, {
      method: 'DELETE',
    });
  }
  /**
   * 重新发送通知
   * @param id 发送记录Id
   * @returns {Promise<void>}
   */
  function reSendApi(id: string): Promise<void> {
    return request(`/api/notifications/send-records/${id}/re-send`, {
      method: 'POST',
    });
  }

  return {
    cancel,
    deleteApi,
    getPagedListApi,
    reSendApi,
  };
}
