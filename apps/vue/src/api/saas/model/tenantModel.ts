import {
  AuditedEntityDto,
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';

/** 与 multi-tenancy中不同,此为管理tenant api */
export interface Tenant extends AuditedEntityDto {
  id: string;
  name: string;
}

export interface TenantConnectionString {
  name: string;
  value: string;
}

export interface CreateTenant {
  name: string;
  adminEmailAddress: string;
  adminPassword: string;
}

export interface UpdateTenant {
  name: string;
}

export interface GetTenantPagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export type TenantPagedResult = PagedResultDto<Tenant>;

export type TenantConnectionStringListResult = ListResultDto<TenantConnectionString>;
