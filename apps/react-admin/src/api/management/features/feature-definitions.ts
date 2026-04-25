import type { ListResultDto } from "#/abp-core";

import type {
	FeatureDefinitionCreateDto,
	FeatureDefinitionDto,
	FeatureDefinitionGetListInput,
	FeatureDefinitionUpdateDto,
} from "#/management/features/definitions";

import requestClient from "../../request";

/**
 * 删除功能定义
 * @param name 功能名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/feature-management/definitions/${name}`);
}

/**
 * 查询功能定义
 * @param name 功能名称
 * @returns 功能定义数据传输对象
 */
export function getApi(name: string): Promise<FeatureDefinitionDto> {
	return requestClient.get<FeatureDefinitionDto>(`/api/feature-management/definitions/${name}`);
}

/**
 * 查询功能定义列表
 * @param input 功能过滤条件
 * @returns 功能定义数据传输对象列表
 */
export function getListApi(input?: FeatureDefinitionGetListInput): Promise<ListResultDto<FeatureDefinitionDto>> {
	return requestClient.get<ListResultDto<FeatureDefinitionDto>>("/api/feature-management/definitions", {
		params: input,
	});
}

/**
 * 创建功能定义
 * @param input 功能定义参数
 * @returns 功能定义数据传输对象
 */
export function createApi(input: FeatureDefinitionCreateDto): Promise<FeatureDefinitionDto> {
	return requestClient.post<FeatureDefinitionDto>("/api/feature-management/definitions", input);
}

/**
 * 更新功能定义
 * @param name 功能名称
 * @param input 功能定义参数
 * @returns 功能定义数据传输对象
 */
export function updateApi(name: string, input: FeatureDefinitionUpdateDto): Promise<FeatureDefinitionDto> {
	return requestClient.put<FeatureDefinitionDto>(`/api/feature-management/definitions/${name}`, input);
}
