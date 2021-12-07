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
      title: L('CreationTime'),
      dataIndex: 'creationTime',
      align: 'left',
      width: 150,
      sorter: true,
      format: (text) => {
        return formatToDateTime(text);
      },
    },
    {
      title: L('Actions'),
      dataIndex: 'action',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('ClientIpAddress'),
      dataIndex: 'clientIpAddress',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('BrowserInfo'),
      dataIndex: 'browserInfo',
      align: 'left',
      width: 270,
      sorter: true,
    },
    {
      title: L('ApplicationName'),
      dataIndex: 'applicationName',
      align: 'left',
      width: 140,
      sorter: true,
    },
    {
      title: L('TenantName'),
      dataIndex: 'tenantName',
      align: 'left',
      width: 120,
      sorter: true,
    },
    {
      title: L('Identity'),
      dataIndex: 'identity',
      align: 'left',
      width: 120,
      sorter: true,
    },
    {
      title: L('UserName'),
      dataIndex: 'userName',
      align: 'left',
      width: 120,
      sorter: true,
    },
    {
      title: L('ClientId'),
      dataIndex: 'clientId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('CorrelationId'),
      dataIndex: 'correlationId',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
}
