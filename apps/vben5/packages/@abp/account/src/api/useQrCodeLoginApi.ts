import type {
  GenerateQrCodeResult,
  QrCodeUserInfoResult,
} from '../types/qrcode';
import type { OAuthTokenResult, QrCodeTokenRequest } from '../types/token';

import { useAppConfig } from '@vben/hooks';

import { useRequest } from '@abp/request';

export function useQrCodeLoginApi() {
  const { cancel, request } = useRequest();

  /**
   * 生成登录二维码
   * @returns 二维码信息
   */
  function generateApi(): Promise<GenerateQrCodeResult> {
    return request<GenerateQrCodeResult>('/api/account/qrcode/generate', {
      method: 'POST',
    });
  }

  /**
   * 检查二维码状态
   * @param key 二维码Key
   * @returns 二维码信息
   */
  function checkCodeApi(key: string): Promise<QrCodeUserInfoResult> {
    return request<QrCodeUserInfoResult>(`/api/account/qrcode/${key}/check`, {
      method: 'GET',
    });
  }

  /**
   * 二维码登录
   * @param key 二维码Key
   * @returns 用户token
   */
  async function loginApi(input: QrCodeTokenRequest) {
    const { audience, clientId, clientSecret } = useAppConfig(
      import.meta.env,
      import.meta.env.PROD,
    );
    const result = await request<OAuthTokenResult>('/connect/token', {
      data: {
        client_id: clientId,
        client_secret: clientSecret,
        grant_type: 'qr_code',
        qrcode_key: input.key,
        scope: audience,
        tenant_id: input.tenantId,
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
    checkCodeApi,
    generateApi,
    loginApi,
  };
}
