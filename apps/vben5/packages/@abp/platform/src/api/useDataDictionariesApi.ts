import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  DataCreateDto,
  DataDto,
  DataItemCreateDto,
  DataItemUpdateDto,
  DataMoveDto,
  DataUpdateDto,
  GetDataListInput,
} from '../types/dataDictionaries';

import { useRequest } from '@abp/request';

export function useDataDictionariesApi() {
  const { cancel, request } = useRequest();

  function createApi(input: DataCreateDto): Promise<DataDto> {
    return request<DataDto>('/api/platform/datas', {
      data: input,
      method: 'POST',
    });
  }

  function deleteApi(id: string): Promise<void> {
    return request(`/api/platform/datas/${id}`, {
      method: 'DELETE',
    });
  }

  function createItemApi(id: string, input: DataItemCreateDto): Promise<void> {
    return request(`/api/platform/datas/${id}/items`, {
      data: input,
      method: 'POST',
    });
  }

  function deleteItemApi(id: string, name: string): Promise<void> {
    return request(`/api/platform/datas/${id}/items/${name}`, {
      method: 'DELETE',
    });
  }

  function getApi(id: string): Promise<DataDto> {
    return request<DataDto>(`/api/platform/datas/${id}`, {
      method: 'GET',
    });
  }

  function getByNameApi(name: string): Promise<DataDto> {
    return request<DataDto>(`/api/platform/datas/by-name/${name}`, {
      method: 'GET',
    });
  }

  function getAllApi(): Promise<ListResultDto<DataDto>> {
    return request<ListResultDto<DataDto>>(`/api/platform/datas/all`, {
      method: 'GET',
    });
  }

  function getPagedListApi(
    input?: GetDataListInput,
  ): Promise<PagedResultDto<DataDto>> {
    return request<PagedResultDto<DataDto>>(`/api/platform/datas`, {
      method: 'GET',
      params: input,
    });
  }

  function moveApi(id: string, input: DataMoveDto): Promise<DataDto> {
    return request<DataDto>(`/api/platform/datas/${id}/move`, {
      data: input,
      method: 'PUT',
    });
  }

  function updateApi(id: string, input: DataUpdateDto): Promise<DataDto> {
    return request<DataDto>(`/api/platform/datas/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  function updateItemApi(
    id: string,
    name: string,
    input: DataItemUpdateDto,
  ): Promise<void> {
    return request(`/api/platform/datas/${id}/items/${name}`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    createItemApi,
    deleteApi,
    deleteItemApi,
    getAllApi,
    getApi,
    getByNameApi,
    getPagedListApi,
    moveApi,
    updateApi,
    updateItemApi,
  };
}
