import type { ListResultDto } from '@abp/core';

import type {
  SettingDefinitionCreateDto,
  SettingDefinitionDto,
  SettingDefinitionGetListInput,
  SettingDefinitionUpdateDto,
} from '../types/definitions';

import { requestClient } from '@abp/request';

/**
 * 删除设置定义
 * @param name 设置名称
 */
export function deleteApi(name: string): Promise<void> {
  return requestClient.delete(
    `/api/setting-management/settings/definitions/${name}`,
  );
}

/**
 * 查询设置定义
 * @param name 设置名称
 * @returns 设置定义数据传输对象
 */
export function getApi(name: string): Promise<SettingDefinitionDto> {
  return requestClient.get<SettingDefinitionDto>(
    `/api/setting-management/settings/definitions/${name}`,
  );
}

/**
 * 查询设置定义列表
 * @param input 设置过滤条件
 * @returns 设置定义数据传输对象列表
 */
export function getListApi(
  input?: SettingDefinitionGetListInput,
): Promise<ListResultDto<SettingDefinitionDto>> {
  return requestClient.get<ListResultDto<SettingDefinitionDto>>(
    `/api/setting-management/settings/definitions`,
    {
      params: input,
    },
  );
}

/**
 * 创建设置定义
 * @param input 设置定义参数
 * @returns 设置定义数据传输对象
 */
export function createApi(
  input: SettingDefinitionCreateDto,
): Promise<SettingDefinitionDto> {
  return requestClient.post<SettingDefinitionDto>(
    '/api/setting-management/settings/definitions',
    input,
  );
}

/**
 * 更新设置定义
 * @param name 设置名称
 * @param input 设置定义参数
 * @returns 设置定义数据传输对象
 */
export function updateApi(
  name: string,
  input: SettingDefinitionUpdateDto,
): Promise<SettingDefinitionDto> {
  return requestClient.put<SettingDefinitionDto>(
    `/api/setting-management/settings/definitions/${name}`,
    input,
  );
}
