import type { PagedResultDto } from "#/abp-core";
import type { OpenIddictTokenDto, OpenIddictTokenGetListInput } from "#/openiddict/tokens";
import requestClient from "../request";

/**
 * 删除令牌
 * @param id 令牌id
 */
export function deleteApi(id: string): Promise<void> {
	return requestClient.delete(`/api/openiddict/tokens/${id}`);
}

/**
 * 获取令牌
 * @param id 令牌id
 * @returns 令牌实体数据传输对象
 */
export function getApi(id: string): Promise<OpenIddictTokenDto> {
	return requestClient.get<OpenIddictTokenDto>(`/api/openiddict/tokens/${id}`);
}

/**
 * 获取令牌分页列表
 * @param input 过滤参数
 * @returns 令牌实体数据传输对象分页列表
 */
export function getPagedListApi(input?: OpenIddictTokenGetListInput): Promise<PagedResultDto<OpenIddictTokenDto>> {
	return requestClient.get<PagedResultDto<OpenIddictTokenDto>>("/api/openiddict/tokens", {
		params: input,
	});
}
