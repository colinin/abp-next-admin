import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  GetIdentityClaimTypePagedListInput,
  IdentityClaimTypeCreateDto,
  IdentityClaimTypeDto,
  IdentityClaimTypeUpdateDto,
} from '../types';

import { useRequest } from '@abp/request';

export function useClaimTypesApi() {
  const { cancel, request } = useRequest();

  /**
   * 新增用户声明
   * @param input 参数
   * @returns 用户声明实体数据传输对象
   */
  function createApi(
    input: IdentityClaimTypeCreateDto,
  ): Promise<IdentityClaimTypeDto> {
    return request<IdentityClaimTypeDto>('/api/identity/claim-types', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 删除用户声明
   * @param id 用户声明id
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/identity/claim-types/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询用户声明
   * @param id 用户声明id
   * @returns 用户声明实体数据传输对象
   */
  function getApi(id: string): Promise<IdentityClaimTypeDto> {
    return request<IdentityClaimTypeDto>(`/api/identity/claim-types/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 更新用户声明
   * @param id 用户声明id
   * @returns 用户声明实体数据传输对象
   */
  function updateApi(
    id: string,
    input: IdentityClaimTypeUpdateDto,
  ): Promise<IdentityClaimTypeDto> {
    return request<IdentityClaimTypeDto>(`/api/identity/claim-types/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 查询用户声明分页列表
   * @param input 过滤参数
   * @returns 用户声明实体数据传输对象分页列表
   */
  function getPagedListApi(
    input?: GetIdentityClaimTypePagedListInput,
  ): Promise<PagedResultDto<IdentityClaimTypeDto>> {
    return request<PagedResultDto<IdentityClaimTypeDto>>(
      `/api/identity/claim-types`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 获取可用的声明类型列表
   */
  function getAssignableClaimsApi(): Promise<
    ListResultDto<IdentityClaimTypeDto>
  > {
    return request<ListResultDto<IdentityClaimTypeDto>>(
      `/api/identity/claim-types/actived-list`,
      {
        method: 'GET',
      },
    );
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getAssignableClaimsApi,
    getPagedListApi,
    updateApi,
  };
}
