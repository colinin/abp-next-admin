import type { PagedResultDto } from "#/abp-core";
import type { GetUserSessionsInput, IdentitySessionDto } from "#/management/identity/sessions";
import requestClient from "@/api/request";

/**
 * 查询会话列表
 * @param { GetUserSessionsInput } input 查询参数
 * @returns { Promise<PagedResultDto<IdentitySessionDto>> } 用户会话列表
 */
export function getSessionsApi(input?: GetUserSessionsInput): Promise<PagedResultDto<IdentitySessionDto>> {
	return requestClient.get<PagedResultDto<IdentitySessionDto>>("/api/identity/sessions", {
		params: input,
	});
}
/**
 * 撤销会话
 * @param { string } sessionId 会话id
 * @returns { Promise<void> }
 */
export function revokeSessionApi(sessionId: string): Promise<void> {
	return requestClient.delete(`/api/identity/sessions/${sessionId}/revoke`);
}
