import type { ListResultDto } from "#/abp-core";

import type {
	WebhookGroupDefinitionCreateDto,
	WebhookGroupDefinitionDto,
	WebhookGroupDefinitionGetListInput,
	WebhookGroupDefinitionUpdateDto,
} from "#/webhooks/groups";

import requestClient from "../request";

/**
 * 删除Webhook分组定义
 * @param name Webhook分组名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/webhooks/definitions/groups/${name}`);
}

/**
 * 查询Webhook分组定义
 * @param name Webhook分组名称
 * @returns Webhook分组定义数据传输对象
 */
export function getApi(name: string): Promise<WebhookGroupDefinitionDto> {
	return requestClient.get<WebhookGroupDefinitionDto>(`/api/webhooks/definitions/groups/${name}`);
}

/**
 * 查询Webhook分组定义列表
 * @param input Webhook分组过滤条件
 * @returns Webhook分组定义数据传输对象列表
 */
export function getListApi(
	input?: WebhookGroupDefinitionGetListInput,
): Promise<ListResultDto<WebhookGroupDefinitionDto>> {
	return requestClient.get<ListResultDto<WebhookGroupDefinitionDto>>("/api/webhooks/definitions/groups", {
		params: input,
	});
}

/**
 * 创建Webhook分组定义
 * @param input Webhook分组定义参数
 * @returns Webhook分组定义数据传输对象
 */
export function createApi(input: WebhookGroupDefinitionCreateDto): Promise<WebhookGroupDefinitionDto> {
	return requestClient.post<WebhookGroupDefinitionDto>("/api/webhooks/definitions/groups", input);
}

/**
 * 更新Webhook分组定义
 * @param name Webhook分组名称
 * @param input Webhook分组定义参数
 * @returns Webhook分组定义数据传输对象
 */
export function updateApi(name: string, input: WebhookGroupDefinitionUpdateDto): Promise<WebhookGroupDefinitionDto> {
	return requestClient.put<WebhookGroupDefinitionDto>(`/api/webhooks/definitions/groups/${name}`, input);
}
