import type { PagedResultDto } from '@abp/core';

import type {
  TemplateCreateDto,
  TemplateDeleteDto,
  TemplateDto,
  TemplateGetPagedListInput,
  TemplateUpdateDto,
} from '../types/templates';

import { useRequest } from '@abp/request';

export function useTemplatesAdminApi() {
  const { cancel, request } = useRequest();

  function createApi(input: TemplateCreateDto): Promise<TemplateDto> {
    return request<TemplateDto>(`/api/print-designer/admin/templates`, {
      data: input,
      method: 'POST',
    });
  }

  function getPagedListApi(
    input?: TemplateGetPagedListInput,
  ): Promise<PagedResultDto<TemplateDto>> {
    return request<PagedResultDto<TemplateDto>>(
      '/api/print-designer/admin/templates',
      {
        method: 'GET',
        params: input,
      },
    );
  }

  function getApi(id: string): Promise<TemplateDto> {
    return request<TemplateDto>(`/api/print-designer/admin/templates/${id}`, {
      method: 'GET',
    });
  }

  function deleteApi(id: string): Promise<TemplateDeleteDto> {
    return request<TemplateDeleteDto>(
      `/api/print-designer/admin/templates/${id}`,
      {
        method: 'DELETE',
      },
    );
  }

  function updateApi(
    id: string,
    input: TemplateUpdateDto,
  ): Promise<TemplateDto> {
    return request<TemplateDto>(`/api/print-designer/admin/templates/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    deleteApi,
    getPagedListApi,
    getApi,
    updateApi,
  };
}
