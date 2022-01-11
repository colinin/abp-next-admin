import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { JobStatus, JobType, JobPriority } from '/@/api/task-management/model/backgroundJobInfoModel';
import { JobStatusMap, JobTypeMap, JobPriorityMap } from './typing';

const { L } = useLocalization('TaskManagement', 'AbpUi');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'group',
        component: 'Input',
        label: L('DisplayName:Group'),
        colProps: { span: 6 },
      },
      {
        field: 'name',
        component: 'Input',
        label: L('DisplayName:Name'),
        colProps: { span: 6 },
      },
      {
        field: 'type',
        component: 'Input',
        label: L('DisplayName:Type'),
        colProps: { span: 12 },
      },
      {
        field: 'status',
        component: 'Select',
        label: L('DisplayName:Status'),
        colProps: { span: 6 },
        componentProps: {
          options: [
            { label: JobStatusMap[JobStatus.None], value: JobStatus.None },
            { label: JobStatusMap[JobStatus.Running], value: JobStatus.Running },
            { label: JobStatusMap[JobStatus.Completed], value: JobStatus.Completed },
            { label: JobStatusMap[JobStatus.Paused], value: JobStatus.Paused },
            { label: JobStatusMap[JobStatus.Stopped], value: JobStatus.Stopped },
          ],
        },
      },
      {
        field: 'beginTime',
        component: 'DatePicker',
        label: L('DisplayName:BeginTime'),
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'endTime',
        component: 'DatePicker',
        label: L('DisplayName:EndTime'),
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'jobType',
        component: 'Select',
        label: L('DisplayName:JobType'),
        colProps: { span: 6 },
        componentProps: {
          options: [
            { label: JobTypeMap[JobType.Once], value: JobType.Once },
            { label: JobTypeMap[JobType.Period], value: JobType.Period },
            { label: JobTypeMap[JobType.Persistent], value: JobType.Persistent },
          ],
        },
      },
      {
        field: 'beginLastRunTime',
        component: 'DatePicker',
        label: L('DisplayName:BeginLastRunTime'),
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'endLastRunTime',
        component: 'DatePicker',
        label: L('DisplayName:EndLastRunTime'),
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'priority',
        component: 'Select',
        label: L('DisplayName:Priority'),
        colProps: { span: 6 },
        componentProps: {
          options: [
            { label: JobPriorityMap[JobPriority.Low], value: JobPriority.Low },
            { label: JobPriorityMap[JobPriority.BelowNormal], value: JobPriority.BelowNormal },
            { label: JobPriorityMap[JobPriority.Normal], value: JobPriority.Normal },
            { label: JobPriorityMap[JobPriority.AboveNormal], value: JobPriority.AboveNormal },
            { label: JobPriorityMap[JobPriority.High], value: JobPriority.High },
          ],
        },
      },
      {
        field: 'beginCreationTime',
        component: 'DatePicker',
        label: L('DisplayName:BeginCreationTime'),
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'endCreationTime',
        component: 'DatePicker',
        label: L('DisplayName:EndCreationTime'),
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'isAbandoned',
        component: 'Checkbox',
        label: L('DisplayName:IsAbandoned'),
        colProps: { span: 4 },
        renderComponentContent: L('DisplayName:IsAbandoned'),
      },
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 20 },
      },
    ],
  };
}
