import type { OAuthUserInfo, UserInfo } from '../types/user';

import { useRequest } from '@abp/request';

export function useUserInfoApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取用户信息
   */
  async function getUserInfoApi(): Promise<UserInfo> {
    const result = await request<OAuthUserInfo>('/connect/userinfo', {
      method: 'GET',
    });
    return {
      ...result,
      emailVerified: result.email_verified,
      givenName: result.given_name,
      phoneNumberVerified: result.phone_number_verified,
      preferredUsername: result.preferred_username,
      uniqueName: result.unique_name,
    };
  }

  return {
    cancel,
    getUserInfoApi,
  };
}
