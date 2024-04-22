import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpAuditLogging');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('MachineName'),
      dataIndex: ['fields', 'machineName'],
      key: 'machineName',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('Application'),
      dataIndex: ['fields', 'application'],
      key: 'application',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('TimeStamp'),
      dataIndex: 'timeStamp',
      align: 'left',
      width: 150,
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
    },
    {
      title: L('Message'),
      dataIndex: 'message',
      align: 'left',
      width: 'auto',
    },
    {
      title: L('Environment'),
      dataIndex: ['fields', 'environment'],
      key: 'environment',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('ProcessId'),
      dataIndex: ['fields', 'processId'],
      key: 'processId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('ThreadId'),
      dataIndex: ['fields', 'threadId'],
      key: 'threadId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('Context'),
      dataIndex: ['fields', 'context'],
      key: 'context',
      align: 'left',
      width: 330,
      sorter: true,
    },
    {
      title: L('ConnectionId'),
      dataIndex: ['fields', 'connectionId'],
      key: 'connectionId',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('CorrelationId'),
      dataIndex: ['fields', 'correlationId'],
      key: 'correlationId',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('RequestId'),
      dataIndex: ['fields', 'requestId'],
      key: 'requestId',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('RequestPath'),
      dataIndex: ['fields', 'requestPath'],
      key: 'requestPath',
      align: 'left',
      width: 300,
      sorter: true,
    },
  ];
}
