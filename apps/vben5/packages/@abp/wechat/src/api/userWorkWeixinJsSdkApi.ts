import type { AgentConfigDto } from '../types/js-sdk';

import { useRequest } from '@abp/request';

export function userWorkWeixinJsSdkApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取企业微信应用配置
   * @returns 企业微信应用配置Dto
   */
  function getAgentConfigApi(): Promise<AgentConfigDto> {
    return request<AgentConfigDto>(`/api/wechat/work/jssdk/agent-config`, {
      method: 'GET',
    });
  }

  return {
    cancel,
    getAgentConfigApi,
  };
}
