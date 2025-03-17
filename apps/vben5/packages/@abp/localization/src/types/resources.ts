import type { AuditedEntityDto } from '@abp/core';

interface ResourceDto extends AuditedEntityDto<string> {
  defaultCultureName?: string;
  description?: string;
  displayName: string;
  enable: boolean;
  name: string;
}

interface ResourceCreateOrUpdateDto {
  defaultCultureName?: string;
  description?: string;
  displayName: string;
  enable: boolean;
}

interface ResourceCreateDto extends ResourceCreateOrUpdateDto {
  name: string;
}

type ResourceUpdateDto = ResourceCreateOrUpdateDto;

interface ResourceGetListInput {
  filter?: string;
}

export type {
  ResourceCreateDto,
  ResourceDto,
  ResourceGetListInput,
  ResourceUpdateDto,
};
