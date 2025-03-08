import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/core';

interface GdprRequestDto extends EntityDto<string> {
  /** 创建时间 */
  creationTime: string;
  /** 就绪时间 */
  readyTime: string;
}

interface GdprRequestGetListInput extends PagedAndSortedResultRequestDto {
  /** 创建时间 */
  creationTime?: string;
  /** 就绪时间 */
  readyTime?: string;
}

export type { GdprRequestDto, GdprRequestGetListInput };
