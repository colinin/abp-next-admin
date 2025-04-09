import type {
  RoleEntityRuleCreateDto,
  RoleEntityRuleDto,
  RoleEntityRuleGetInput,
} from '../types/roleEntityRules';

import { useRequest } from '@abp/request';

export function useRoleEntityRulesApi() {
  const { cancel, request } = useRequest();

  function getApi(input: RoleEntityRuleGetInput): Promise<RoleEntityRuleDto> {
    return request('/api/data-protection-management/entity-rule/roles', {
      method: 'GET',
      params: input,
    });
  }

  function createApi(
    input: RoleEntityRuleCreateDto,
  ): Promise<RoleEntityRuleDto> {
    return request('/api/data-protection-management/entity-rule/roles', {
      data: input,
      method: 'POST',
    });
  }

  function updateApi(
    id: string,
    input: RoleEntityRuleCreateDto,
  ): Promise<RoleEntityRuleDto> {
    return request(`/api/data-protection-management/entity-rule/roles/${id}`, {
      data: input,
      method: 'PUT',
    });
  }

  return {
    cancel,
    createApi,
    getApi,
    updateApi,
  };
}
