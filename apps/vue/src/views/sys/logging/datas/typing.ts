import { LogLevel } from '/@/api/logging/model/loggingModel';

export const LogLevelColor: { [key: number]: string } = {
  [LogLevel.Trace]: 'purple',
  [LogLevel.Debug]: 'blue',
  [LogLevel.Information]: 'green',
  [LogLevel.Warning]: 'orange',
  [LogLevel.Error]: 'red',
  [LogLevel.Critical]: '#f50',
  [LogLevel.None]: '',
};

export const LogLevelLabel: { [key: number]: string } = {
  [LogLevel.Trace]: 'Trace',
  [LogLevel.Debug]: 'Debug',
  [LogLevel.Information]: 'Information',
  [LogLevel.Warning]: 'Warning',
  [LogLevel.Error]: 'Error',
  [LogLevel.Critical]: 'Critical',
  [LogLevel.None]: 'None',
};
