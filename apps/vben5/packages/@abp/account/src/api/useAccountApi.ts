import type { ListResultDto } from '@abp/core';

import type {
  GetTwoFactorProvidersInput,
  PhoneResetPasswordDto,
  SendEmailSigninCodeDto,
  SendPhoneResetPasswordCodeDto,
  SendPhoneSigninCodeDto,
  TwoFactorProvider,
} from '../types/account';

import { useRequest } from '@abp/request';

export function useAccountApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取可用的二次认证验证器
   * @param input 参数
   */
  function getTwoFactorProvidersApi(
    input: GetTwoFactorProvidersInput,
  ): Promise<ListResultDto<TwoFactorProvider>> {
    return request<ListResultDto<TwoFactorProvider>>(
      '/api/account/two-factor-providers',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 发送登陆验证邮件
   * @param input 参数
   */
  function sendEmailSigninCodeApi(
    input: SendEmailSigninCodeDto,
  ): Promise<void> {
    return request('/api/account/email/send-signin-code', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 发送登陆验证短信
   * @param input 参数
   */
  function sendPhoneSigninCodeApi(
    input: SendPhoneSigninCodeDto,
  ): Promise<void> {
    return request('/api/account/phone/send-signin-code', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 发送重置密码验证短信
   * @param input 参数
   */
  function sendPhoneResetPasswordCodeApi(
    input: SendPhoneResetPasswordCodeDto,
  ): Promise<void> {
    return request('/api/account/phone/send-password-reset-code', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 重置密码
   * @param input 参数
   */
  function resetPasswordApi(input: PhoneResetPasswordDto): Promise<void> {
    return request('/api/account/phone/reset-password', {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    getTwoFactorProvidersApi,
    resetPasswordApi,
    sendEmailSigninCodeApi,
    sendPhoneResetPasswordCodeApi,
    sendPhoneSigninCodeApi,
  };
}
