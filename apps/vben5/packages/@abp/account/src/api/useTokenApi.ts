import type {
  OAuthTokenRefreshModel,
  OAuthTokenResult,
  PasswordTokenRequestModel,
  RevokeTokenRequest,
  TokenResult,
} from '../types';

import { useAppConfig } from '@vben/hooks';

import { useRequest } from '@abp/request';

export function useTokenApi() {
  const { cancel, request } = useRequest();
  /**
   * 用户登录
   * @param input 参数
   * @returns 用户token
   */
  async function loginApi(
    input: PasswordTokenRequestModel,
  ): Promise<TokenResult> {
    const { audience, clientId, clientSecret } = useAppConfig(
      import.meta.env,
      import.meta.env.PROD,
    );
    const result = await request<OAuthTokenResult>('/connect/token', {
      data: {
        client_id: clientId,
        client_secret: clientSecret,
        grant_type: 'password',
        scope: audience,
        ...input,
      },
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      method: 'POST',
    });
    return {
      accessToken: result.access_token,
      expiresIn: result.expires_in,
      refreshToken: result.refresh_token,
      tokenType: result.token_type,
    };
  }

  /**
   * 刷新令牌
   * @param input 参数
   * @returns 用户token
   */
  async function refreshTokenApi(input: OAuthTokenRefreshModel) {
    const { audience, clientId, clientSecret } = useAppConfig(
      import.meta.env,
      import.meta.env.PROD,
    );
    const result = await request<OAuthTokenResult>('/connect/token', {
      data: {
        client_id: clientId,
        client_secret: clientSecret,
        grant_type: 'refresh_token',
        refresh_token: input.refreshToken,
        scope: audience,
      },
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      method: 'POST',
    });
    return {
      accessToken: result.access_token,
      expiresIn: result.expires_in,
      refreshToken: result.refresh_token,
      tokenType: result.token_type,
    };
  }

  /**
   * 注销登录
   * @param input 参数
   */
  async function logoutApi(input: RevokeTokenRequest): Promise<void> {
    const { clientId, clientSecret } = useAppConfig(
      import.meta.env,
      import.meta.env.PROD,
    );
    return await request('/connect/revocat', {
      data: {
        client_id: clientId,
        client_secret: clientSecret,
        token: input.token,
        token_type_hint: input.tokenType,
      },
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      method: 'POST',
    });
  }

  return {
    cancel,
    loginApi,
    logoutApi,
    refreshTokenApi,
  };
}
