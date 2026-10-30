import type { ParticipantRef } from '../types/approval-flow';

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
  displayName: string;
  id: string;
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
