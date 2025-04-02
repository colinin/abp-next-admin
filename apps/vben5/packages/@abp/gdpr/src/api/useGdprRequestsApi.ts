import type { PagedResultDto } from '@abp/core';

import type { GdprRequestDto, GdprRequestGetListInput } from '../types';

import { useRequest } from '@abp/request';

export function useGdprRequestsApi() {
  const { cancel, request } = useRequest();

  /**
   * 删除个人数据请求
   * @param {string} id 请求Id
   * @returns {void}
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/gdpr/requests/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 删除用户账户
   * @returns {Promise<void>}
   */
  function deletePersonalAccountApi(): Promise<void> {
    return request(`/api/gdpr/requests/personal-account`, {
      method: 'DELETE',
    });
  }

  /**
   * 删除用户数据
   * @returns {Promise<void>}
   */
  function deletePersonalDataApi(): Promise<void> {
    return request(`/api/gdpr/requests/personal-data`, {
      method: 'DELETE',
    });
  }

  /**
   * 准备用户数据
   * @returns {Promise<void>}
   */
  function preparePersonalDataApi(): Promise<void> {
    return request(`/api/gdpr/requests/personal-data/prepare`, {
      method: 'POST',
    });
  }

  /**
   * 下载用户数据
   * @returns {Promise<Blob>} 用户数据文件
   */
  function downloadPersonalDataApi(requestId: string): Promise<Blob> {
    return request<Blob>(
      `/api/gdpr/requests/personal-data/download/${requestId}`,
      {
        method: 'GET',
        responseType: 'blob',
      },
    );
  }

  /**
   * 查询个人数据请求
   * @param {string} id 请求Id
   * @returns {Promise<GdprRequestDto>} 个人数据请求
   */
  function getApi(id: string): Promise<GdprRequestDto> {
    return request<GdprRequestDto>(`/api/gdpr/requests/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 查询个人数据请求分页列表
   * @param {GdprRequestGetListInput} input 参数
   * @returns {Promise<PagedResultDto<GdprRequestDto>>} 个人数据请求列表
   */
  function getPagedListApi(
    input?: GdprRequestGetListInput,
  ): Promise<PagedResultDto<GdprRequestDto>> {
    return request<PagedResultDto<GdprRequestDto>>(`/api/gdpr/requests`, {
      method: 'GET',
      params: input,
    });
  }

  return {
    cancel,
    deleteApi,
    deletePersonalAccountApi,
    deletePersonalDataApi,
    downloadPersonalDataApi,
    getApi,
    getPagedListApi,
    preparePersonalDataApi,
  };
}
