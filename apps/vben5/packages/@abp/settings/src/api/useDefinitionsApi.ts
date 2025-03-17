import type { ListResultDto } from '@abp/core';

import type {
  SettingDefinitionCreateDto,
  SettingDefinitionDto,
  SettingDefinitionGetListInput,
  SettingDefinitionUpdateDto,
} from '../types';

import { useRequest } from '@abp/request';

export function useDefinitionsApi() {
  const { cancel, request } = useRequest();
  /**
   * 删除设置定义
   * @param name 设置名称
   */
  function deleteApi(name: string): Promise<void> {
    return request(`/api/setting-management/settings/definitions/${name}`, {
      method: 'DELETE',
    });
  }

  /**
   * 查询设置定义
   * @param name 设置名称
   * @returns 设置定义数据传输对象
   */
  function getApi(name: string): Promise<SettingDefinitionDto> {
    return request<SettingDefinitionDto>(
      `/api/setting-management/settings/definitions/${name}`,
      {
        method: 'GET',
      },
    );
  }

  /**
   * 查询设置定义列表
   * @param input 设置过滤条件
   * @returns 设置定义数据传输对象列表
   */
  function getListApi(
    input?: SettingDefinitionGetListInput,
  ): Promise<ListResultDto<SettingDefinitionDto>> {
    return request<ListResultDto<SettingDefinitionDto>>(
      `/api/setting-management/settings/definitions`,
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 创建设置定义
   * @param input 设置定义参数
   * @returns 设置定义数据传输对象
   */
  function createApi(
    input: SettingDefinitionCreateDto,
  ): Promise<SettingDefinitionDto> {
    return request<SettingDefinitionDto>(
      '/api/setting-management/settings/definitions',
      {
        data: input,
        method: 'POST',
      },
    );
  }

  /**
   * 更新设置定义
   * @param name 设置名称
   * @param input 设置定义参数
   * @returns 设置定义数据传输对象
   */
  function updateApi(
    name: string,
    input: SettingDefinitionUpdateDto,
  ): Promise<SettingDefinitionDto> {
    return request<SettingDefinitionDto>(
      `/api/setting-management/settings/definitions/${name}`,
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
