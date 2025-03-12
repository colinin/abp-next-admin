import type { ListResultDto } from '@abp/core';

import type {
  GetTextByKeyInput,
  GetTextsInput,
  SetTextInput,
  TextDifferenceDto,
  TextDto,
} from '../types/texts';

import { useRequest } from '@abp/request';

export function useTextsApi() {
  const { cancel, request } = useRequest();

  /**
   * 查询文本列表
   * @param input 参数
   * @returns 文本列表
   */
  function getListApi(
    input: GetTextsInput,
  ): Promise<ListResultDto<TextDifferenceDto>> {
    return request<ListResultDto<TextDifferenceDto>>(
      '/api/abp/localization/texts',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 查询文本
   * @param input 参数
   * @returns 查询的文本
   */
  function getApi(input: GetTextByKeyInput): Promise<TextDto> {
    return request<TextDto>('/api/abp/localization/texts/by-culture-key', {
      method: 'GET',
      params: input,
    });
  }

  /**
   * 设置文本
   * @param input 参数
   */
  function setApi(input: SetTextInput): Promise<void> {
    return request('/api/localization/texts', {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    getApi,
    getListApi,
    setApi,
  };
}
