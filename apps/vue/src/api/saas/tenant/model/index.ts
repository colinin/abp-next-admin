export interface TenantGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface TenantCreateDto extends TenantCreateOrUpdateBase {
  adminEmailAddress: string;
  adminPassword: string;
  useSharedDatabase?: boolean;
  defaultConnectionString?: string;
}

export interface TenantUpdateDto extends TenantCreateOrUpdateBase {
  concurrencyStamp?: string;
}

export interface TenantConnectionStringCreateOrUpdate {
  name: string;
  value: string;
}

export interface TenantDto extends ExtensibleAuditedEntityDto<string> {
  name?: string;
  editionId?: string;
  editionName?: string;
  isActive?: boolean;
  enableTime?: string;
  disableTime?: string;
  concurrencyStamp?: string;
}

export interface TenantCreateOrUpdateBase extends ExtensibleObject {
  name: string;
  isActive?: boolean;
  editionId?: string;
  enableTime?: string;
  disableTime?: string;
}

export interface TenantConnectionStringDto {
  name?: string;
  value?: string;
}

