import type { EntityDto, PagedAndSortedResultRequestDto } from '@abp/core';

interface BackgroundJobLogDto extends EntityDto<string> {
  exception?: string;
  jobGroup: string;
  jobName: string;
  jobType: string;
  message: string;
  runTime: string;
}

interface BackgroundJobLogGetListInput extends PagedAndSortedResultRequestDto {
  beginRunTime?: string;
  endRunTime?: string;
  filter?: string;
  group?: string;
  hasExceptions?: boolean;
  jobId?: string;
  name?: string;
  type?: string;
}

export type { BackgroundJobLogDto, BackgroundJobLogGetListInput };
