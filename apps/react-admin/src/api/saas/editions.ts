import type { PagedResultDto } from "#/abp-core";

import type { EditionCreateDto, EditionDto, EditionUpdateDto, GetEditionPagedListInput } from "#/saas/editions";

import requestClient from "../request";

/**
 * 创建版本
 * @param {EditionCreateDto} input 参数
 * @returns 创建的版本
 */
export function createApi(input: EditionCreateDto): Promise<EditionDto> {
	return requestClient.post<EditionDto>("/api/saas/editions", input);
}

/**
 * 编辑版本
 * @param {string} id 参数
 * @param {EditionUpdateDto} input 参数
 * @returns 编辑的版本
 */
export function updateApi(id: string, input: EditionUpdateDto): Promise<EditionDto> {
	return requestClient.put<EditionDto>(`/api/saas/editions/${id}`, input);
}

/**
 * 查询版本
 * @param {string} id Id
 * @returns 查询的版本
 */
export function getApi(id: string): Promise<EditionDto> {
	return requestClient.get<EditionDto>(`/api/saas/editions/${id}`);
}

/**
 * 删除版本
 * @param {string} id Id
 * @returns {void}
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/saas/editions/${id}`);
}

/**
 * 查询版本分页列表
 * @param {GetEditionPagedListInput} input 参数
 * @returns {void}
 */
export function getPagedListApi(input?: GetEditionPagedListInput): Promise<PagedResultDto<EditionDto>> {
	return requestClient.get<PagedResultDto<EditionDto>>("/api/saas/editions", {
		params: input,
	});
}
