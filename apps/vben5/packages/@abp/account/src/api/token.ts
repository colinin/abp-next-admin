import type {
  OAuthTokenResult,
  PasswordTokenRequestModel,
  TokenResult,
} from '../types';

import { useAppConfig } from '@vben/hooks';

import { requestClient } from '@abp/request';

/**
 * 用户登录
 * @param request 参数
 * @returns 用户token
 */
export async function loginApi(
  request: PasswordTokenRequestModel,
): Promise<TokenResult> {
  const { clientId, clientSecret } = useAppConfig(
    import.meta.env,
    import.meta.env.PROD,
  );
  const result = await requestClient.post<OAuthTokenResult>(
    '/connect/token',
    {
      client_id: clientId,
      client_secret: clientSecret,
      grant_type: 'password',
      password: request.password,
      scope:
        'openid email address phone profile offline_access lingyun-abp-application',
      username: request.username,
    },
    {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
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
