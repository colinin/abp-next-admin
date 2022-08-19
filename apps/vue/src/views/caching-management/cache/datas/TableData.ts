import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';

const { L } = useLocalization('CachingManagement');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Key'),
      dataIndex: 'key',
      align: 'left',
      width: 'auto',
      sorter: (last, next) => {
        return last.key.localeCompare(next.key);
      },
    },
  ];
}
