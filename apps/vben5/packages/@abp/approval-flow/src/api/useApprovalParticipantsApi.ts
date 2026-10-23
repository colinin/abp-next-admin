import type {
  FormDefinitionDto,
  FormDefinitionPagedResult,
  ParticipantRef,
} from '../types/approval-flow';

import { useRequest } from '@abp/request';

interface IdentityUserDto {
  id: string;
  name?: null | string;
  userName?: null | string;
}

interface IdentityRoleDto {
  id: string;
  name: string;
}

interface OrganizationUnitDto {
  id: string;
  displayName: string;
}

export function useApprovalFormsApi() {
  const { request } = useRequest();

  function getListApi(input?: {
    filter?: null | string;
    isPublished?: boolean;
    maxResultCount?: number;
    skipCount?: number;
    sorting?: string;
  }): Promise<FormDefinitionPagedResult> {
    return request<FormDefinitionPagedResult>('/api/approval-flow/form-definitions', {
      method: 'GET',
      params: input ?? { maxResultCount: 100 },
    });
  }

  function getApi(id: string): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(`/api/approval-flow/form-definitions/${id}`, {
      method: 'GET',
    });
  }

  function createApi(input: {
    code: string;
    description?: null | string;
    name: string;
    version: number;
  }): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>('/api/approval-flow/form-definitions', {
      data: input,
      method: 'POST',
    });
  }

  function updateApi(
    id: string,
    input: { description?: null | string; name: string },
  ): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(`/api/approval-flow/form-definitions/${id}`, {
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
    return request<FormDefinitionDto>(`/api/approval-flow/form-definitions/${id}/fields`, {
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
      `/api/approval-flow/form-definitions/${id}/fields/${fieldId}`,
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
      `/api/approval-flow/form-definitions/${id}/fields/${fieldId}`,
      { method: 'DELETE' },
    );
  }

  function publishApi(id: string): Promise<FormDefinitionDto> {
    return request<FormDefinitionDto>(`/api/approval-flow/form-definitions/${id}/publish`, {
      method: 'POST',
    });
  }
  function deleteApi(id: string): Promise<void> {
    return request(`/api/approval-flow/form-definitions/${id}`, {
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

export function useApprovalParticipantsApi() {
  const { request } = useRequest();

  async function searchUsersAsync(filter?: string): Promise<ParticipantRef[]> {
    const result = await request<{ items: IdentityUserDto[] }>(
      '/api/identity/users',
      {
        method: 'GET',
        params: { filter, maxResultCount: 20 },
      },
    );

    return result.items.map((user) => ({
      provider: 'identity',
      type: 'User',
      id: user.id,
      displayName: user.name || user.userName || user.id,
    }));
  }

  async function getOrganizationUnitsAsync(): Promise<ParticipantRef[]> {
    const result = await request<{ items: OrganizationUnitDto[] }>(
      '/api/identity/organization-units',
      {
        method: 'GET',
        params: { maxResultCount: 200 },
      },
    );

    return result.items.map((unit) => ({
      provider: 'organization-unit',
      type: 'Group',
      id: unit.id,
      displayName: unit.displayName,
    }));
  }

  async function getRolesAsync(): Promise<ParticipantRef[]> {
    const result = await request<{ items: IdentityRoleDto[] }>(
      '/api/identity/roles',
      {
        method: 'GET',
        params: { maxResultCount: 200 },
      },
    );

    return result.items.map((role) => ({
      provider: 'role',
      type: 'Group',
      id: role.name,
      displayName: role.name,
    }));
  }

  return {
    getOrganizationUnitsAsync,
    getRolesAsync,
    searchUsersAsync,
  };
}
