import { reactive } from 'vue';

import { $t } from '@vben/locales';

import { JobPriority, JobSource, JobStatus, JobType } from '../types/job-infos';

export function useJobEnumsMap() {
  const jobStatusMap = reactive<{
    [key: number]: string;
  }>({
    [JobStatus.Completed]: $t('TaskManagement.DisplayName:Completed'),
    [JobStatus.FailedRetry]: $t('TaskManagement.DisplayName:FailedRetry'),
    [JobStatus.None]: $t('TaskManagement.DisplayName:None'),
    [JobStatus.Paused]: $t('TaskManagement.DisplayName:Paused'),
    [JobStatus.Queuing]: $t('TaskManagement.DisplayName:Queuing'),
    [JobStatus.Running]: $t('TaskManagement.DisplayName:Running'),
    [JobStatus.Stopped]: $t('TaskManagement.DisplayName:Stopped'),
  });
  const jobStatusColor = reactive<{
    [key: number]: string;
  }>({
    [JobStatus.Completed]: '#339933',
    [JobStatus.FailedRetry]: '#FF6600',
    [JobStatus.None]: '',
    [JobStatus.Paused]: '#CC6633',
    [JobStatus.Queuing]: '#008B8B',
    [JobStatus.Running]: '#3399CC',
    [JobStatus.Stopped]: '#F00000',
  });

  const jobTypeMap = reactive<{
    [key: number]: string;
  }>({
    [JobType.Once]: $t('TaskManagement.DisplayName:Once'),
    [JobType.Period]: $t('TaskManagement.DisplayName:Period'),
    [JobType.Persistent]: $t('TaskManagement.DisplayName:Persistent'),
  });

  const jobPriorityMap = reactive<{
    [key: number]: string;
  }>({
    [JobPriority.AboveNormal]: $t('TaskManagement.DisplayName:AboveNormal'),
    [JobPriority.BelowNormal]: $t('TaskManagement.DisplayName:BelowNormal'),
    [JobPriority.High]: $t('TaskManagement.DisplayName:High'),
    [JobPriority.Low]: $t('TaskManagement.DisplayName:Low'),
    [JobPriority.Normal]: $t('TaskManagement.DisplayName:Normal'),
  });

  const jobPriorityColor = reactive<{
    [key: number]: string;
  }>({
    [JobPriority.AboveNormal]: 'orange',
    [JobPriority.BelowNormal]: 'cyan',
    [JobPriority.High]: 'red',
    [JobPriority.Low]: 'purple',
    [JobPriority.Normal]: 'blue',
  });

  const jobSourceMap = reactive<{
    [key: number]: string;
  }>({
    [JobSource.None]: $t('TaskManagement.DisplayName:None'),
    [JobSource.System]: $t('TaskManagement.DisplayName:System'),
    [JobSource.User]: $t('TaskManagement.DisplayName:User'),
  });

  return {
    jobPriorityColor,
    jobPriorityMap,
    jobSourceMap,
    jobStatusColor,
    jobStatusMap,
    jobTypeMap,
  };
}
