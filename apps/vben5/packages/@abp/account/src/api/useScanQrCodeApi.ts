import type {
  GenerateQrCodeResult,
  QrCodeUserInfoResult,
} from '../types/qrcode';

import { useRequest } from '@abp/request';

export function useScanQrCodeApi() {
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

  return {
    cancel,
    checkCodeApi,
    generateApi,
  };
}
