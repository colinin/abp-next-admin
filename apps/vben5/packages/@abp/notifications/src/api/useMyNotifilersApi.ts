import type { PagedResultDto } from '@abp/core';

import type {
  GetMyNotifilerPagedListInput,
  MarkReadStateInput,
  UserNotificationDto,
} from '../types/my-notifilers';

import { useRequest } from '@abp/request';

export function useMyNotifilersApi() {
  const { cancel, request } = useRequest();
  /**
   * 获取我的通知列表
   * @param {GetMyNotifilerPagedListInput} input 参数
   * @returns {Promise<PagedResultDto<UserNotificationDto>>} 通知分页列表
   */
  function getMyNotifilersApi(
    input?: GetMyNotifilerPagedListInput,
  ): Promise<PagedResultDto<UserNotificationDto>> {
    return request<PagedResultDto<UserNotificationDto>>(
      '/api/notifications/my-notifilers',
      {
        method: 'GET',
        params: input,
      },
    );
  }
  /**
   * 删除我的通知
   * @param {string} id 通知id
   * @returns {void}
   */
  function deleteMyNotifilerApi(id: string): Promise<void> {
    return request(`/api/notifications/my-notifilers/${id}`, {
      method: 'DELETE',
    });
  }
  /**
   * 设置通知已读状态
   * @param {MarkReadStateInput} input 参数
   * @returns {void}
   */
  function markReadStateApi(input: MarkReadStateInput): Promise<void> {
    return request(`/api/notifications/my-notifilers/mark-read-state`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    deleteMyNotifilerApi,
    getMyNotifilersApi,
    markReadStateApi,
  };
}
