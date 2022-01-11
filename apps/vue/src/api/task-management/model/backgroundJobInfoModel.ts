import { ExtensibleAuditedEntity, IHasConcurrencyStamp, PagedAndSortedResultRequestDto } from "../../model/baseModel";

export enum JobStatus {
  None = -1,
  Completed = 0,
  Running = 10,
  FailedRetry = 15,
  Paused = 20,
  Stopped = 30,
}

export enum JobType {
  Once,
  Period,
  Persistent,
}

export enum JobPriority {
  Low = 5,
  BelowNormal = 10,
  Normal = 0xF,
  AboveNormal = 20,
  High = 25
}

export interface BackgroundJobInfo extends ExtensibleAuditedEntity<string>, IHasConcurrencyStamp {
  isEnabled: boolean;
  name: string;
  group: string;
  type: string;
  result: string;
  args: ExtraPropertyDictionary,
  description?: string;
  beginTime: Date;
  endTime?: Date;
  lastRunTime?: Date;
  nextRunTime?: Date;
  jobType: JobType;
  cron: string;
  triggerCount: number;
  tryCount: number;
  maxTryCount: number;
  maxCount: number;
  isAbandoned: boolean;
  interval: number;
  priority: JobPriority;
  lockTimeOut: number;
}

interface BackgroundJobInfoCreateOrUpdate {
  isEnabled: boolean;
  args: ExtraPropertyDictionary;
  description?: string;
  beginTime: Date;
  endTime?: Date;
  jobType: JobType;
  cron: string;
  maxCount: number;
  interval: number;
  priority: JobPriority;
  lockTimeOut: number;
}

export interface BackgroundJobInfoCreate extends BackgroundJobInfoCreateOrUpdate {
  name: string;
  group: string;
  type: string;
}

export interface BackgroundJobInfoUpdate extends BackgroundJobInfoCreateOrUpdate, IHasConcurrencyStamp {
  
}

export interface BackgroundJobInfoGetListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  name?: string;
  group?: string;
  type?: string;
  status?: JobStatus;
  beginTime?: Date;
  endTime?: Date;
  beginLastRunTime?: Date;
  endLastRunTime?: Date;
  beginCreationTime?: Date;
  endCreationTime?: Date;
  isAbandoned?: boolean;
  jobType?: JobType;
  priority?: JobPriority;
}
