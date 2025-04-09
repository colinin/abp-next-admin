import type {
  AuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

enum BookType {
  Undefined,
  Adventure,
  Biography,
  Dystopia,
  Fantastic,
  Horror,
  Science,
  ScienceFiction,
  Poetry,
}

interface BookDto extends AuditedEntityDto<string> {
  authorId: string;
  authorName: string;
  name: string;
  price?: number;
  publishDate: string;
  type: BookType;
}

interface CreateUpdateBookDto {
  authorId: string;
  name: string;
  price: number;
  publishDate: string;
  type: BookType;
}

interface GetBookPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export { BookType };

export type { BookDto, CreateUpdateBookDto, GetBookPagedListInput };
