import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { Menu } from '/@/api/platform/model/menuModel';

const { L } = useLocalization('AppPlatform');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: 'code',
      dataIndex: 'code',
      width: 1,
      ifShow: false,
    },
    {
      title: 'layoutId',
      dataIndex: 'layoutId',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 150,
      sorter: (a: Menu, b: Menu) => a.name.localeCompare(b.name),
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      width: 180,
      sorter: (a: Menu, b: Menu) => a.displayName.localeCompare(b.displayName),
    },
    {
      title: L('DisplayName:Path'),
      dataIndex: 'path',
      width: 180,
      sorter: (a: Menu, b: Menu) => a.path.localeCompare(b.path),
    },
    {
      title: L('DisplayName:Component'),
      dataIndex: 'component',
      width: 180,
      sorter: (a: Menu, b: Menu) => a.component.localeCompare(b.component),
    },
    {
      title: L('DisplayName:UIFramework'),
      dataIndex: 'framework',
      width: 180,
      sorter: (a: Menu, b: Menu) => a.framework.localeCompare(b.framework),
    },
    {
      title: L('DisplayName:Description'),
      dataIndex: 'description',
      width: 250,
    },
    {
      title: L('DisplayName:Redirect'),
      dataIndex: 'redirect',
      width: 'auto',
    },
  ];
}
