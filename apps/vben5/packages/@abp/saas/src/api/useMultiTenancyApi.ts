import type { FindTenantResultDto } from '../types/multiTenancys';

import { useRequest } from '@abp/request';

export function useMultiTenancyApi() {
  const { cancel, request } = useRequest();

  function findTenantByNameApi(name: string): Promise<FindTenantResultDto> {
    return request<FindTenantResultDto>(
      `/api/abp/multi-tenancy/tenants/by-name/${name}`,
      {
        method: 'GET',
      },
    );
  }

  function findTenantByIdApi(id: string): Promise<FindTenantResultDto> {
    return request<FindTenantResultDto>(
      `/api/abp/multi-tenancy/tenants/by-id/${id}`,
      {
        method: 'GET',
      },
    );
  }

  return {
    cancel,
    findTenantByIdApi,
    findTenantByNameApi,
  };
}
