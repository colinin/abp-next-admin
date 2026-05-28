import type { PagedResultDto } from '@abp/core';

import type {
  BlobDto,
  BlobFolderCreateDto,
  BlobGetPagedListInput,
} from '../types/blobs';

import { useRequest } from '@abp/request';

import { BlobType } from '../types/blobs';

export function useBlobsApi() {
  const { cancel, request } = useRequest();

  function createFolderApi(input: BlobFolderCreateDto): Promise<BlobDto> {
    return request<BlobDto>(`/api/blob-management/blobs/folder`, {
      method: 'POST',
      data: input,
    });
  }

  function deleteApi(id: string): Promise<void> {
    return request(`/api/blob-management/blobs/${id}`, {
      method: 'DELETE',
    });
  }

  function getApi(id: string): Promise<BlobDto> {
    return request<BlobDto>(`/api/blob-management/blobs/${id}`, {
      method: 'GET',
    });
  }

  function generateDownloadUrlApi(id: string): Promise<string> {
    return request<string>(
      `/api/blob-management/blobs/generate/download-url/${id}`,
      {
        method: 'GET',
      },
    );
  }

  function generatePreviewUrlApi(id: string): Promise<string> {
    return request<string>(
      `/api/blob-management/blobs/generate/preview-url/${id}`,
      {
        method: 'GET',
      },
    );
  }

  function downloadApi(id: string): Promise<Blob> {
    return request<Blob>(`/api/blob-management/blobs/download/${id}`, {
      method: 'GET',
      responseType: 'blob',
    });
  }

  function getFolderPagedListApi(
    input: BlobGetPagedListInput,
  ): Promise<PagedResultDto<BlobDto>> {
    return getPagedListApi({
      ...input,
      type: BlobType.Folder,
    });
  }

  function getFilePagedListApi(
    input: BlobGetPagedListInput,
  ): Promise<PagedResultDto<BlobDto>> {
    return getPagedListApi({
      ...input,
      type: BlobType.File,
    });
  }

  function getPagedListApi(
    input: BlobGetPagedListInput,
  ): Promise<PagedResultDto<BlobDto>> {
    return request<PagedResultDto<BlobDto>>(`/api/blob-management/blobs`, {
      method: 'GET',
      params: input,
    });
  }

  return {
    cancel,
    createFolderApi,
    deleteApi,
    downloadApi,
    getApi,
    generateDownloadUrlApi,
    generatePreviewUrlApi,
    getFolderPagedListApi,
    getFilePagedListApi,
    getPagedListApi,
  };
}
