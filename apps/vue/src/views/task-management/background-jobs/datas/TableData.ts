import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('TaskManagement');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:Group'),
      dataIndex: 'group',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 300,
      sorter: true,
    },
    {
      title: L('DisplayName:Type'),
      dataIndex: 'type',
      align: 'left',
      width: 350,
      sorter: true,
    },
    {
      title: L('DisplayName:CreationTime'),
      dataIndex: 'creationTime',
      align: 'left',
      width: 150,
      sorter: true,
      format: (text) => {
        return formatToDateTime(text);
      },
    },
    {
      title: L('DisplayName:Status'),
      dataIndex: 'status',
      align: 'left',
      width: 100,
      sorter: true,
      slots: {
        customRender: 'status',
      }
    },
    {
      title: L('DisplayName:Result'),
      dataIndex: 'result',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('DisplayName:LastRunTime'),
      dataIndex: 'lastRunTime',
      align: 'left',
      width: 150,
      sorter: true,
      format: (text) => {
        return text ? formatToDateTime(text) : '';
      },
    },
    {
      title: L('DisplayName:NextRunTime'),
      dataIndex: 'nextRunTime',
      align: 'left',
      width: 150,
      sorter: true,
      format: (text) => {
        return text ? formatToDateTime(text) : '';
      },
    },
    {
      title: L('DisplayName:JobType'),
      dataIndex: 'jobType',
      align: 'left',
      width: 150,
      sorter: true,
      slots: {
        customRender: 'type',
      }
    },
    {
      title: L('DisplayName:Priority'),
      dataIndex: 'priority',
      align: 'left',
      width: 150,
      sorter: true,
      slots: {
        customRender: 'priority',
      }
    },
    {
      title: L('DisplayName:Cron'),
      dataIndex: 'cron',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('DisplayName:TriggerCount'),
      dataIndex: 'triggerCount',
      align: 'left',
      width: 100,
      sorter: true,
    },
    {
      title: L('DisplayName:TryCount'),
      dataIndex: 'tryCount',
      align: 'left',
      width: 100,
      sorter: true,
    },
  ];
}
