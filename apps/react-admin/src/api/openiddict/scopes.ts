import type { PagedResultDto } from "#/abp-core";
import type {
	OpenIddictScopeCreateDto,
	OpenIddictScopeDto,
	OpenIddictScopeGetListInput,
	OpenIddictScopeUpdateDto,
} from "#/openiddict/scopes";

import requestClient from "../request";

/**
 * 创建范围
 * @param input 参数
 * @returns 范围实体数据传输对象
 */
export function createApi(input: OpenIddictScopeCreateDto): Promise<OpenIddictScopeDto> {
	return requestClient.post<OpenIddictScopeDto>("/api/openiddict/scopes", input);
}

/**
 * 删除范围
 * @param id 范围id
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/openiddict/scopes/${id}`);
}

/**
 * 获取范围
 * @param id 范围id
 * @returns 范围实体数据传输对象
 */
export function getApi(id: string): Promise<OpenIddictScopeDto> {
	return requestClient.get<OpenIddictScopeDto>(`/api/openiddict/scopes/${id}`);
}

/**
 * 更新范围
 * @param id 范围id
 * @param input 更新参数
 * @returns 范围实体数据传输对象
 */
export function updateApi(id: string, input: OpenIddictScopeUpdateDto): Promise<OpenIddictScopeDto> {
	return requestClient.put<OpenIddictScopeDto>(`/api/openiddict/scopes/${id}`, input);
}

/**
 * 获取范围分页列表
 * @param input 过滤参数
 * @returns 范围实体数据传输对象分页列表
 */
export function getPagedListApi(input?: OpenIddictScopeGetListInput): Promise<PagedResultDto<OpenIddictScopeDto>> {
	return requestClient.get<PagedResultDto<OpenIddictScopeDto>>("/api/openiddict/scopes", {
		params: input,
	});
}
