import type {
  ExtensibleAuditedEntityDto,
  IHasConcurrencyStamp,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface EditionDto
  extends ExtensibleAuditedEntityDto<string>,
    IHasConcurrencyStamp {
  /** 显示名称 */
  displayName: string;
}

interface EditionCreateOrUpdateBase {
  /** 显示名称 */
  displayName: string;
}

type EditionCreateDto = EditionCreateOrUpdateBase;

interface EditionUpdateDto
  extends EditionCreateOrUpdateBase,
    IHasConcurrencyStamp {}

interface GetEditionPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export type {
  EditionCreateDto,
  EditionDto,
  EditionUpdateDto,
  GetEditionPagedListInput,
};
