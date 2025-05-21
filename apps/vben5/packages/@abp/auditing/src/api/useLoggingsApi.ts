import type { PagedResultDto } from '@abp/core';

import type { LogDto, LogGetListInput } from '../types/loggings';

import { useRequest } from '@abp/request';

export function useLoggingsApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取系统日志
   * @param id 日志id
   */
  function getApi(id: string): Promise<LogDto> {
    return request<LogDto>(`/api/auditing/logging/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 获取系统日志分页列表
   * @param input 参数
   */
  function getPagedListApi(
    input: LogGetListInput,
  ): Promise<PagedResultDto<LogDto>> {
    return request<PagedResultDto<LogDto>>('/api/auditing/logging', {
      method: 'GET',
      params: input,
    });
  }

  return {
    cancel,
    getApi,
    getPagedListApi,
  };
}
