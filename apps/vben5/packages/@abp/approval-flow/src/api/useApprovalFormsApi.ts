import type {
  FormDefinitionDto,
  FormDefinitionPagedResult,
} from '../types/approval-flow';

import { useRequest } from '@abp/request';

const formDefinitionUrl = '/api/approval-flow/form-definitions';

export function useApprovalFormsApi() {
  const { request } = useRequest();

  function getListApi(input?: {
    filter?: null | string;
    isPublished?: boolean;
    maxResultCount?: number;
    skipCount?: number;
    sorting?: string;
  }): Promise<FormDefinitionPagedResult> {
    return request<FormDefinitionPagedResult>(formDefinitionUrl, {
      method: 'GET',
      params: input ?? { maxResultCount: 100 },
    });
  }

  function getApi(id: string): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(`${formDefinitionUrl}/${id}`, {
      method: 'GET',
    });
  }

  function createApi(input: {
    code: string;
    description?: null | string;
    name: string;
    version: number;
  }): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(formDefinitionUrl, {
      data: input,
      method: 'POST',
    });
  }

  function updateApi(
    id: string,
    input: { description?: null | string; name: string },
  ): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(`${formDefinitionUrl}/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  function addFieldApi(
    id: string,
    input: {
      hint?: null | string;
      isRequired: boolean;
      key: string;
      label: string;
      optionsJson?: null | string;
      order: number;
      permission: string;
      type: string;
    },
  ): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(`${formDefinitionUrl}/${id}/fields`, {
      data: input,
      method: 'POST',
    });
  }

  function updateFieldApi(
    id: string,
    fieldId: string,
    input: {
      hint?: null | string;
      isRequired: boolean;
      label: string;
      optionsJson?: null | string;
      order: number;
      permission: string;
    },
  ): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(
      `${formDefinitionUrl}/${id}/fields/${fieldId}`,
      {
        data: input,
        method: 'PUT',
      },
    );
  }

  function removeFieldApi(
    id: string,
    fieldId: string,
  ): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(
      `${formDefinitionUrl}/${id}/fields/${fieldId}`,
      { method: 'DELETE' },
    );
  }

  function publishApi(id: string): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(`${formDefinitionUrl}/${id}/publish`, {
      method: 'POST',
    });
  }

  function deleteApi(id: string): Promise<void> {
    return request(`${formDefinitionUrl}/${id}`, {
      method: 'DELETE',
    });
  }

  return {
    addFieldApi,
    createApi,
    deleteApi,
    getApi,
    getListApi,
    publishApi,
    removeFieldApi,
    updateApi,
    updateFieldApi,
  };
}
