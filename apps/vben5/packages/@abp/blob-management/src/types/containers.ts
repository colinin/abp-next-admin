import type {
  ExtensibleAuditedEntityDto,
  IHasConcurrencyStamp,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface BlobContainerDto
  extends ExtensibleAuditedEntityDto<string>, IHasConcurrencyStamp {
  name: string;
}

interface BlobContainerCreateDto {
  name: string;
}

interface BlobContainerGetPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export type {
  BlobContainerCreateDto,
  BlobContainerDto,
  BlobContainerGetPagedListInput,
};
