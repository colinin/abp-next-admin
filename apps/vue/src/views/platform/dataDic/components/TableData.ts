import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { valueTypeMaps } from './BasicType';
import { DataItem } from '/@/api/platform/model/dataItemModel';

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
      width: 120,
      sorter: (a: DataItem, b: DataItem) => a.name.localeCompare(b.name),
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      width: 120,
      sorter: (a: DataItem, b: DataItem) => a.displayName.localeCompare(b.displayName),
    },
    {
      title: L('DisplayName:Description'),
      dataIndex: 'description',
      width: 150,
    },
    {
      title: L('DisplayName:ValueType'),
      dataIndex: 'valueType',
      width: 100,
      format: (value) => {
        return valueTypeMaps[value];
      },
    },
    {
      title: L('DisplayName:DefaultValue'),
      dataIndex: 'defaultValue',
      width: 100,
    },
    {
      title: L('DisplayName:AllowBeNull'),
      dataIndex: 'allowBeNull',
      width: 100,
      slots: { customRender: 'allow' },
    },
  ];
}
