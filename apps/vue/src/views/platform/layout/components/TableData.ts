import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { Layout } from '/@/api/platform/model/layoutModel';

const { L } = useLocalization('AppPlatform');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 100,
      ifShow: false,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      fixed: 'left',
      width: 150,
      sorter: (a: Layout, b: Layout) => a.name.localeCompare(b.name),
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      width: 180,
      sorter: (a: Layout, b: Layout) => a.displayName.localeCompare(b.displayName),
    },
    {
      title: L('DisplayName:Path'),
      dataIndex: 'path',
      width: 180,
      sorter: (a: Layout, b: Layout) => a.path.localeCompare(b.path),
    },
    {
      title: L('DisplayName:UIFramework'),
      dataIndex: 'framework',
      width: 180,
      sorter: (a: Layout, b: Layout) => a.framework.localeCompare(b.framework),
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
