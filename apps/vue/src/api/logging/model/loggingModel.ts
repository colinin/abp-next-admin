import { PagedAndSortedResultRequestDto, PagedResultDto } from '../../model/baseModel';

export interface LogException {
  depth: number;
  class: string;
  message: string;
  source: string;
  stackTrace: string;
  hResult: number;
  helpURL: string;
}

export interface LogField {
  id: string;
  machineName: string;
  environment: string;
  application: string;
  context: string;
  actionId: string;
  actionName: string;
  requestId: string;
  requestPath: string;
  connectionId: string;
  correlationId: string;
  clientId: string;
  userId: string;
  processId: number;
  threadId: number;
}

export enum LogLevel {
  Trace,
  Debug,
  Information,
  Warning,
  Error,
  Critical,
  None,
}

export interface Log {
  timeStamp: Date;
  level: LogLevel;
  message: string;
  fields: LogField;
  exceptions: LogException[];
}

export interface GetLogPagedRequest extends PagedAndSortedResultRequestDto {
  startTime?: Date;
  endTime?: Date;
  machineName?: string;
  environment?: string;
  application?: string;
  context?: string;
  requestId?: string;
  requestPath?: string;
  correlationId?: string;
  processId?: number;
  threadId?: number;
  hasException?: boolean;
}

export class LogPagedResult extends PagedResultDto<Log> {}
