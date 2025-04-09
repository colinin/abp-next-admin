import type {
  SubjectStrategyDto,
  SubjectStrategyGetInput,
  SubjectStrategySetInput,
} from '../types';

import { useRequest } from '@abp/request';

export function useSubjectStrategysApi() {
  const { cancel, request } = useRequest();

  /**
   * 获取主体数据权限策略
   * @param input 参数
   * @returns 数据权限策略配置
   */
  function getApi(
    input: SubjectStrategyGetInput,
  ): Promise<null | SubjectStrategyDto> {
    return request<null | SubjectStrategyDto>(
      '/api/data-protection-management/subject-strategys',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  /**
   * 设置主体数据权限策略
   * @param input 参数
   * @returns 数据权限策略配置
   */
  function setApi(input: SubjectStrategySetInput): Promise<SubjectStrategyDto> {
    return request<SubjectStrategyDto>(
      '/api/data-protection-management/subject-strategys',
      {
        data: input,
        method: 'PUT',
      },
    );
  }

  return {
    cancel,
    getApi,
    setApi,
  };
}
