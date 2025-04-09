import type { PagedResultDto } from '@abp/core';
import type {
  EntityTypeInfoGetModel,
  EntityTypeInfoModel,
} from '@abp/data-protection';

import type {
  BookDto,
  CreateUpdateBookDto,
  GetBookPagedListInput,
} from '../types/books';

import { useRequest } from '@abp/request';

export function useBooksApi() {
  const { cancel, request } = useRequest();

  function createApi(input: CreateUpdateBookDto): Promise<BookDto> {
    return request<BookDto>(`/api/demo/books`, {
      data: input,
      method: 'POST',
    });
  }

  function deleteApi(id: string): Promise<void> {
    return request(`/api/demo/books/${id}`, {
      method: 'DELETE',
    });
  }

  function getApi(id: string): Promise<BookDto> {
    return request<BookDto>(`/api/demo/books/${id}`, {
      method: 'GET',
    });
  }

  function getPagedListApi(
    input?: GetBookPagedListInput,
  ): Promise<PagedResultDto<BookDto>> {
    return request<PagedResultDto<BookDto>>('/api/demo/books', {
      method: 'GET',
      params: input,
    });
  }

  function getEntityInfoApi(
    input: EntityTypeInfoGetModel,
  ): Promise<EntityTypeInfoModel> {
    return request<EntityTypeInfoModel>('/api/demo/books/entity', {
      method: 'GET',
      params: input,
    });
  }

  function updateApi(id: string, input: CreateUpdateBookDto): Promise<BookDto> {
    return request<BookDto>(`/api/demo/books/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getEntityInfoApi,
    getPagedListApi,
    updateApi,
  };
}
