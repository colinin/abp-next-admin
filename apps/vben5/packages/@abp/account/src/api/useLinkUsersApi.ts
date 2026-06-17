import type { ListResultDto } from '@abp/core';

import type { LinkUserDto, UnLinkUserInput } from '../types/link-user';

import { useRequest } from '@abp/request';

export function useLinkUsersApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询关联用户列表
   * @returns { Promise<ListResultDto<LinkUserDto>> } 关联用户列表
   */
  function getListApi(): Promise<ListResultDto<LinkUserDto>> {
    return request<ListResultDto<LinkUserDto>>('api/account/link-users', {
      method: 'GET',
    });
  }
  /**
   * 生成关联用户Token
   */
  function generateLinkTokenApi(): Promise<string> {
    return request(`api/account/link-users/generate-link-token`, {
      method: 'POST',
    });
  }
  /**
   * 生成关联用户登录Token
   */
  function generateLinkLoginTokenApi(): Promise<string> {
    return request(`api/account/link-users/generate-link-login-token`, {
      method: 'POST',
    });
  }

  /**
   * 取消用户关联
   * @param input 参数
   */
  function unLinkApi(input: UnLinkUserInput): Promise<void> {
    return request(`api/account/link-users/unlink`, {
      method: 'POST',
      data: input,
    });
  }

  return {
    cancel,
    getListApi,
    generateLinkTokenApi,
    generateLinkLoginTokenApi,
    unLinkApi,
  };
}
