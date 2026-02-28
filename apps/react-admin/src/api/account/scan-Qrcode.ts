import type { GenerateQrCodeResult, QrCodeUserInfoResult } from "#/account/qrcode";

import requestClient from "@/api/request";

/**
 * 生成登录二维码
 * @returns 二维码信息
 */
export function generateApi(): Promise<GenerateQrCodeResult> {
	return requestClient.post<GenerateQrCodeResult>("/api/account/qrcode/generate");
}

/**
 * 检查二维码状态
 * @param key 二维码Key
 * @returns 二维码信息
 */
export function checkCodeApi(key: string): Promise<QrCodeUserInfoResult> {
	return requestClient.get<QrCodeUserInfoResult>(`/api/account/qrcode/${key}/check`);
}
