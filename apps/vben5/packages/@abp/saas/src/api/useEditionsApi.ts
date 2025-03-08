import type { PagedResultDto } from '@abp/core';

import type {
  EditionCreateDto,
  EditionDto,
  EditionUpdateDto,
  GetEditionPagedListInput,
} from '../types';

import { useRequest } from '@abp/request';

export function useEditionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 创建版本
   * @param {EditionCreateDto} input 参数
   * @returns 创建的版本
   */
  function createApi(input: EditionCreateDto): Promise<EditionDto> {
    return request<EditionDto>('/api/saas/editions', {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 编辑版本
   * @param {string} id 参数
   * @param {EditionUpdateDto} input 参数
   * @returns 编辑的版本
   */
  function updateApi(id: string, input: EditionUpdateDto): Promise<EditionDto> {
    return request<EditionDto>(`/api/saas/editions/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  /**
   * 查询版本
   * @param {string} id Id
   * @returns 查询的版本
   */
  function getApi(id: string): Promise<EditionDto> {
    return request<EditionDto>(`/api/saas/editions/${id}`, {
      method: 'GET',
    });
  }

  /**
   * 删除版本
   * @param {string} id Id
   * @returns {void}
   */
  function deleteApi(id: string): Promise<void> {
    return request(`/api/saas/editions/${id}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询版本分页列表
   * @param {GetEditionPagedListInput} input 参数
   * @returns {void}
   */
  function getPagedListApi(
    input?: GetEditionPagedListInput,
  ): Promise<PagedResultDto<EditionDto>> {
    return request<PagedResultDto<EditionDto>>(`/api/saas/editions`, {
      method: 'GET',
      params: input,
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getPagedListApi,
    updateApi,
  };
}
