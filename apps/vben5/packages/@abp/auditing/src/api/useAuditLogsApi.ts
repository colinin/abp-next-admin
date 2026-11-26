import type { PagedResultDto } from '@abp/core';

import type {
  AuditLogDeleteManyInput,
  AuditLogDto,
  AuditLogGetListInput,
} from '../types/audit-logs';

import { useRequest } from '@abp/request';
import qs from 'qs';

export function useAuditLogsApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取审计日志
   * @param id 日志id
   */
  function getApi(id: string): Promise<AuditLogDto> {
    return request<AuditLogDto>(`/api/auditing/audit-log/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 导出审计日志列表
   * @param input 参数
   */
  function exportApi(input: AuditLogGetListInput): Promise<Blob> {
    return request<Blob>('/api/auditing/audit-log/export', {
      method: 'GET',
      params: input,
      responseType: 'blob',
    });
  }

  /**
   * 获取导出审计日志列表请求Url
   * @param input 参数
   */
  function getExportApiUrl(input: AuditLogGetListInput) {
    const query = qs.stringify(input, { arrayFormat: 'brackets' });
    return `/api/auditing/audit-log/export${query ? `?${query}` : ''}`;
  }

  /**
   * 获取审计日志分页列表
   * @param input 参数
   */
  function getPagedListApi(
    input: AuditLogGetListInput,
  ): Promise<PagedResultDto<AuditLogDto>> {
    return request<PagedResultDto<AuditLogDto>>('/api/auditing/audit-log', {
      method: 'GET',
      params: input,
    });
  }

  /**
   * 删除审计日志
   * @param id 日志id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/auditing/audit-log/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 批量删除审计日志
   * @param input 参数
   */
  function deleteManyApi(input: AuditLogDeleteManyInput): Promise<void> {
    return request(`/api/auditing/audit-log/bulk`, {
      data: input,
      method: 'DELETE',
    });
  }

  return {
    cancel,
    deleteApi,
    deleteManyApi,
    exportApi,
    getApi,
    getExportApiUrl,
    getPagedListApi,
  };
}
