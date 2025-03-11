import type { ListResultDto } from '@abp/core';

import type {
  NotificationGroupDefinitionCreateDto,
  NotificationGroupDefinitionDto,
  NotificationGroupDefinitionGetListInput,
  NotificationGroupDefinitionUpdateDto,
} from '../types';

import { useRequest } from '@abp/request';

export function useNotificationGroupDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除通知分组定义
   * @param name 通知分组名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/notifications/definitions/groups/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询通知分组定义
   * @param name 通知分组名称
   * @returns 通知分组定义数据传输对象
   */
  function getApi(name: string): Promise<NotificationGroupDefinitionDto> {
    return request<NotificationGroupDefinitionDto>(
      `/api/notifications/definitions/groups/${name}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 查询通知分组定义列表
   * @param input 通知分组过滤条件
   * @returns 通知分组定义数据传输对象列表
   */
  function getListApi(
    input?: NotificationGroupDefinitionGetListInput,
  ): Promise<ListResultDto<NotificationGroupDefinitionDto>> {
    return request<ListResultDto<NotificationGroupDefinitionDto>>(
      `/api/notifications/definitions/groups`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 创建通知分组定义
   * @param input 通知分组定义参数
   * @returns 通知分组定义数据传输对象
   */
  function createApi(
    input: NotificationGroupDefinitionCreateDto,
  ): Promise<NotificationGroupDefinitionDto> {
    return request<NotificationGroupDefinitionDto>(
      '/api/notifications/definitions/groups',
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 更新通知分组定义
   * @param name 通知分组名称
   * @param input 通知分组定义参数
   * @returns 通知分组定义数据传输对象
   */
  function updateApi(
    name: string,
    input: NotificationGroupDefinitionUpdateDto,
  ): Promise<NotificationGroupDefinitionDto> {
    return request<NotificationGroupDefinitionDto>(
      `/api/notifications/definitions/groups/${name}`,
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
