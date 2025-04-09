import type { PagedResultDto } from '@abp/core';

import type { AuthorDto, GetAuthorPagedListInput } from '../types/authors';

import { useRequest } from '@abp/request';

export function useAuthorsApi() {
  const { cancel, request } = useRequest();

  function getPagedListApi(
    input?: GetAuthorPagedListInput,
  ): Promise<PagedResultDto<AuthorDto>> {
    return request<PagedResultDto<AuthorDto>>('/api/demo/authors', {
      method: 'GET',
      params: input,
    });
  }

  return {
    cancel,
    getPagedListApi,
  };
}
