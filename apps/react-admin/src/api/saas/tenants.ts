import type { ListResultDto, PagedResultDto } from "#/abp-core";

import type {
	GetTenantPagedListInput,
	TenantConnectionStringCheckInput,
	TenantConnectionStringDto,
	TenantConnectionStringSetInput,
	TenantCreateDto,
	TenantDto,
	TenantUpdateDto,
} from "#/saas/tenants";

import requestClient from "../request";

/**
 * 创建租户
 * @param {TenantCreateDto} input 参数
 * @returns 创建的租户
 */
export function createApi(input: TenantCreateDto): Promise<TenantDto> {
	return requestClient.post<TenantDto>("/api/saas/tenants", input);
}

/**
 * 编辑租户
 * @param {string} id 参数
 * @param {TenantUpdateDto} input 参数
 * @returns 编辑的租户
 */
export function updateApi(id: string, input: TenantUpdateDto): Promise<TenantDto> {
	return requestClient.put<TenantDto>(`/api/saas/tenants/${id}`, input);
}

/**
 * 查询租户
 * @param {string} id Id
 * @returns 查询的租户
 */
export function getApi(id: string): Promise<TenantDto> {
	return requestClient.get<TenantDto>(`/api/saas/tenants/${id}`);
}

/**
 * 删除租户
 * @param {string} id Id
 * @returns {void}
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/saas/tenants/${id}`);
}

/**
 * 查询租户分页列表
 * @param {GetTenantPagedListInput} input 参数
 * @returns {void}
 */
export function getPagedListApi(input?: GetTenantPagedListInput): Promise<PagedResultDto<TenantDto>> {
	return requestClient.get<PagedResultDto<TenantDto>>("/api/saas/tenants", {
		params: input,
	});
}

/**
 * 设置连接字符串
 * @param {string} id 租户Id
 * @param {TenantConnectionStringSetInput} input 参数
 * @returns 连接字符串
 */
export function setConnectionStringApi(
	id: string,
	input: TenantConnectionStringSetInput,
): Promise<TenantConnectionStringDto> {
	return requestClient.put<TenantConnectionStringDto>(`/api/saas/tenants/${id}/connection-string`, input);
}

/**
 * 查询连接字符串
 * @param {string} id 租户Id
 * @param {string} name 连接字符串名称
 * @returns 连接字符串
 */
export function getConnectionStringApi(id: string, name: string): Promise<TenantConnectionStringDto> {
	return requestClient.get<TenantConnectionStringDto>(`/api/saas/tenants/${id}/connection-string/${name}`);
}

/**
 * 查询所有连接字符串
 * @param {string} id 租户Id
 * @returns 连接字符串列表
 */
export function getConnectionStringsApi(id: string): Promise<ListResultDto<TenantConnectionStringDto>> {
	return requestClient.get<ListResultDto<TenantConnectionStringDto>>(`/api/saas/tenants/${id}/connection-string`);
}

/**
 * 删除租户
 * @param {string} id 租户Id
 * @param {string} name 连接字符串名称
 * @returns {void}
 */
export function deleteConnectionStringApi(id: string, name: string): Promise<void> {
	return requestClient.delete(`/api/saas/tenants/${id}/connection-string/${name}`);
}

/**
 * 检查数据库连接字符串
 * @param input 参数
 */
export function checkConnectionString(input: TenantConnectionStringCheckInput): Promise<void> {
	return requestClient.post("/api/saas/tenants/connection-string/check", input);
}
