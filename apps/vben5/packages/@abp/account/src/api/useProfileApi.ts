import type {
  AuthenticatorDto,
  AuthenticatorRecoveryCodeDto,
  ChangePasswordInput,
  ProfileDto,
  TwoFactorEnabledDto,
  UpdateProfileDto,
  VerifyAuthenticatorCodeInput,
} from '../types/profile';

import { useRequest } from '@abp/request';

export function useProfileApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询个人信息
   * @returns 个人信息
   */
  function getApi(): Promise<ProfileDto> {
    return request<ProfileDto>('/api/account/my-profile', {
      method: 'GET',
    });
  }

  /**
   * 更新个人信息
   * @param input 参数
   * @returns 个人信息
   */
  function updateApi(input: UpdateProfileDto): Promise<ProfileDto> {
    return request<ProfileDto>('/api/account/my-profile', {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 修改密码
   * @param input 参数
   */
  function changePasswordApi(input: ChangePasswordInput): Promise<void> {
    return request('/api/account/my-profile/change-password', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 获取二次认证启用状态
   */
  function getTwoFactorEnabledApi(): Promise<TwoFactorEnabledDto> {
    return request<TwoFactorEnabledDto>('/api/account/my-profile/two-factor', {
      method: 'GET',
    });
  }

  /**
   * 设置二次认证启用状态
   */
  function changeTwoFactorEnabledApi(
    input: TwoFactorEnabledDto,
  ): Promise<void> {
    return request('/api/account/my-profile/change-two-factor', {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 获取身份验证器配置
   * @returns 身份验证器
   */
  function getAuthenticatorApi(): Promise<AuthenticatorDto> {
    return request<AuthenticatorDto>('/api/account/my-profile/authenticator', {
      method: 'GET',
    });
  }

  /**
   * 验证身份验证代码
   * @param input 参数
   * @returns 重置代码
   */
  function verifyAuthenticatorCodeApi(
    input: VerifyAuthenticatorCodeInput,
  ): Promise<AuthenticatorRecoveryCodeDto> {
    return request<AuthenticatorRecoveryCodeDto>(
      '/api/account/my-profile/verify-authenticator-code',
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 重置验证器
   */
  function resetAuthenticatorApi(): Promise<void> {
    return request('/api/account/my-profile/reset-authenticator', {
      method: 'POST',
    });
  }

  return {
    cancel,
    changePasswordApi,
    changeTwoFactorEnabledApi,
    getApi,
    getAuthenticatorApi,
    getTwoFactorEnabledApi,
    resetAuthenticatorApi,
    updateApi,
    verifyAuthenticatorCodeApi,
  };
}
