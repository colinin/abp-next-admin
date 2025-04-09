import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  GetTenantPagedListInput,
  TenantConnectionStringCheckInput,
  TenantConnectionStringDto,
  TenantConnectionStringSetInput,
  TenantCreateDto,
  TenantDto,
  TenantUpdateDto,
} from '../types';

import { useRequest } from '@abp/request';

export function useTenantsApi() {
  const { cancel, request } = useRequest();
  /**
   * 创建租户
   * @param {TenantCreateDto} input 参数
   * @returns 创建的租户
   */
  function createApi(input: TenantCreateDto): Promise<TenantDto> {
    return request<TenantDto>('/api/saas/tenants', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 编辑租户
   * @param {string} id 参数
   * @param {TenantUpdateDto} input 参数
   * @returns 编辑的租户
   */
  function updateApi(id: string, input: TenantUpdateDto): Promise<TenantDto> {
    return request<TenantDto>(`/api/saas/tenants/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 查询租户
   * @param {string} id Id
   * @returns 查询的租户
   */
  function getApi(id: string): Promise<TenantDto> {
    return request<TenantDto>(`/api/saas/tenants/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 删除租户
   * @param {string} id Id
   * @returns {void}
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/saas/tenants/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询租户分页列表
   * @param {GetTenantPagedListInput} input 参数
   * @returns {void}
   */
  function getPagedListApi(
    input?: GetTenantPagedListInput,
  ): Promise<PagedResultDto<TenantDto>> {
    return request<PagedResultDto<TenantDto>>(`/api/saas/tenants`, {
      method: 'GET',
      params: input,
    });
  }

  /**
   * 设置连接字符串
   * @param {string} id 租户Id
   * @param {TenantConnectionStringSetInput} input 参数
   * @returns 连接字符串
   */
  function setConnectionStringApi(
    id: string,
    input: TenantConnectionStringSetInput,
  ): Promise<TenantConnectionStringDto> {
    return request<TenantConnectionStringDto>(
      `/api/saas/tenants/${id}/connection-string`,
      {
        data: input,
        method: 'PUT',
      },
    );
  }

  /**
   * 查询连接字符串
   * @param {string} id 租户Id
   * @param {string} name 连接字符串名称
   * @returns 连接字符串
   */
  function getConnectionStringApi(
    id: string,
    name: string,
  ): Promise<TenantConnectionStringDto> {
    return request<TenantConnectionStringDto>(
      `/api/saas/tenants/${id}/connection-string/${name}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 查询所有连接字符串
   * @param {string} id 租户Id
   * @returns 连接字符串列表
   */
  function getConnectionStringsApi(
    id: string,
  ): Promise<ListResultDto<TenantConnectionStringDto>> {
    return request<ListResultDto<TenantConnectionStringDto>>(
      `/api/saas/tenants/${id}/connection-string`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 删除租户
   * @param {string} id 租户Id
   * @param {string} name 连接字符串名称
   * @returns {void}
   */
  function deleteConnectionStringApi(id: string, name: string): Promise<void> {
    return request(`/api/saas/tenants/${id}/connection-string/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 检查数据库连接字符串
   * @param input 参数
   */
  function checkConnectionString(
    input: TenantConnectionStringCheckInput,
  ): Promise<void> {
    return request(`/api/saas/tenants/connection-string/check`, {
      data: input,
      method: 'POST',
    });
  }

  return {
    cancel,
    checkConnectionString,
    createApi,
    deleteApi,
    deleteConnectionStringApi,
    getApi,
    getConnectionStringApi,
    getConnectionStringsApi,
    getPagedListApi,
    setConnectionStringApi,
    updateApi,
  };
}
