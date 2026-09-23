import type { ListResultDto } from '@abp/core';

import type {
  DeleteTextInput,
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
      '/api/localization/texts',
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
    return request<TextDto>('/api/localization/texts/by-key', {
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

  /**
   * 删除文本
   * @param input 参数
   */
  function deleteApi(input: DeleteTextInput): Promise<void> {
    return request('/api/localization/texts', {
      data: input,
      method: 'DELETE',
    });
  }

  return {
    cancel,
    deleteApi,
    getApi,
    getListApi,
    setApi,
  };
}
