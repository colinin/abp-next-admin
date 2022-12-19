import { createVNode } from 'vue';
import { Tag, Checkbox } from 'ant-design-vue';
import { DescItem } from "/@/components/Description";
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { JobStatusMap, JobStatusColor, JobTypeMap, JobPriorityMap, JobSourceMap } from './typing';

const { L } = useLocalization(['TaskManagement']);

export function getDescriptionSchemas() : DescItem[] {
  return [
    {
      label: L('DisplayName:Source'),
      field: 'source',
      render: (val) => {
        return JobSourceMap[val];
      },
      span: 1.5,
    },
    {
      label: L('DisplayName:IsEnabled'),
      field: 'isEnabled',
      render: (val) => createVNode(
        Checkbox,
        {
          checked: val,
          disabled: true,
        }),
      span: 1.5,
    },
    {
      label: L('DisplayName:Group'),
      field: 'group',
    },
    {
      label: L('DisplayName:Name'),
      field: 'name',
    },
    {
      label: L('DisplayName:Type'),
      field: 'type',
    },
    {
      label: L('DisplayName:CreationTime'),
      field: 'creationTime',
    },
    {
      label: L('DisplayName:BeginTime'),
      field: 'beginTime',
    },
    {
      label: L('DisplayName:EndTime'),
      field: 'endTime',
    },
    {
      label: L('DisplayName:LockTimeOut'),
      field: 'lockTimeOut',
      span: 1,
    },
    {
      label: L('DisplayName:Description'),
      field: 'description',
      span: 2,
    },
    {
      label: L('DisplayName:Cron'),
      field: 'cron',
      span: 1,
    },
    {
      label: L('DisplayName:LastRunTime'),
      field: 'lastRunTime',
      span: 1,
    },
    {
      label: L('DisplayName:NextRunTime'),
      field: 'nextRunTime',
      span: 1,
    },
    {
      label: L('DisplayName:Status'),
      field: 'status',
      render: (val) => createVNode(
        Tag,
        {
          color: JobStatusColor[val]
        },
        {
          default: () => JobStatusMap[val]
        }),
      span: 1,
    },
    {
      label: L('DisplayName:JobType'),
      field: 'jobType',
      render: (val) => {
        return JobTypeMap[val];
      },
      span: 1,
    },
    {
      label: L('DisplayName:Priority'),
      field: 'priority',
      render: (val) => {
        return JobPriorityMap[val];
      },
      span: 1,
    },
    {
      label: L('DisplayName:TriggerCount'),
      field: 'triggerCount',
      span: 0.75,
    },
    {
      label: L('DisplayName:MaxCount'),
      field: 'maxCount',
      span: 0.75,
    },
    {
      label: L('DisplayName:TryCount'),
      field: 'tryCount',
      span: 0.75,
    },
    {
      label: L('DisplayName:MaxTryCount'),
      field: 'maxTryCount',
      span: 0.75,
    },
  ];
}
