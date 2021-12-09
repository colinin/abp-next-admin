import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpIdentityServer');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('Name'),
      dataIndex: 'name',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('DisplayName'),
      dataIndex: 'displayName',
      align: 'left',
      width: 300,
      sorter: true,
    },
    {
      title: L('Description'),
      dataIndex: 'description',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('Resource:Enabled'),
      dataIndex: 'enabled',
      align: 'left',
      width: 200,
      sorter: true,
      slots: { customRender: 'enabled' },
    },
    {
      title: L('ShowInDiscoveryDocument'),
      dataIndex: 'showInDiscoveryDocument',
      align: 'left',
      width: 200,
      sorter: true,
      slots: { customRender: 'discovery' },
    },
    {
      title: L('AllowedAccessTokenSigningAlgorithms'),
      dataIndex: 'allowedAccessTokenSigningAlgorithms',
      align: 'left',
      width: 180,
      sorter: true,
    },
  ];
}

export function getSecretColumns(): BasicColumn[] {
  return [
    {
      title: L('Secret:Type'),
      dataIndex: 'type',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('Secret:Value'),
      dataIndex: 'value',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('Description'),
      dataIndex: 'description',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('Expiration'),
      dataIndex: 'expiration',
      align: 'left',
      width: 180,
      sorter: true,
      format: (text) => {
        if (text) {
          return formatToDateTime(text);
        }
        return '';
      },
    },
  ];
}

export function getPropertyColumns(): BasicColumn[] {
  return [
    {
      title: L('Propertites:Key'),
      dataIndex: 'key',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('Propertites:Value'),
      dataIndex: 'value',
      align: 'left',
      width: 180,
      sorter: true,
    },
  ];
}
