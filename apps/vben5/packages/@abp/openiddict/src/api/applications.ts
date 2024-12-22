import type { PagedResultDto } from '@abp/core';

import type {
  OpenIddictApplicationCreateDto,
  OpenIddictApplicationDto,
  OpenIddictApplicationGetListInput,
  OpenIddictApplicationUpdateDto,
} from '../types/applications';

import { requestClient } from '@abp/request';

/**
 * 新增应用
 * @param input 参数
 * @returns 应用实体数据传输对象
 */
export function createApi(
  input: OpenIddictApplicationCreateDto,
): Promise<OpenIddictApplicationDto> {
  return requestClient.post<OpenIddictApplicationDto>(
    '/api/openiddict/applications',
    input,
  );
}

/**
 * 删除应用
 * @param id 应用id
 */
export function deleteApi(id: string): Promise<void> {
  return requestClient.delete(`/api/openiddict/applications/${id}`);
}

/**
 * 查询应用
 * @param id 应用id
 * @returns 应用实体数据传输对象
 */
export function getApi(id: string): Promise<OpenIddictApplicationDto> {
  return requestClient.get<OpenIddictApplicationDto>(
    `/api/openiddict/applications/${id}`,
  );
}

/**
 * 更新应用
 * @param id 应用id
 * @returns 应用实体数据传输对象
 */
export function updateApi(
  id: string,
  input: OpenIddictApplicationUpdateDto,
): Promise<OpenIddictApplicationDto> {
  return requestClient.put<OpenIddictApplicationDto>(
    `/api/openiddict/applications/${id}`,
    input,
  );
}

/**
 * 查询应用分页列表
 * @param input 过滤参数
 * @returns 应用实体数据传输对象分页列表
 */
export function getPagedListApi(
  input?: OpenIddictApplicationGetListInput,
): Promise<PagedResultDto<OpenIddictApplicationDto>> {
  return requestClient.get<PagedResultDto<OpenIddictApplicationDto>>(
    `/api/openiddict/applications`,
    {
      params: input,
    },
  );
}
