import { JobStatus, JobType, JobPriority, JobSource } from '/@/api/task-management/jobs/model';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('TaskManagement');

export const JobStatusMap = {
  [JobStatus.None]: L('DisplayName:None'),
  [JobStatus.Completed]: L('DisplayName:Completed'),
  [JobStatus.Queuing]: L('DisplayName:Queuing'),
  [JobStatus.Running]: L('DisplayName:Running'),
  [JobStatus.FailedRetry]: L('DisplayName:FailedRetry'),
  [JobStatus.Paused]: L('DisplayName:Paused'),
  [JobStatus.Stopped]: L('DisplayName:Stopped'),
}
export const JobStatusColor = {
  [JobStatus.None]: '',
  [JobStatus.Completed]: '#339933',
  [JobStatus.Queuing]: '#008B8B',
  [JobStatus.Running]: '#3399CC',
  [JobStatus.FailedRetry]: '#FF6600',
  [JobStatus.Paused]: '#CC6633',
  [JobStatus.Stopped]: '#F00000',
}

export const JobTypeMap = {
  [JobType.Once]: L('DisplayName:Once'),
  [JobType.Period]: L('DisplayName:Period'),
  [JobType.Persistent]: L('DisplayName:Persistent'),
}

export const JobPriorityMap = {
  [JobPriority.Low]: L('DisplayName:Low'),
  [JobPriority.BelowNormal]: L('DisplayName:BelowNormal'),
  [JobPriority.Normal]: L('DisplayName:Normal'),
  [JobPriority.AboveNormal]: L('DisplayName:AboveNormal'),
  [JobPriority.High]: L('DisplayName:High'),
}

export const JobPriorityColor = {
  [JobPriority.Low]: 'purple',
  [JobPriority.BelowNormal]: 'cyan',
  [JobPriority.Normal]: 'blue',
  [JobPriority.AboveNormal]: 'orange',
  [JobPriority.High]: 'red',
}

export const JobSourceMap = {
  [JobSource.None]: L('DisplayName:None'),
  [JobSource.User]: L('DisplayName:User'),
  [JobSource.System]: L('DisplayName:System'),
}
