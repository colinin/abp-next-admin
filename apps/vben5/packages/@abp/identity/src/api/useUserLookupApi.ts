import type { ListResultDto } from '@abp/core';

import type {
  IdentityUserDto,
  UserLookupCountInput,
  UserLookupSearchInput,
} from '../types/users';

import { useRequest } from '@abp/request';

export function useUserLookupApi() {
  const { cancel, request } = useRequest();

  /**
   * 通过id查询用户
   * @param id 用户id
   * @returns 用户实体数据传输对象
   */
  function findByIdApi(id: string): Promise<IdentityUserDto> {
    return request<IdentityUserDto>(`/api/identity/users/lookup/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 通过用户名查询用户
   * @param userName 用户名
   * @returns 用户实体数据传输对象
   */
  function findByUserNameApi(userName: string): Promise<IdentityUserDto> {
    return request<IdentityUserDto>(
      `/api/identity/users/lookup/by-username/${userName}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 搜索用户列表
   * @param input 搜索过滤条件
   * @returns 用户实体数据传输对象列表
   */
  function searchApi(
    input?: UserLookupSearchInput,
  ): Promise<ListResultDto<IdentityUserDto>> {
    return request<ListResultDto<IdentityUserDto>>(
      `/api/identity/users/lookup/search`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 搜索用户数量
   * @param input 搜索过滤条件
   */
  function countApi(input?: UserLookupCountInput): Promise<number> {
    return request<number>(`/api/identity/users/lookup/count`, {
      method: 'GET',
      params: input,
    });
  }

  return {
    cancel,
    countApi,
    findByIdApi,
    findByUserNameApi,
    searchApi,
  };
}
