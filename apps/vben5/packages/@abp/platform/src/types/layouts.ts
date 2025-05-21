import type { PagedAndSortedResultRequestDto } from '@abp/core';

import type { RouteDto } from './routes';

interface LayoutDto extends RouteDto {
  dataId: string;
  framework: string;
}

interface LayoutCreateOrUpdateDto {
  description?: string;
  displayName: string;
  name: string;
  path: string;
  redirect?: string;
}

interface LayoutCreateDto extends LayoutCreateOrUpdateDto {
  dataId: string;
  framework: string;
}

type LayoutUpdateDto = LayoutCreateOrUpdateDto;

interface LayoutGetPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  framework?: string;
}

export type {
  LayoutCreateDto,
  LayoutDto,
  LayoutGetPagedListInput,
  LayoutUpdateDto,
};
