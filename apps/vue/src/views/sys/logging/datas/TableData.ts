import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpAuditLogging');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('MachineName'),
      dataIndex: 'fields.machineName',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('Application'),
      dataIndex: 'fields.application',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('TimeStamp'),
      dataIndex: 'timeStamp',
      align: 'left',
      width: 'auto',
      sorter: true,
      format: (text) => {
        return text ? formatToDateTime(text, 'YYYY-MM-DD HH:mm:ss') : text;
      },
    },
    {
      title: L('Level'),
      dataIndex: 'level',
      align: 'left',
      width: 150,
      sorter: true,
      slots: {
        customRender: 'level',
      },
    },
    {
      title: L('Message'),
      dataIndex: 'message',
      align: 'left',
      width: 150,
    },
    {
      title: L('Environment'),
      dataIndex: 'fields.environment',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('ProcessId'),
      dataIndex: 'fields.processId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('ThreadId'),
      dataIndex: 'fields.threadId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('Context'),
      dataIndex: 'fields.context',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('ConnectionId'),
      dataIndex: 'fields.connectionId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('CorrelationId'),
      dataIndex: 'fields.correlationId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('RequestId'),
      dataIndex: 'fields.requestId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('RequestPath'),
      dataIndex: 'fields.requestPath',
      align: 'left',
      width: 150,
      sorter: true,
    },
  ];
}
