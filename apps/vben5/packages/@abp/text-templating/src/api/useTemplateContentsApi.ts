import type {
  TextTemplateContentDto,
  TextTemplateContentGetInput,
  TextTemplateContentUpdateDto,
  TextTemplateRestoreInput,
} from '../types/contents';

import { useRequest } from '@abp/request';

export function useTemplateContentsApi() {
  const { cancel, request } = useRequest();
  /**
   * 获取模板内容
   * @param input 参数
   * @returns 模板内容数据传输对象
   */
  function getApi(
    input: TextTemplateContentGetInput,
  ): Promise<TextTemplateContentDto> {
    let url = '/api/text-templating/templates/content';
    url += input.culture ? `/${input.culture}/${input.name}` : `/${input.name}`;
    return request<TextTemplateContentDto>(url, {
      method: 'GET',
    });
  }
  /**
   * 重置模板内容为默认值
   * @param name 模板名称
   * @param input 参数
   * @returns 模板定义数据传输对象列表
   */
  function restoreToDefaultApi(
    name: string,
    input: TextTemplateRestoreInput,
  ): Promise<void> {
    return request(
      `/api/text-templating/templates/content/${name}/restore-to-default`,
      {
        data: input,
        method: 'PUT',
      },
    );
  }
  /**
   * 更新模板内容
   * @param name 模板名称
   * @param input 参数
   * @returns 模板内容数据传输对象
   */
  function updateApi(
    name: string,
    input: TextTemplateContentUpdateDto,
  ): Promise<TextTemplateContentDto> {
    return request<TextTemplateContentDto>(
      `/api/text-templating/templates/content/${name}`,
      {
        data: input,
        method: 'PUT',
      },
    );
  }

  return {
    cancel,
    getApi,
    restoreToDefaultApi,
    updateApi,
  };
}
