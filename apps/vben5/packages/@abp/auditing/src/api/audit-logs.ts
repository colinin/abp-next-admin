import type { PagedResultDto } from '@abp/core';

import type { AuditLogDto, AuditLogGetListInput } from '../types/audit-logs';

import { requestClient } from '@abp/request';

/**
 * 获取审计日志
 * @param id 日志id
 */
export function getApi(id: string): Promise<AuditLogDto> {
  return requestClient.get<AuditLogDto>(`/api/auditing/audit-log/${id}`);
}

/**
 * 获取审计日志分页列表
 * @param input 参数
 */
export function getPagedListApi(
  input: AuditLogGetListInput,
): Promise<PagedResultDto<AuditLogDto>> {
  return requestClient.get<PagedResultDto<AuditLogDto>>(
    '/api/auditing/audit-log',
    {
      params: input,
    },
  );
}

/**
 * 删除审计日志
 * @param id 日志id
 */
export function deleteApi(id: string): Promise<void> {
  return requestClient.delete(`/api/auditing/audit-log/${id}`);
}
