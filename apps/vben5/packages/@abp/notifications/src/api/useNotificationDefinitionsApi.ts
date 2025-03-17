import type { ListResultDto } from '@abp/core';

import type {
  NotificationDefinitionCreateDto,
  NotificationDefinitionDto,
  NotificationDefinitionGetListInput,
  NotificationDefinitionUpdateDto,
} from '../types/definitions';

import { useRequest } from '@abp/request';

export function useNotificationDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除通知定义
   * @param name 通知名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/notifications/definitions/notifications/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询通知定义
   * @param name 通知名称
   * @returns 通知定义数据传输对象
   */
  function getApi(name: string): Promise<NotificationDefinitionDto> {
    return request<NotificationDefinitionDto>(
      `/api/notifications/definitions/notifications/${name}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 查询通知定义列表
   * @param input 通知过滤条件
   * @returns 通知定义数据传输对象列表
   */
  function getListApi(
    input?: NotificationDefinitionGetListInput,
  ): Promise<ListResultDto<NotificationDefinitionDto>> {
    return request<ListResultDto<NotificationDefinitionDto>>(
      `/api/notifications/definitions/notifications`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 创建通知定义
   * @param input 通知定义参数
   * @returns 通知定义数据传输对象
   */
  function createApi(
    input: NotificationDefinitionCreateDto,
  ): Promise<NotificationDefinitionDto> {
    return request<NotificationDefinitionDto>(
      '/api/notifications/definitions/notifications',
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 更新通知定义
   * @param name 通知名称
   * @param input 通知定义参数
   * @returns 通知定义数据传输对象
   */
  function updateApi(
    name: string,
    input: NotificationDefinitionUpdateDto,
  ): Promise<NotificationDefinitionDto> {
    return request<NotificationDefinitionDto>(
      `/api/notifications/definitions/notifications/${name}`,
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
    getListApi,
    updateApi,
  };
}
