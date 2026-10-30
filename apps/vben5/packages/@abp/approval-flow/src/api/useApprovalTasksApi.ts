import type { ListResultDto } from '@abp/core';

import type { ApprovalPendingTaskDto } from '../types/approval-flow';
import type { DoneGetPagedListInput, DonePagedResult } from '../types/done';

import { useRequest } from '@abp/request';

const taskUrl = '/api/approval-flow/tasks';

export function useApprovalTasksApi() {
  const { request } = useRequest();

  function getPendingListApi(): Promise<ListResultDto<ApprovalPendingTaskDto>> {
    return request<ListResultDto<ApprovalPendingTaskDto>>(taskUrl, {
      method: 'GET',
    });
  }

  function getDoneListApi(
    input?: DoneGetPagedListInput,
  ): Promise<DonePagedResult> {
    return request<DonePagedResult>(`${taskUrl}/done`, {
      method: 'GET',
      params: input,
    });
  }

  function markViewedApi(taskId: string): Promise<void> {
    return request(`${taskUrl}/${taskId}/view`, { method: 'POST' });
  }

  return {
    getDoneListApi,
    markViewedApi,
    getPendingListApi,
  };
}
