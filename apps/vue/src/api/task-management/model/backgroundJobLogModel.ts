import { PagedAndSortedResultRequestDto } from '/@/api/model/baseModel';

export interface BackgroundJobLog {
  id: number;
  jobName: string;
  jobGroup: string;
  jobType: string;
  message: string;
  runTime: Date;
  exception?: string;
}

export interface BackgroundJobLogGetListInput extends PagedAndSortedResultRequestDto {
  jobId?: string;
  filter?: string;
  hasExceptions?: boolean;
  name?: string;
  group?: string;
  type?: string;
  beginRunTime?: Date;
  endRunTime?: Date;
}
