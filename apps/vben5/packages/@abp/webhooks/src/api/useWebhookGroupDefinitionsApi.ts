import type { ListResultDto } from '@abp/core';

import type {
  WebhookGroupDefinitionCreateDto,
  WebhookGroupDefinitionDto,
  WebhookGroupDefinitionGetListInput,
  WebhookGroupDefinitionUpdateDto,
} from '../types/groups';

import { useRequest } from '@abp/request';

export function useWebhookGroupDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除Webhook分组定义
   * @param name Webhook分组名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/webhooks/definitions/groups/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询Webhook分组定义
   * @param name Webhook分组名称
   * @returns Webhook分组定义数据传输对象
   */
  function getApi(name: string): Promise<WebhookGroupDefinitionDto> {
    return request<WebhookGroupDefinitionDto>(
      `/api/webhooks/definitions/groups/${name}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 查询Webhook分组定义列表
   * @param input Webhook分组过滤条件
   * @returns Webhook分组定义数据传输对象列表
   */
  function getListApi(
    input?: WebhookGroupDefinitionGetListInput,
  ): Promise<ListResultDto<WebhookGroupDefinitionDto>> {
    return request<ListResultDto<WebhookGroupDefinitionDto>>(
      `/api/webhooks/definitions/groups`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 创建Webhook分组定义
   * @param input Webhook分组定义参数
   * @returns Webhook分组定义数据传输对象
   */
  function createApi(
    input: WebhookGroupDefinitionCreateDto,
  ): Promise<WebhookGroupDefinitionDto> {
    return request<WebhookGroupDefinitionDto>(
      '/api/webhooks/definitions/groups',
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 更新Webhook分组定义
   * @param name Webhook分组名称
   * @param input Webhook分组定义参数
   * @returns Webhook分组定义数据传输对象
   */
  function updateApi(
    name: string,
    input: WebhookGroupDefinitionUpdateDto,
  ): Promise<WebhookGroupDefinitionDto> {
    return request<WebhookGroupDefinitionDto>(
      `/api/webhooks/definitions/groups/${name}`,
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
