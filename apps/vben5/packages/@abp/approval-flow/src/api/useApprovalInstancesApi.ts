import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  AddApprovalDto,
  ApprovalDecisionDto,
  ApprovalDecisionResultDto,
  ApprovalInstanceDetailDto,
  ApprovalInstanceDto,
  ApprovalRecordDto,
  GetApprovalInstancesDto,
  ReturnApprovalDto,
  StartApprovalDto,
  TransferApprovalDto,
} from '../types/approval-flow';

import { useRequest } from '@abp/request';

const instanceUrl = '/api/approval-flow/instances';

export function useApprovalInstancesApi() {
  const { request } = useRequest();

  function startApi(input: StartApprovalDto): Promise<ApprovalInstanceDto> {
    return request<ApprovalInstanceDto>(instanceUrl, {
      data: input,
      method: 'POST',
    });
  }

  function getApi(id: string): Promise<ApprovalInstanceDto> {
    return request<ApprovalInstanceDto>(`${instanceUrl}/${id}`, {
      method: 'GET',
    });
  }

  function getListApi(
    input?: GetApprovalInstancesDto,
  ): Promise<PagedResultDto<ApprovalInstanceDto>> {
    return request<PagedResultDto<ApprovalInstanceDto>>(instanceUrl, {
      method: 'GET',
      params: input,
    });
  }

  function getDetailApi(id: string): Promise<ApprovalInstanceDetailDto> {
    return request<ApprovalInstanceDetailDto>(`${instanceUrl}/${id}/detail`, {
      method: 'GET',
    });
  }

  function getRecordsApi(
    id: string,
  ): Promise<ListResultDto<ApprovalRecordDto>> {
    return request<ListResultDto<ApprovalRecordDto>>(
      `${instanceUrl}/${id}/records`,
      {
        method: 'GET',
      },
    );
  }

  function decideApi(
    id: string,
    input: ApprovalDecisionDto,
  ): Promise<ApprovalDecisionResultDto> {
    return request<ApprovalDecisionResultDto>(`${instanceUrl}/${id}/decide`, {
      data: input,
      method: 'POST',
    });
  }

  function returnApi(id: string, input: ReturnApprovalDto) {
    return request(`${instanceUrl}/${id}/return`, {
      data: input,
      method: 'POST',
    });
  }

  function transferApi(id: string, input: TransferApprovalDto) {
    return request(`${instanceUrl}/${id}/transfer`, {
      data: input,
      method: 'POST',
    });
  }

  function addApprovalApi(id: string, input: AddApprovalDto) {
    return request(`${instanceUrl}/${id}/add-approval`, {
      data: input,
      method: 'POST',
    });
  }

  function withdrawApi(
    id: string,
    comment?: string,
  ): Promise<ApprovalInstanceDto> {
    return request<ApprovalInstanceDto>(`${instanceUrl}/${id}/withdraw`, {
      data: { comment },
      method: 'POST',
    });
  }

  return {
    addApprovalApi,
    decideApi,
    getApi,
    getDetailApi,
    getListApi,
    getRecordsApi,
    returnApi,
    startApi,
    transferApi,
    withdrawApi,
  };
}
