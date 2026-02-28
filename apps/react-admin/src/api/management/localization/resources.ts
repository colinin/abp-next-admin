import type { ListResultDto } from "#/abp-core";

import type {
	ResourceCreateDto,
	ResourceDto,
	ResourceGetListInput,
	ResourceUpdateDto,
} from "#/management/localization/resources";

import requestClient from "../../request";

/**
 * 查询资源列表
 * @param input 参数
 * @returns 资源列表
 */
export function getListApi(input?: ResourceGetListInput): Promise<ListResultDto<ResourceDto>> {
	return requestClient.get<ListResultDto<ResourceDto>>("/api/abp/localization/resources", {
		params: input,
	});
}

/**
 * 查询资源
 * @param name 资源名称
 * @returns 查询的资源
 */
export function getApi(name: string): Promise<ResourceDto> {
	return requestClient.get<ResourceDto>(`/api/localization/resources/${name}`);
}

/**
 * 删除资源
 * @param name 资源名称
 */
export function deleteApi(name: string): Promise<void> {
	return requestClient.delete(`/api/localization/resources/${name}`);
}

/**
 * 创建资源
 * @param input 参数
 * @returns 创建的资源
 */
export function createApi(input: ResourceCreateDto): Promise<ResourceDto> {
	return requestClient.post<ResourceDto>("/api/localization/resources", input);
}

/**
 * 编辑资源
 * @param name 资源名称
 * @param input 参数
 * @returns 编辑的资源
 */
export function updateApi(name: string, input: ResourceUpdateDto): Promise<ResourceDto> {
	return requestClient.put<ResourceDto>(`/api/localization/resources/${name}`, input);
}
