import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  WebhookAvailableGroupDto,
  WebhookSubscriptionCreateDto,
  WebhookSubscriptionDeleteManyInput,
  WebhookSubscriptionDto,
  WebhookSubscriptionGetListInput,
  WebhookSubscriptionUpdateDto,
} from '../types/subscriptions';

import { useRequest } from '@abp/request';

export function useSubscriptionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 创建订阅
   * @param input 参数
   * @returns 订阅Dto
   */
  function createApi(
    input: WebhookSubscriptionCreateDto,
  ): Promise<WebhookSubscriptionDto> {
    return request<WebhookSubscriptionDto>(`/api/webhooks/subscriptions`, {
      data: input,
      method: 'POST',
    });
  }
  /**
   * 删除订阅
   * @param id 订阅Id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/webhooks/subscriptions/${id}`, {
      method: 'DELETE',
    });
  }
  /**
   * 批量删除订阅
   * @param input 参数
   */
  function bulkDeleteApi(
    input: WebhookSubscriptionDeleteManyInput,
  ): Promise<void> {
    return request(`/api/webhooks/subscriptions/delete-many`, {
      data: input,
      method: 'DELETE',
    });
  }
  /**
   * 查询所有可用的Webhook分组列表
   * @returns Webhook分组列表
   */
  function getAllAvailableWebhooksApi(): Promise<
    ListResultDto<WebhookAvailableGroupDto>
  > {
    return request<ListResultDto<WebhookAvailableGroupDto>>(
      `/api/webhooks/subscriptions/availables`,
      {
        method: 'GET',
      },
    );
  }
  /**
   * 查询订阅
   * @param id 订阅Id
   * @returns 订阅Dto
   */
  function getApi(id: string): Promise<WebhookSubscriptionDto> {
    return request<WebhookSubscriptionDto>(
      `/api/webhooks/subscriptions/${id}`,
      {
        method: 'GET',
      },
    );
  }
  /**
   * 查询订阅分页列表
   * @param input 过滤参数
   * @returns 订阅Dto列表
   */
  function getPagedListApi(
    input: WebhookSubscriptionGetListInput,
  ): Promise<PagedResultDto<WebhookSubscriptionDto>> {
    return request<PagedResultDto<WebhookSubscriptionDto>>(
      `/api/webhooks/subscriptions`,
      {
        method: 'GET',
        params: input,
      },
    );
  }
  /**
   * 更新订阅
   * @param id 订阅Id
   * @param input 更新参数
   * @returns 订阅Dto
   */
  function updateApi(
    id: string,
    input: WebhookSubscriptionUpdateDto,
  ): Promise<WebhookSubscriptionDto> {
    return request<WebhookSubscriptionDto>(
      `/api/webhooks/subscriptions/${id}`,
      {
        data: input,
        method: 'PUT',
      },
    );
  }

  return {
    bulkDeleteApi,
    cancel,
    createApi,
    deleteApi,
    getAllAvailableWebhooksApi,
    getApi,
    getPagedListApi,
    updateApi,
  };
}
