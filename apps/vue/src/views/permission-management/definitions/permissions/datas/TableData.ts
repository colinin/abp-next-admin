import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { sorter } from '/@/utils/table';

const { L } = useLocalization(['AbpPermissionManagement']);

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:IsEnabled'),
      dataIndex: 'isEnabled',
      align: 'center',
      width: 150,
      resizable: true,
      defaultHidden: true,
      sorter: (a, b) => sorter(a, b, 'isEnabled'),
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 180,
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
      title: L('DisplayName:MultiTenancySide'),
      dataIndex: 'multiTenancySide',
      align: 'left',
      width: 80,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'multiTenancySide'),
    },
    {
      title: L('DisplayName:Providers'),
      dataIndex: 'providers',
      align: 'left',
      width: 80,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'providers'),
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
