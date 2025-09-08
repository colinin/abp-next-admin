import type {
  ExternalLoginResultDto,
  RemoveExternalLoginInput,
  WorkWeixinLoginBindInput,
} from '../types/external-logins';

import { useRequest } from '@abp/request';

export function useExternalLoginsApi() {
  const { cancel, request } = useRequest();

  /**
   * 绑定企业微信
   * @param input 绑定参数
   * @returns { Promise<void> }
   */
  async function bindWorkWeixinApi(
    input: WorkWeixinLoginBindInput,
  ): Promise<void> {
    return await request(`/api/account/oauth/work-weixin/bind`, {
      method: 'POST',
      data: input,
    });
  }

  /**
   * 获取外部登录提供者列表
   * @returns 外部登录提供者列表
   */
  async function getExternalLoginsApi(): Promise<ExternalLoginResultDto> {
    return await request<ExternalLoginResultDto>(
      `/api/account/external-logins`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 移除外部登录提供者
   * @returns { Promise<void> }
   */
  async function removeExternalLoginApi(
    input: RemoveExternalLoginInput,
  ): Promise<void> {
    return await request(`/api/account/external-logins/remove`, {
      method: 'DELETE',
      params: input,
    });
  }

  return {
    cancel,
    bindWorkWeixinApi,
    getExternalLoginsApi,
    removeExternalLoginApi,
  };
}
