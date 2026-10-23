import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  CarbonCopyDto,
  CarbonCopyGetPagedListInput,
  CarbonCopyPagedResult,
} from '../types/carbon-copies';

import type {
  AddApprovalDto,
  ApprovalDecisionDto,
  ApprovalDecisionResultDto,
  ApprovalInstanceDetailDto,
  ApprovalInstanceDto,
  ApprovalRecordDto,
  ApprovalTemplateDto,
  CreateApprovalTemplateDto,
  GetApprovalInstancesDto,
  PublishApprovalTemplateDto,
  ReturnApprovalDto,
  SaveApprovalDesignDto,
  StartApprovalDto,
  TransferApprovalDto,
  UpdateApprovalTemplateDto,
} from '../types/approval-flow';

import { useRequest } from '@abp/request';

const templateUrl = '/api/approval-flow/templates';
const instanceUrl = '/api/approval-flow/instances';

export function useApprovalTemplatesApi() {
  const { request } = useRequest();

  function getListApi(input?: {
    category?: null | string;
    filter?: null | string;
    isEnabled?: boolean;
    isPublished?: boolean;
    maxResultCount?: number;
    skipCount?: number;
    sorting?: string;
  }): Promise<PagedResultDto<ApprovalTemplateDto>> {
    return request<PagedResultDto<ApprovalTemplateDto>>(templateUrl, {
      method: 'GET',
      params: input,
    });
  }

  function getAvailableTemplatesApi(): Promise<
    ListResultDto<ApprovalTemplateDto>
  > {
    return request<ListResultDto<ApprovalTemplateDto>>(
      '/api/approval-flow/available-templates',
      { method: 'GET' },
    );
  }

  function getApi(id: string): Promise<ApprovalTemplateDto> {
    return request<ApprovalTemplateDto>(`${templateUrl}/${id}`, {
      method: 'GET',
    });
  }

  function createApi(
    input: CreateApprovalTemplateDto,
  ): Promise<ApprovalTemplateDto> {
    return request<ApprovalTemplateDto>(templateUrl, {
      data: input,
      method: 'POST',
    });
  }

  function updateApi(
    id: string,
    input: UpdateApprovalTemplateDto,
  ): Promise<ApprovalTemplateDto> {
    return request<ApprovalTemplateDto>(`${templateUrl}/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  function deleteApi(id: string): Promise<void> {
    return request(`${templateUrl}/${id}`, { method: 'DELETE' });
  }

  function publishApi(
    id: string,
    input: PublishApprovalTemplateDto,
  ): Promise<ApprovalTemplateDto> {
    return request<ApprovalTemplateDto>(`${templateUrl}/${id}/publish`, {
      data: input,
      method: 'POST',
    });
  }

  function saveDesignApi(
    id: string,
    input: SaveApprovalDesignDto,
  ): Promise<ApprovalTemplateDto> {
    return request<ApprovalTemplateDto>(`${templateUrl}/${id}/design`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    createApi,
    deleteApi,
    getApi,
    getAvailableTemplatesApi,
    getListApi,
    publishApi,
    saveDesignApi,
    updateApi,
  };
}

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
