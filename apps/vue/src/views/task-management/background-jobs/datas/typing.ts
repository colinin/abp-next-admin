import { JobStatus, JobType, JobPriority } from '/@/api/task-management/model/backgroundJobInfoModel';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('TaskManagement');

export const JobStatusMap = {
  [JobStatus.None]: L('DisplayName:None'),
  [JobStatus.Completed]: L('DisplayName:Completed'),
  [JobStatus.Running]: L('DisplayName:Running'),
  [JobStatus.FailedRetry]: L('DisplayName:FailedRetry'),
  [JobStatus.Paused]: L('DisplayName:Paused'),
  [JobStatus.Stopped]: L('DisplayName:Stopped'),
}
export const JobStatusColor = {
  [JobStatus.None]: '',
  [JobStatus.Completed]: '#339933',
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
