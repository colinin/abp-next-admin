import type { ListResultDto } from '@abp/core';

import type {
  LanguageCreateDto,
  LanguageDto,
  LanguageGetListInput,
  LanguageUpdateDto,
} from '../types/languages';

import { useRequest } from '@abp/request';

export function useLanguagesApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询语言列表
   * @param input 参数
   * @returns 语言列表
   */
  function getListApi(
    input?: LanguageGetListInput,
  ): Promise<ListResultDto<LanguageDto>> {
    return request<ListResultDto<LanguageDto>>(
      '/api/abp/localization/languages',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 查询语言
   * @param name 语言名称
   * @returns 查询的语言
   */
  function getApi(name: string): Promise<LanguageDto> {
    return request<LanguageDto>(`/api/localization/languages/${name}`, {
      method: 'GET',
    });
  }

  /**
   * 删除语言
   * @param name 语言名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/localization/languages/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 创建语言
   * @param input 参数
   * @returns 创建的语言
   */
  function createApi(input: LanguageCreateDto): Promise<LanguageDto> {
    return request<LanguageDto>(`/api/localization/languages`, {
      data: input,
      method: 'POST',
    });
  }

  /**
   * 编辑语言
   * @param name 语言名称
   * @param input 参数
   * @returns 编辑的语言
   */
  function updateApi(
    name: string,
    input: LanguageUpdateDto,
  ): Promise<LanguageDto> {
    return request<LanguageDto>(`/api/localization/languages/${name}`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getApi,
    getListApi,
    updateApi,
  };
}
