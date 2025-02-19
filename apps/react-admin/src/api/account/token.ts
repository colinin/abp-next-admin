import type { OAuthTokenResult, PasswordTokenRequestModel, RefreshTokenRequestModel, TokenResult } from "#/account";
import requestClient from "../request";

/**
 * 用户登录
 * @param request 参数
 * @returns 用户token
 */
export async function loginApi(request: PasswordTokenRequestModel): Promise<TokenResult> {
	const clientId = import.meta.env.VITE_GLOB_CLIENT_ID;
	const clientSecret = import.meta.env.VITE_GLOB_CLIENT_SECRET;
	const scope = import.meta.env.VITE_GLOB_SCOPE;
	const result = await requestClient.post<OAuthTokenResult>(
		"/connect/token",
		{
			client_id: clientId,
			client_secret: clientSecret,
			grant_type: "password",
			password: request.password,
			scope: scope,
			username: request.username,
		},
		{
			headers: {
				"Content-Type": "application/x-www-form-urlencoded",
			},
			timeout: 30_000,
		},
	);
	return {
		accessToken: result.access_token,
		expiresIn: result.expires_in,
		refreshToken: result.refresh_token,
		tokenType: result.token_type,
	};
}

export async function refreshToken(request: RefreshTokenRequestModel): Promise<TokenResult> {
	const clientId = import.meta.env.VITE_GLOB_CLIENT_ID;
	const clientSecret = import.meta.env.VITE_GLOB_CLIENT_SECRET;
	const scope = import.meta.env.VITE_GLOB_SCOPE;
	const result = await requestClient.post<OAuthTokenResult>(
		"/connect/token",
		{
			client_id: clientId,
			client_secret: clientSecret,
			grant_type: "refresh_token",
			refresh_token: request.refreshToken,
			scope: scope,
		},
		{
			headers: {
				"Content-Type": "application/x-www-form-urlencoded",
			},
		},
	);
	return {
		accessToken: result.access_token,
		expiresIn: result.expires_in,
		refreshToken: result.refresh_token,
		tokenType: result.token_type,
	};
}
