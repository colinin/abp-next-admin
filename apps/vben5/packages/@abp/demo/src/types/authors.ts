import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/core';

interface AuthorDto extends EntityDto<string> {
  birthDate: string;
  name: string;
  shortBio?: string;
}

interface GetAuthorPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export type { AuthorDto, GetAuthorPagedListInput };
