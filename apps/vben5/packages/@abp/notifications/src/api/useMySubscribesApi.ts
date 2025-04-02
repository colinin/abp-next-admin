import type { ListResultDto } from '@abp/core';

import type { UserSubscreNotification } from '../types/subscribes';

import { useRequest } from '@abp/request';

export function useMySubscribesApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取我的所有订阅通知
   * @returns 订阅通知列表
   */
  function getMySubscribesApi(): Promise<
    ListResultDto<UserSubscreNotification>
  > {
    return request<ListResultDto<UserSubscreNotification>>(
      '/api/notifications/my-subscribes/all',
      {
        method: 'GET',
      },
    );
  }

  /**
   * 订阅通知
   * @param name 通知名称
   */
  function subscribeApi(name: string): Promise<void> {
    return request('/api/notifications/my-subscribes', {
      data: {
        name,
      },
      method: 'POST',
    });
  }

  /**
   * 取消订阅通知
   * @param name 通知名称
   */
  function unSubscribeApi(name: string): Promise<void> {
    return request(`/api/notifications/my-subscribes?name=${name}`, {
      method: 'DELETE',
    });
  }

  return {
    cancel,
    getMySubscribesApi,
    subscribeApi,
    unSubscribeApi,
  };
}
