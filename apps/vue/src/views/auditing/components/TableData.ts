import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpAuditLogging');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('RequestUrl'),
      dataIndex: 'url',
      align: 'left',
      width: 500,
      sorter: true,
      slots: {
        customRender: 'request',
      },
    },
    {
      title: L('UserName'),
      dataIndex: 'userName',
      align: 'left',
      width: 120,
      sorter: true,
    },
    {
      title: L('ClientIpAddress'),
      dataIndex: 'clientIpAddress',
      align: 'left',
      width: 120,
      sorter: true,
    },
    {
      title: L('ExecutionTime'),
      dataIndex: 'executionTime',
      align: 'left',
      width: 150,
      sorter: true,
      format: (text) => {
        return formatToDateTime(text);
      },
    },
    {
      title: L('ExecutionDuration'),
      dataIndex: 'executionDuration',
      align: 'left',
      width: 100,
      sorter: true,
    },
    {
      title: L('ApplicationName'),
      dataIndex: 'applicationName',
      align: 'left',
      width: 100,
      sorter: true,
    },
    {
      title: L('TenantName'),
      dataIndex: 'tenantName',
      align: 'left',
      width: 100,
      sorter: true,
    },
    {
      title: L('BrowserInfo'),
      dataIndex: 'browserInfo',
      align: 'left',
      width: 300,
      sorter: true,
    },
  ];
}
