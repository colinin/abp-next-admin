import type { PagedResultDto } from '@abp/core';

import type { GetUserSessionsInput, IdentitySessionDto } from '../types';

import { useRequest } from '@abp/request';

export function useUserSessionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询会话列表
   * @param { GetUserSessionsInput } input 查询参数
   * @returns { Promise<PagedResultDto<IdentitySessionDto>> } 用户会话列表
   */
  function getSessionsApi(
    input?: GetUserSessionsInput,
  ): Promise<PagedResultDto<IdentitySessionDto>> {
    return request<PagedResultDto<IdentitySessionDto>>(
      '/api/identity/sessions',
      {
        method: 'GET',
        params: input,
      },
    );
  }
  /**
   * 撤销会话
   * @param { string } sessionId 会话id
   * @returns { Promise<void> }
   */
  function revokeSessionApi(sessionId: string): Promise<void> {
    return request(`/api/identity/sessions/${sessionId}/revoke`, {
      method: 'DELETE',
    });
  }

  return {
    cancel,
    getSessionsApi,
    revokeSessionApi,
  };
}
