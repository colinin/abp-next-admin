import type {
  AuditedEntityDto,
  IHasConcurrencyStamp,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface TemplatePermissionDto {
  copyable: boolean;
  deletable: boolean;
  editable: boolean;
}

interface TemplateDto extends AuditedEntityDto<string>, IHasConcurrencyStamp {
  data?: Record<string, any>;
  description?: string;
  ext: Record<string, any>;
  name: string;
  permissions: TemplatePermissionDto;
  updatedAt?: number;
}

interface TemplateCreateOrUpdateDto {
  data?: Record<string, any>;
  description?: string;
  ext: Record<string, any>;
  name: string;
}

type TemplateCreateDto = TemplateCreateOrUpdateDto;

interface TemplateUpdateDto
  extends IHasConcurrencyStamp, TemplateCreateOrUpdateDto {}

interface TemplateDeleteDto {
  success: boolean;
}

interface TemplateGetPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export type {
  TemplateCreateDto,
  TemplateDeleteDto,
  TemplateDto,
  TemplateGetPagedListInput,
  TemplateUpdateDto,
};
