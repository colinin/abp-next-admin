import type { ListResultDto, PagedResultDto } from "#/abp-core";

import type {
	WebhookAvailableGroupDto,
	WebhookSubscriptionCreateDto,
	WebhookSubscriptionDeleteManyInput,
	WebhookSubscriptionDto,
	WebhookSubscriptionGetListInput,
	WebhookSubscriptionUpdateDto,
} from "#/webhooks/subscriptions";

import requestClient from "../request";

/**
 * 创建订阅
 * @param input 参数
 * @returns 订阅Dto
 */
export function createApi(input: WebhookSubscriptionCreateDto): Promise<WebhookSubscriptionDto> {
	return requestClient.post<WebhookSubscriptionDto>("/api/webhooks/subscriptions", input);
}

/**
 * 删除订阅
 * @param id 订阅Id
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/webhooks/subscriptions/${id}`);
}

/**
 * 批量删除订阅
 * @param input 参数
 */
export function bulkDeleteApi(input: WebhookSubscriptionDeleteManyInput): Promise<void> {
	return requestClient.delete("/api/webhooks/subscriptions/delete-many", {
		data: input,
	});
}

/**
 * 查询所有可用的Webhook分组列表
 * @returns Webhook分组列表
 */
export function getAllAvailableWebhooksApi(): Promise<ListResultDto<WebhookAvailableGroupDto>> {
	return requestClient.get<ListResultDto<WebhookAvailableGroupDto>>("/api/webhooks/subscriptions/availables");
}

/**
 * 查询订阅
 * @param id 订阅Id
 * @returns 订阅Dto
 */
export function getApi(id: string): Promise<WebhookSubscriptionDto> {
	return requestClient.get<WebhookSubscriptionDto>(`/api/webhooks/subscriptions/${id}`);
}

/**
 * 查询订阅分页列表
 * @param input 过滤参数
 * @returns 订阅Dto列表
 */
export function getPagedListApi(
	input: WebhookSubscriptionGetListInput,
): Promise<PagedResultDto<WebhookSubscriptionDto>> {
	return requestClient.get<PagedResultDto<WebhookSubscriptionDto>>("/api/webhooks/subscriptions", {
		params: input,
	});
}

/**
 * 更新订阅
 * @param id 订阅Id
 * @param input 更新参数
 * @returns 订阅Dto
 */
export function updateApi(id: string, input: WebhookSubscriptionUpdateDto): Promise<WebhookSubscriptionDto> {
	return requestClient.put<WebhookSubscriptionDto>(`/api/webhooks/subscriptions/${id}`, input);
}
