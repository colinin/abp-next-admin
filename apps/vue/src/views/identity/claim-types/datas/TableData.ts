import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('AbpIdentity');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('IdentityClaim:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 220,
      sorter: true,
    },
    {
      title: L('IdentityClaim:ValueType'),
      dataIndex: 'valueType',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('IdentityClaim:Regex'),
      dataIndex: 'regex',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('IdentityClaim:Required'),
      dataIndex: 'required',
      align: 'center',
      width: 150,
      sorter: true,
    },
    {
      title: L('IdentityClaim:IsStatic'),
      dataIndex: 'isStatic',
      align: 'center',
      width: 150,
      sorter: true,
    },
    {
      title: L('IdentityClaim:Description'),
      dataIndex: 'description',
      align: 'left',
      width: 300,
      sorter: true,
    },
  ];
}
