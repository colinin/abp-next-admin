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
      align: 'left',
      width: 200,
      sorter: true,
      slots: { customRender: 'enabled' },
    },
    {
      title: L('Client:Id'),
      dataIndex: 'clientId',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('Name'),
      dataIndex: 'clientName',
      align: 'left',
      width: 300,
      sorter: true,
    },
    {
      title: L('Description'),
      dataIndex: 'description',
      align: 'left',
      width: 300,
      sorter: true,
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
