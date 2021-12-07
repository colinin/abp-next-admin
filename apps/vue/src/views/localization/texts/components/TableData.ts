import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('LocalizationManagement');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:Key'),
      dataIndex: 'key',
      align: 'left',
      width: 260,
      sorter: true,
    },
    {
      title: L('DisplayName:Value'),
      dataIndex: 'value',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('DisplayName:TargetValue'),
      dataIndex: 'targetValue',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('DisplayName:ResourceName'),
      dataIndex: 'resourceName',
      align: 'left',
      width: 200,
      sorter: true,
    },
  ];
}
