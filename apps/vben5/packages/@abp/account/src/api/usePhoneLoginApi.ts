import type { OAuthTokenResult, PhoneNumberTokenRequest } from '../types/token';

import { useAppConfig } from '@vben/hooks';

import { useRequest } from '@abp/request';

export function usePhoneLoginApi() {
  const { cancel, request } = useRequest();

  /**
   * 手机验证码登录
   * @param input 登录参数
   * @returns 用户token
   */
  async function loginApi(input: PhoneNumberTokenRequest) {
    const { audience, clientId, clientSecret } = useAppConfig(
      import.meta.env,
      import.meta.env.PROD,
    );
    const result = await request<OAuthTokenResult>('/connect/token', {
      data: {
        client_id: clientId,
        client_secret: clientSecret,
        grant_type: 'phone_verify',
        phone_number: input.phoneNumber,
        phone_verify_code: input.code,
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

  return {
    cancel,
    loginApi,
  };
}
