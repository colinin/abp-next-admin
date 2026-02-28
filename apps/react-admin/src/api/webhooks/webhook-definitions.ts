import type { ListResultDto } from "#/abp-core";

import type {
	WebhookDefinitionCreateDto,
	WebhookDefinitionDto,
	WebhookDefinitionGetListInput,
	WebhookDefinitionUpdateDto,
} from "#/webhooks/definitions";

import requestClient from "../request";

/**
 * 删除Webhook定义
 * @param name Webhook名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/webhooks/definitions/${name}`);
}

/**
 * 查询Webhook定义
 * @param name Webhook名称
 * @returns Webhook定义数据传输对象
 */
export function getApi(name: string): Promise<WebhookDefinitionDto> {
	return requestClient.get<WebhookDefinitionDto>(`/api/webhooks/definitions/${name}`);
}

/**
 * 查询Webhook定义列表
 * @param input Webhook过滤条件
 * @returns Webhook定义数据传输对象列表
 */
export function getListApi(input?: WebhookDefinitionGetListInput): Promise<ListResultDto<WebhookDefinitionDto>> {
	return requestClient.get<ListResultDto<WebhookDefinitionDto>>("/api/webhooks/definitions", {
		params: input,
	});
}

/**
 * 创建Webhook定义
 * @param input Webhook定义参数
 * @returns Webhook定义数据传输对象
 */
export function createApi(input: WebhookDefinitionCreateDto): Promise<WebhookDefinitionDto> {
	return requestClient.post<WebhookDefinitionDto>("/api/webhooks/definitions", input);
}

/**
 * 更新Webhook定义
 * @param name Webhook名称
 * @param input Webhook定义参数
 * @returns Webhook定义数据传输对象
 */
export function updateApi(name: string, input: WebhookDefinitionUpdateDto): Promise<WebhookDefinitionDto> {
	return requestClient.put<WebhookDefinitionDto>(`/api/webhooks/definitions/${name}`, input);
}
