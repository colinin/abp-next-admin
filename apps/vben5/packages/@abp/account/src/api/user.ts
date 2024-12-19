import type { OAuthUserInfo, UserInfo } from '../types/user';

import { requestClient } from '@abp/request';

/**
 * 获取用户信息
 */
export async function getUserInfoApi(): Promise<UserInfo> {
  const result = await requestClient.get<OAuthUserInfo>('/connect/userinfo');
  return {
    ...result,
    emailVerified: result.email_verified,
    givenName: result.given_name,
    phoneNumberVerified: result.phone_number_verified,
    preferredUsername: result.preferred_username,
    uniqueName: result.unique_name,
  };
}
