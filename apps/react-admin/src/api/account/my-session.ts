import type { IdentitySessionDto } from "#/management/identity/sessions";
import type { PagedResultDto } from "#/abp-core";
import requestClient from "@/api/request";

export interface GetMySessionsInput {
	filter?: string;
	maxResultCount?: number;
	skipCount?: number;
}

/**
 * Get current user's sessions
 */
export const getSessionsApi = (input?: GetMySessionsInput) =>
	requestClient.get<PagedResultDto<IdentitySessionDto>>("/api/account/my-profile/sessions", {
		params: input,
	});

/**
 * Revoke a session
 */
export const revokeSessionApi = (sessionId: string) =>
	requestClient.delete(`/api/account/my-profile/sessions/${sessionId}/revoke`);
