import type { ListResultDto } from "#/abp-core";

import type {
	FeatureGroupDefinitionCreateDto,
	FeatureGroupDefinitionDto,
	FeatureGroupDefinitionGetListInput,
	FeatureGroupDefinitionUpdateDto,
} from "#/management/features/groups";

import requestClient from "../../request";

/**
 * 删除功能定义
 * @param name 功能名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/feature-management/definitions/groups/${name}`);
}

/**
 * 查询功能定义
 * @param name 功能名称
 * @returns 功能定义数据传输对象
 */
export function getApi(name: string): Promise<FeatureGroupDefinitionDto> {
	return requestClient.get<FeatureGroupDefinitionDto>(`/api/feature-management/definitions/groups/${name}`);
}

/**
 * 查询功能定义列表
 * @param input 功能过滤条件
 * @returns 功能定义数据传输对象列表
 */
export function getListApi(
	input?: FeatureGroupDefinitionGetListInput,
): Promise<ListResultDto<FeatureGroupDefinitionDto>> {
	return requestClient.get<ListResultDto<FeatureGroupDefinitionDto>>("/api/feature-management/definitions/groups", {
		params: input,
	});
}

/**
 * 创建功能定义
 * @param input 功能定义参数
 * @returns 功能定义数据传输对象
 */
export function createApi(input: FeatureGroupDefinitionCreateDto): Promise<FeatureGroupDefinitionDto> {
	return requestClient.post<FeatureGroupDefinitionDto>("/api/feature-management/definitions/groups", input);
}

/**
 * 更新功能定义
 * @param name 功能名称
 * @param input 功能定义参数
 * @returns 功能定义数据传输对象
 */
export function updateApi(name: string, input: FeatureGroupDefinitionUpdateDto): Promise<FeatureGroupDefinitionDto> {
	return requestClient.put<FeatureGroupDefinitionDto>(`/api/feature-management/definitions/groups/${name}`, input);
}
