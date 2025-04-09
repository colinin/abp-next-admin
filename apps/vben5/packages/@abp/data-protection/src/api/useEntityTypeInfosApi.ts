import type { PagedResultDto } from '@abp/core';

import type {
  EntityTypeInfoDto,
  GetEntityTypeInfoListInput,
} from '../types/entityTypeInfos';

import { useRequest } from '@abp/request';

export function useEntityTypeInfosApi() {
  const { cancel, request } = useRequest();

  function getApi(id: string): Promise<EntityTypeInfoDto> {
    return request(`/api/data-protection-management/entity-type-infos/${id}`, {
      method: 'GET',
    });
  }

  function getPagedListApi(
    input?: GetEntityTypeInfoListInput,
  ): Promise<PagedResultDto<EntityTypeInfoDto>> {
    return request(`/api/data-protection-management/entity-type-infos`, {
      method: 'GET',
      params: input,
    });
  }

  return {
    cancel,
    getApi,
    getPagedListApi,
  };
}
