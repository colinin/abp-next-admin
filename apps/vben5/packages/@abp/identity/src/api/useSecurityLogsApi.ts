import type { PagedResultDto } from '@abp/core';

import type {
  GetSecurityLogPagedRequest,
  SecurityLogDto,
} from '../types/security-logs';

import { useRequest } from '@abp/request';

export function useSecurityLogsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除安全日志
   * @param id id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/auditing/security-log/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询安全日志
   * @param id id
   * @returns 安全日志实体数据传输对象
   */
  function getApi(id: string): Promise<SecurityLogDto> {
    return request<SecurityLogDto>(`/api/auditing/security-log/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 查询安全日志分页列表
   * @param input 过滤参数
   * @returns 安全日志实体数据传输对象分页列表
   */
  function getPagedListApi(
    input?: GetSecurityLogPagedRequest,
  ): Promise<PagedResultDto<SecurityLogDto>> {
    return request<PagedResultDto<SecurityLogDto>>(
      `/api/auditing/security-log`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  return {
    cancel,
    deleteApi,
    getApi,
    getPagedListApi,
  };
}
