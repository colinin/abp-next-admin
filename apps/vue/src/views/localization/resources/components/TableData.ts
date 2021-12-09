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
      title: L('DisplayName:Enable'),
      dataIndex: 'enable',
      align: 'left',
      width: 100,
      sorter: true,
      slots: {
        customRender: 'enable',
      },
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('DisplayName:Description'),
      dataIndex: 'description',
      align: 'left',
      width: 200,
      sorter: true,
    },
  ];
}
