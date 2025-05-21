import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  BackgroundJobDefinitionDto,
  BackgroundJobInfoBatchInput,
  BackgroundJobInfoCreateDto,
  BackgroundJobInfoDto,
  BackgroundJobInfoGetListInput,
  BackgroundJobInfoUpdateDto,
} from '../types/job-infos';

import { useRequest } from '@abp/request';

export function useJobInfosApi() {
  const { cancel, request } = useRequest();

  function createApi(
    input: BackgroundJobInfoCreateDto,
  ): Promise<BackgroundJobInfoDto> {
    return request<BackgroundJobInfoDto>(
      '/api/task-management/background-jobs',
      {
        data: input,
        method: 'POST',
      },
    );
  }

  function deleteApi(id: string): Promise<void> {
    return request(`/api/task-management/background-jobs/${id}`, {
      method: 'DELETE',
    });
  }

  function bulkDeleteApi(input: BackgroundJobInfoBatchInput): Promise<void> {
    return request(`/api/task-management/background-jobs/bulk-delete`, {
      data: input,
      method: 'DELETE',
    });
  }

  function getApi(id: string): Promise<BackgroundJobInfoDto> {
    return request<BackgroundJobInfoDto>(
      `/api/task-management/background-jobs/${id}`,
      {
        method: 'GET',
      },
    );
  }

  function getPagedListApi(
    input?: BackgroundJobInfoGetListInput,
  ): Promise<PagedResultDto<BackgroundJobInfoDto>> {
    return request<PagedResultDto<BackgroundJobInfoDto>>(
      `/api/task-management/background-jobs`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  function pauseApi(id: string): Promise<void> {
    return request(`/api/task-management/background-jobs/${id}/pause`, {
      method: 'PUT',
    });
  }

  function bulkPauseApi(input: BackgroundJobInfoBatchInput): Promise<void> {
    return request(`/api/task-management/background-jobs/bulk-pause`, {
      data: input,
      method: 'PUT',
    });
  }

  function resumeApi(id: string): Promise<void> {
    return request(`/api/task-management/background-jobs/${id}/resume`, {
      method: 'PUT',
    });
  }

  function bulkResumeApi(input: BackgroundJobInfoBatchInput): Promise<void> {
    return request(`/api/task-management/background-jobs/bulk-resume`, {
      data: input,
      method: 'PUT',
    });
  }

  function triggerApi(id: string): Promise<void> {
    return request(`/api/task-management/background-jobs/${id}/trigger`, {
      method: 'PUT',
    });
  }

  function bulkTriggerApi(input: BackgroundJobInfoBatchInput): Promise<void> {
    return request(`/api/task-management/background-jobs/bulk-trigger`, {
      data: input,
      method: 'PUT',
    });
  }

  function stopApi(id: string): Promise<void> {
    return request(`/api/task-management/background-jobs/${id}/stop`, {
      method: 'PUT',
    });
  }

  function bulkStopApi(input: BackgroundJobInfoBatchInput): Promise<void> {
    return request(`/api/task-management/background-jobs/bulk-stop`, {
      data: input,
      method: 'PUT',
    });
  }

  function startApi(id: string): Promise<void> {
    return request(`/api/task-management/background-jobs/${id}/start`, {
      method: 'PUT',
    });
  }

  function bulkStartApi(input: BackgroundJobInfoBatchInput): Promise<void> {
    return request(`/api/task-management/background-jobs/bulk-start`, {
      data: input,
      method: 'PUT',
    });
  }

  function updateApi(
    id: string,
    input: BackgroundJobInfoUpdateDto,
  ): Promise<BackgroundJobInfoDto> {
    return request(`/api/task-management/background-jobs/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  function getDefinitionsApi(): Promise<
    ListResultDto<BackgroundJobDefinitionDto>
  > {
    return request<ListResultDto<BackgroundJobDefinitionDto>>(
      `/api/task-management/background-jobs/definitions`,
      {
        method: 'GET',
      },
    );
  }

  return {
    bulkDeleteApi,
    bulkPauseApi,
    bulkResumeApi,
    bulkStartApi,
    bulkStopApi,
    bulkTriggerApi,
    cancel,
    createApi,
    deleteApi,
    getApi,
    getDefinitionsApi,
    getPagedListApi,
    pauseApi,
    resumeApi,
    startApi,
    stopApi,
    triggerApi,
    updateApi,
  };
}
