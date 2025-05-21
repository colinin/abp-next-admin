import type { ListResultDto } from '@abp/core';

import type {
  WebhookDefinitionCreateDto,
  WebhookDefinitionDto,
  WebhookDefinitionGetListInput,
  WebhookDefinitionUpdateDto,
} from '../types/definitions';

import { useRequest } from '@abp/request';

export function useWebhookDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除Webhook定义
   * @param name Webhook名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/webhooks/definitions/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询Webhook定义
   * @param name Webhook名称
   * @returns Webhook定义数据传输对象
   */
  function getApi(name: string): Promise<WebhookDefinitionDto> {
    return request<WebhookDefinitionDto>(`/api/webhooks/definitions/${name}`, {
      method: 'GET',
    });
  }

  /**
   * 查询Webhook定义列表
   * @param input Webhook过滤条件
   * @returns Webhook定义数据传输对象列表
   */
  function getListApi(
    input?: WebhookDefinitionGetListInput,
  ): Promise<ListResultDto<WebhookDefinitionDto>> {
    return request<ListResultDto<WebhookDefinitionDto>>(
      `/api/webhooks/definitions`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 创建Webhook定义
   * @param input Webhook定义参数
   * @returns Webhook定义数据传输对象
   */
  function createApi(
    input: WebhookDefinitionCreateDto,
  ): Promise<WebhookDefinitionDto> {
    return request<WebhookDefinitionDto>('/api/webhooks/definitions', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 更新Webhook定义
   * @param name Webhook名称
   * @param input Webhook定义参数
   * @returns Webhook定义数据传输对象
   */
  function updateApi(
    name: string,
    input: WebhookDefinitionUpdateDto,
  ): Promise<WebhookDefinitionDto> {
    return request<WebhookDefinitionDto>(`/api/webhooks/definitions/${name}`, {
      data: input,
      method: 'PUT',
    });
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
