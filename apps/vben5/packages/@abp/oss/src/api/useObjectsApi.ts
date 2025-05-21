import type {
  CreateOssObjectInput,
  GetOssObjectInput,
  OssObjectDto,
} from '../types/objects';

import { useRequest } from '@abp/request';

export function useObjectsApi() {
  const { cancel, request } = useRequest();

  function createApi(input: CreateOssObjectInput): Promise<OssObjectDto> {
    const formData = new window.FormData();
    formData.append('bucket', input.bucket);
    formData.append('fileName', input.fileName);
    formData.append('overwrite', String(input.overwrite));
    input.expirationTime &&
      formData.append('expirationTime', input.expirationTime.toString());
    input.path && formData.append('path', input.path);
    input.file && formData.append('file', input.file);
    return request<OssObjectDto>(`/api/oss-management/objects`, {
      data: formData,
      headers: {
        'Content-Type': 'multipart/form-data;charset=utf-8',
      },
      method: 'POST',
    });
  }

  function generateUrlApi(input: GetOssObjectInput): Promise<string> {
    return request<string>('/api/oss-management/objects/generate-url', {
      method: 'GET',
      params: input,
    });
  }

  function deleteApi(input: GetOssObjectInput): Promise<void> {
    return request('/api/oss-management/objects', {
      method: 'DELETE',
      params: input,
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    generateUrlApi,
  };
}
