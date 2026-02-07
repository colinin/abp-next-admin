import type { PagedResultDto } from "#/abp-core";

import type { LogDto, LogGetListInput } from "#/management/auditing";

import requestClient from "../../request";

/**
 * 获取系统日志
 * @param id 日志id
 */
export function getApi(id: string): Promise<LogDto> {
	return requestClient.get<LogDto>(`/api/auditing/logging/${id}`, {
		method: "GET",
	});
}

/**
 * 获取系统日志分页列表
 * @param input 参数
 */
export function getPagedListApi(input: LogGetListInput): Promise<PagedResultDto<LogDto>> {
	return requestClient.get<PagedResultDto<LogDto>>("/api/auditing/logging", {
		params: input,
	});
}
