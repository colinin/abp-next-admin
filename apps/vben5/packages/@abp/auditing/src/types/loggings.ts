interface LogExceptionDto {
  class?: string;
  depth?: number;
  helpUrl?: string;
  hResult?: number;
  message?: string;
  source?: string;
  stackTrace?: string;
}

interface LogFieldDto {
  actionId?: string;
  actionName?: string;
  application?: string;
  clientId?: string;
  connectionId?: string;
  context?: string;
  correlationId?: string;
  environment?: string;
  id: string;
  machineName?: string;
  processId?: number;
  requestId?: string;
  requestPath?: string;
  threadId?: number;
  userId?: string;
}

enum LogLevel {
  Critical = 5,
  Debug = 1,
  Error = 4,
  Information = 2,
  None = 6,
  Trace = 0,
  Warning = 3,
}

interface LogDto {
  exceptions: LogExceptionDto[];
  fields: LogFieldDto;
  level: LogLevel;
  message: string;
  timeStamp: Date;
}

interface LogGetListInput {
  application?: string;
  context?: string;
  correlationId?: string;
  endTime?: Date;
  environment?: string;
  hasException?: boolean;
  level?: LogLevel;
  machineName?: string;
  processId?: number;
  requestId?: string;
  requestPath?: string;
  startTime?: Date;
  threadId?: number;
}

export type { LogDto, LogExceptionDto, LogFieldDto, LogGetListInput };

export { LogLevel };
