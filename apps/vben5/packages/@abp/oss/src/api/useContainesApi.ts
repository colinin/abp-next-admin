import type {
  GetOssContainersInput,
  OssContainerDto,
  OssContainersResultDto,
} from '../types/containes';

import { useRequest } from '@abp/request';

export function useContainesApi() {
  const { cancel, request } = useRequest();

  function deleteApi(name: string): Promise<void> {
    return request(`/api/oss-management/containes/${name}`, {
      method: 'DELETE',
    });
  }

  function getApi(name: string): Promise<OssContainerDto> {
    return request<OssContainerDto>(`/api/oss-management/containes/${name}`, {
      method: 'GET',
    });
  }

  function getListApi(
    input?: GetOssContainersInput,
  ): Promise<OssContainersResultDto> {
    return request<OssContainersResultDto>(`/api/oss-management/containes`, {
      method: 'GET',
      params: input,
    });
  }

  function createApi(name: string): Promise<OssContainerDto> {
    return request<OssContainerDto>(`/api/oss-management/containes/${name}`, {
      method: 'POST',
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getListApi,
  };
}
