import type {
  AuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface LanguageDto extends AuditedEntityDto<string> {
  cultureName: string;
  displayName: string;
  twoLetterISOLanguageName?: string;
  uiCultureName: string;
}

interface LanguageGetPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface LanguageCreateOrUpdateDto {
  displayName: string;
}

interface LanguageCreateDto extends LanguageCreateOrUpdateDto {
  cultureName: string;
  uiCultureName?: string;
}

type LanguageUpdateDto = LanguageCreateOrUpdateDto;

export type {
  LanguageCreateDto,
  LanguageDto,
  LanguageGetPagedListInput,
  LanguageUpdateDto,
};
