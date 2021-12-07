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
      title: L('Required'),
      dataIndex: 'required',
      align: 'left',
      width: 200,
      sorter: true,
      slots: { customRender: 'required' },
    },
    {
      title: L('Enabled'),
      dataIndex: 'enabled',
      align: 'left',
      width: 200,
      sorter: true,
      slots: { customRender: 'enabled' },
    },
    {
      title: L('Emphasize'),
      dataIndex: 'emphasize',
      align: 'left',
      width: 200,
      sorter: true,
      slots: { customRender: 'emphasize' },
    },
    {
      title: L('ShowInDiscoveryDocument'),
      dataIndex: 'showInDiscoveryDocument',
      align: 'left',
      width: 200,
      sorter: true,
      slots: { customRender: 'discovery' },
    },
  ];
}
