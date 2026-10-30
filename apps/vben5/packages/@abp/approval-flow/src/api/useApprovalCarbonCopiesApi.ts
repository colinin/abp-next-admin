import type {
  CarbonCopyDto,
  CarbonCopyGetPagedListInput,
  CarbonCopyPagedResult,
} from '../types/carbon-copies';

import { useRequest } from '@abp/request';

export function useApprovalCarbonCopiesApi() {
  const { request } = useRequest();

  function getListApi(
    input?: CarbonCopyGetPagedListInput,
  ): Promise<CarbonCopyPagedResult> {
    return request<CarbonCopyPagedResult>('/api/approval-flow/carbon-copies', {
      method: 'GET',
      params: input,
    });
  }

  function markReadApi(id: string): Promise<CarbonCopyDto> {
    return request<CarbonCopyDto>(
      `/api/approval-flow/carbon-copies/${id}/read`,
      { method: 'POST' },
    );
  }

  return {
    getListApi,
    markReadApi,
  };
}
