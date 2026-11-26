import type { PagedResultDto } from '@abp/core';

import type {
  BackgroundJobLogDto,
  BackgroundJobLogGetListInput,
} from '../types';

import { useRequest } from '@abp/request';

export function useJobLogsApi() {
  const { cancel, request } = useRequest();

  function getApi(id: string): Promise<BackgroundJobLogDto> {
    return request<BackgroundJobLogDto>(
      `/api/task-management/background-jobs/logs/${id}`,
      {
        method: 'GET',
      },
    );
  }

  function deleteApi(id: string): Promise<void> {
    return request(`/api/task-management/background-jobs/logs/${id}`, {
      method: 'DELETE',
    });
  }

  function getPagedListApi(
    input?: BackgroundJobLogGetListInput,
  ): Promise<PagedResultDto<BackgroundJobLogDto>> {
    return request<PagedResultDto<BackgroundJobLogDto>>(
      `/api/task-management/background-jobs/logs`,
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
