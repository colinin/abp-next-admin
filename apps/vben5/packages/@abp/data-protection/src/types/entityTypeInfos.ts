import type {
  AuditedEntityDto,
  EntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface EntityEnumInfoDto extends EntityDto<string> {
  displayName: string;
  name: string;
  value: string;
}

interface EntityPropertyInfoDto extends EntityDto<string> {
  displayName: string;
  enums: EntityEnumInfoDto[];
  javaScriptType: string;
  name: string;
  typeFullName: string;
}

interface EntityTypeInfoDto extends AuditedEntityDto<string> {
  displayName: string;
  isAuditEnabled: boolean;
  name: string;
  properties: EntityPropertyInfoDto[];
  typeFullName: string;
}

interface GetEntityTypeInfoListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  isAuditEnabled?: boolean;
}

export type {
  EntityPropertyInfoDto,
  EntityTypeInfoDto,
  GetEntityTypeInfoListInput,
};
