import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { sorter } from '/@/utils/table';

const { L } = useLocalization(['Notifications']);

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 350,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'name'),
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      align: 'left',
      width: 180,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'displayName'),
    },
    {
      title: L('DisplayName:Description'),
      dataIndex: 'description',
      align: 'left',
      width: 280,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'description'),
    },
    {
      title: L('DisplayName:IsStatic'),
      dataIndex: 'isStatic',
      align: 'center',
      width: 150,
      resizable: true,
      defaultHidden: true,
      sorter: (a, b) => sorter(a, b, 'isStatic'),
    },
  ];
}
