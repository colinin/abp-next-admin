import type { PagedResultDto } from '@abp/core';

import type {
  BlobContainerCreateDto,
  BlobContainerDto,
  BlobContainerGetPagedListInput,
} from '../types/containers';

import { useRequest } from '@abp/request';

export function useBlobContainersApi() {
  const { cancel, request } = useRequest();

  function deleteApi(id: string): Promise<void> {
    return request(`/api/blob-management/containers/${id}`, {
      method: 'DELETE',
    });
  }

  function getApi(id: string): Promise<BlobContainerDto> {
    return request<BlobContainerDto>(`/api/blob-management/containers/${id}`, {
      method: 'GET',
    });
  }

  function getPagedListApi(
    input?: BlobContainerGetPagedListInput,
  ): Promise<PagedResultDto<BlobContainerDto>> {
    return request<PagedResultDto<BlobContainerDto>>(
      `/api/blob-management/containers`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  function createApi(input: BlobContainerCreateDto): Promise<BlobContainerDto> {
    return request<BlobContainerDto>(`/api/blob-management/containers`, {
      method: 'POST',
      data: input,
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getPagedListApi,
  };
}
