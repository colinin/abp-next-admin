import type { ListResultDto } from "#/abp-core";

import type {
	NotificationDefinitionCreateDto,
	NotificationDefinitionDto,
	NotificationDefinitionGetListInput,
	NotificationDefinitionUpdateDto,
} from "#/notifications/definitions";

import requestClient from "../../request";

/**
 * 删除通知定义
 * @param name 通知名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/notifications/definitions/notifications/${name}`);
}

/**
 * 查询通知定义
 * @param name 通知名称
 * @returns 通知定义数据传输对象
 */
export function getApi(name: string): Promise<NotificationDefinitionDto> {
	return requestClient.get<NotificationDefinitionDto>(`/api/notifications/definitions/notifications/${name}`);
}

/**
 * 查询通知定义列表
 * @param input 通知过滤条件
 * @returns 通知定义数据传输对象列表
 */
export function getListApi(
	input?: NotificationDefinitionGetListInput,
): Promise<ListResultDto<NotificationDefinitionDto>> {
	return requestClient.get<ListResultDto<NotificationDefinitionDto>>("/api/notifications/definitions/notifications", {
		params: input,
	});
}

/**
 * 创建通知定义
 * @param input 通知定义参数
 * @returns 通知定义数据传输对象
 */
export function createApi(input: NotificationDefinitionCreateDto): Promise<NotificationDefinitionDto> {
	return requestClient.post<NotificationDefinitionDto>("/api/notifications/definitions/notifications", input);
}

/**
 * 更新通知定义
 * @param name 通知名称
 * @param input 通知定义参数
 * @returns 通知定义数据传输对象
 */
export function updateApi(name: string, input: NotificationDefinitionUpdateDto): Promise<NotificationDefinitionDto> {
	return requestClient.put<NotificationDefinitionDto>(`/api/notifications/definitions/notifications/${name}`, input);
}
