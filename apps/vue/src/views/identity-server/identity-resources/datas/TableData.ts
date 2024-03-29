import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';

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
      title: L('Enabled'),
      dataIndex: 'enabled',
      align: 'center',
      width: 150,
      sorter: true,
    },
    {
      title: L('Required'),
      dataIndex: 'required',
      align: 'center',
      width: 150,
      sorter: true,
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
      title: L('Emphasize'),
      dataIndex: 'emphasize',
      align: 'center',
      width: 200,
      sorter: true,
    },
    {
      title: L('ShowInDiscoveryDocument'),
      dataIndex: 'showInDiscoveryDocument',
      align: 'center',
      width: 200,
      sorter: true,
    },
  ];
}
