import type { ListResultDto } from '@abp/core';

import type {
  EntityChangeGetWithUsernameInput,
  EntityChangeWithUsernameDto,
} from '../types/entity-changes';

import { requestClient } from '@abp/request';

/**
 * 获取包含用户名称的实体变更列表
 * @param input 参数
 */
export function getListWithUsernameApi(
  input: EntityChangeGetWithUsernameInput,
): Promise<ListResultDto<EntityChangeWithUsernameDto>> {
  return requestClient.get<ListResultDto<EntityChangeWithUsernameDto>>(
    '/api/auditing/entity-changes/with-username',
    {
      params: input,
    },
  );
}
