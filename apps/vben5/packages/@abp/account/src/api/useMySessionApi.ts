import type { PagedResultDto } from '@abp/core';
import type { GetMySessionsInput, IdentitySessionDto } from '@abp/identity';

import { useRequest } from '@abp/request';

export function useMySessionApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询会话列表
   * @param { GetMySessionsInput } input 查询参数
   * @returns { Promise<PagedResultDto<IdentitySessionDto>> } 用户会话列表
   */
  function getSessionsApi(
    input?: GetMySessionsInput,
  ): Promise<PagedResultDto<IdentitySessionDto>> {
    return request<PagedResultDto<IdentitySessionDto>>(
      '/api/account/my-profile/sessions',
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
    return request(`/api/account/my-profile/sessions/${sessionId}/revoke`, {
      method: 'DELETE',
    });
  }

  return {
    cancel,
    getSessionsApi,
    revokeSessionApi,
  };
}
