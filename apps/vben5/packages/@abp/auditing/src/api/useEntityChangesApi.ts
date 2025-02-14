import type { ListResultDto } from '@abp/core';

import type {
  EntityChangeGetWithUsernameInput,
  EntityChangeWithUsernameDto,
} from '../types/entity-changes';

import { useRequest } from '@abp/request';

export function useEntityChangesApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取包含用户名称的实体变更列表
   * @param input 参数
   */
  function getListWithUsernameApi(
    input: EntityChangeGetWithUsernameInput,
  ): Promise<ListResultDto<EntityChangeWithUsernameDto>> {
    return request<ListResultDto<EntityChangeWithUsernameDto>>(
      '/api/auditing/entity-changes/with-username',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  return {
    cancel,
    getListWithUsernameApi,
  };
}
