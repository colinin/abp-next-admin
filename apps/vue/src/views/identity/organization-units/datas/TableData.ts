import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('AbpIdentity');

export function getMemberColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:RoleName'),
      dataIndex: 'name',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
}
