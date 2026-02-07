import type { FindTenantResultDto } from "#/saas";
import requestClient from "../request";

export function findTenantByNameApi(name: string): Promise<FindTenantResultDto> {
	return requestClient.get<FindTenantResultDto>(`/api/abp/multi-tenancy/tenants/by-name/${name}`);
}

export function findTenantByIdApi(id: string): Promise<FindTenantResultDto> {
	return requestClient.get<FindTenantResultDto>(`/api/abp/multi-tenancy/tenants/by-id/${id}`);
}
