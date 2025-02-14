import type { ListResultDto } from '@abp/core';

import type {
  GetTwoFactorProvidersInput,
  SendEmailSigninCodeDto,
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

  return {
    cancel,
    getTwoFactorProvidersApi,
    sendEmailSigninCodeApi,
    sendPhoneSigninCodeApi,
  };
}
