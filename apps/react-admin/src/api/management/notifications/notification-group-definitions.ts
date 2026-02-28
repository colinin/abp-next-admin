import type { ListResultDto } from "#/abp-core";

import type {
	NotificationGroupDefinitionCreateDto,
	NotificationGroupDefinitionDto,
	NotificationGroupDefinitionGetListInput,
	NotificationGroupDefinitionUpdateDto,
} from "#/notifications/groups";

import requestClient from "../../request";

/**
 * 删除通知分组定义
 * @param name 通知分组名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/notifications/definitions/groups/${name}`);
}

/**
 * 查询通知分组定义
 * @param name 通知分组名称
 * @returns 通知分组定义数据传输对象
 */
export function getApi(name: string): Promise<NotificationGroupDefinitionDto> {
	return requestClient.get<NotificationGroupDefinitionDto>(`/api/notifications/definitions/groups/${name}`);
}

/**
 * 查询通知分组定义列表
 * @param input 通知分组过滤条件
 * @returns 通知分组定义数据传输对象列表
 */
export function getListApi(
	input?: NotificationGroupDefinitionGetListInput,
): Promise<ListResultDto<NotificationGroupDefinitionDto>> {
	return requestClient.get<ListResultDto<NotificationGroupDefinitionDto>>("/api/notifications/definitions/groups", {
		params: input,
	});
}

/**
 * 创建通知分组定义
 * @param input 通知分组定义参数
 * @returns 通知分组定义数据传输对象
 */
export function createApi(input: NotificationGroupDefinitionCreateDto): Promise<NotificationGroupDefinitionDto> {
	return requestClient.post<NotificationGroupDefinitionDto>("/api/notifications/definitions/groups", input);
}

/**
 * 更新通知分组定义
 * @param name 通知分组名称
 * @param input 通知分组定义参数
 * @returns 通知分组定义数据传输对象
 */
export function updateApi(
	name: string,
	input: NotificationGroupDefinitionUpdateDto,
): Promise<NotificationGroupDefinitionDto> {
	return requestClient.put<NotificationGroupDefinitionDto>(`/api/notifications/definitions/groups/${name}`, input);
}
