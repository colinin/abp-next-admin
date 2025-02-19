import type { GetSecurityLogPagedRequest, SecurityLogDto } from "#/management/identity";
import type { PagedResultDto } from "#/abp-core";
import requestClient from "../../request";

/**
 * 删除安全日志
 * @param id 安全日志id
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/auditing/security-log/${id}`);
}

/**
 * 查询安全日志
 * @param id 安全日志id
 * @returns 安全日志实体数据传输对象
 */
export function getApi(id: string): Promise<SecurityLogDto> {
	return requestClient.get<SecurityLogDto>(`/api/auditing/security-log/${id}`);
}

/**
 * 查询安全日志分页列表
 * @param input 过滤参数
 * @returns 安全日志实体数据传输对象分页列表
 */
export function getPagedListApi(input?: GetSecurityLogPagedRequest): Promise<PagedResultDto<SecurityLogDto>> {
	return requestClient.get<PagedResultDto<SecurityLogDto>>("/api/auditing/security-log", {
		params: input,
	});
}
