import { defHttp } from '/@/utils/http/axios';
import {
  IdentitySessionDto,
  GetUserSessionsInput
} from './model';

/**
 * 查询会话列表
 * @param { GetUserSessionsInput } input 查询参数
 * @returns { Promise<PagedResultDto<IdentitySessionDto>> }
 */
export const getSessions = (input?: GetUserSessionsInput): Promise<PagedResultDto<IdentitySessionDto>> => {
  return defHttp.get<PagedResultDto<IdentitySessionDto>>({
    url: '/api/identity/sessions',
    params: input,
  });
};
/**
 * 撤销会话
 * @param { string } sessionId 会话id
 * @returns { Promise<void> }
 */
export const revokeSession = (sessionId: string): Promise<void> => {
  return defHttp.delete<void>({
    url: `/api/identity/sessions/${sessionId}/revoke`,
  });
}
