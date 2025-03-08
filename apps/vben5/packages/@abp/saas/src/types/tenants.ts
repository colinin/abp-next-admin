import type {
  ExtensibleAuditedEntityDto,
  ExtensibleObject,
  IHasConcurrencyStamp,
  NameValue,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

type TenantConnectionStringDto = NameValue<string>;

type TenantConnectionStringSetInput = NameValue<string>;

interface TenantDto
  extends ExtensibleAuditedEntityDto<string>,
    IHasConcurrencyStamp {
  /** 禁用时间 */
  disableTime?: string;
  /** 版本Id */
  editionId?: string;
  /** 版本名称 */
  editionName?: string;
  /** 启用时间 */
  enableTime?: string;
  /** 是否可用 */
  isActive: boolean;
  /** 名称 */
  name: string;
  /** 名称 */
  normalizedName: string;
}

interface GetTenantPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface TenantCreateOrUpdateBase extends ExtensibleObject {
  /** 禁用时间 */
  disableTime?: string;
  /** 版本Id */
  editionId?: string;
  /** 启用时间 */
  enableTime?: string;
  /** 是否可用 */
  isActive: boolean;
  /** 名称 */
  name: string;
}

interface TenantCreateDto extends TenantCreateOrUpdateBase {
  adminEmailAddress: string;
  adminPassword: string;
  connectionStrings?: TenantConnectionStringSetInput[];
  defaultConnectionString?: string;
  useSharedDatabase: boolean;
}

interface TenantUpdateDto
  extends IHasConcurrencyStamp,
    TenantCreateOrUpdateBase {}

export type {
  GetTenantPagedListInput,
  TenantConnectionStringDto,
  TenantConnectionStringSetInput,
  TenantCreateDto,
  TenantDto,
  TenantUpdateDto,
};
