import type { ListResultDto } from '@abp/core';

import type {
  TextTemplateDefinitionCreateDto,
  TextTemplateDefinitionDto,
  TextTemplateDefinitionGetListInput,
  TextTemplateDefinitionUpdateDto,
} from '../types/definitions';

import { useRequest } from '@abp/request';

export function useTemplateDefinitionsApi() {
  const { cancel, request } = useRequest();

  /**
   * 新增模板定义
   * @param input 参数
   * @returns 模板定义数据传输对象
   */
  function createApi(
    input: TextTemplateDefinitionCreateDto,
  ): Promise<TextTemplateDefinitionDto> {
    return request<TextTemplateDefinitionDto>(
      '/api/text-templating/template/definitions',
      {
        data: input,
        method: 'POST',
      },
    );
  }
  /**
   * 删除模板定义
   * @param name 模板名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/text-templating/template/definitions/${name}`, {
      method: 'DELETE',
    });
  }
  /**
   * 获取模板定义
   * @param name 模板名称
   * @returns 模板定义数据传输对象
   */
  function getApi(name: string): Promise<TextTemplateDefinitionDto> {
    return request<TextTemplateDefinitionDto>(
      `/api/text-templating/template/definitions/${name}`,
      {
        method: 'GET',
      },
    );
  }
  /**
   * 获取模板定义列表
   * @param input 过滤参数
   * @returns 模板定义数据传输对象列表
   */
  function getListApi(
    input?: TextTemplateDefinitionGetListInput,
  ): Promise<ListResultDto<TextTemplateDefinitionDto>> {
    return request<ListResultDto<TextTemplateDefinitionDto>>(
      `/api/text-templating/template/definitions/${name}`,
      {
        method: 'GET',
        params: input,
      },
    );
  }
  /**
   * 更新模板定义
   * @param name 模板名称
   * @returns 模板定义数据传输对象
   */
  function updateApi(
    name: string,
    input: TextTemplateDefinitionUpdateDto,
  ): Promise<TextTemplateDefinitionDto> {
    return request<TextTemplateDefinitionDto>(
      `/api/text-templating/template/definitions/${name}`,
      {
        data: input,
        method: 'PUT',
      },
    );
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
