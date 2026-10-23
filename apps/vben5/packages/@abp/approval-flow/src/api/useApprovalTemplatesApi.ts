import type { ListResultDto, PagedResultDto } from '@abp/core';

import type {
  ApprovalTemplateDto,
  CreateApprovalTemplateDto,
  PublishApprovalTemplateDto,
  SaveApprovalDesignDto,
  UpdateApprovalTemplateDto,
} from '../types/approval-flow';

import { useRequest } from '@abp/request';

const templateUrl = '/api/approval-flow/templates';

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
      `${templateUrl}/available-templates`,
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
