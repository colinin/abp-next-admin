import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('LocalizationManagement');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 180,
      resizable: true,
      sorter: (last, next) => {
        return last.name.localeCompare(next.name);
      },
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      align: 'left',
      width: 180,
      resizable: true,
      sorter: (last, next) => {
        return last.displayName.localeCompare(next.displayName);
      },
    },
    {
      title: L('DisplayName:Description'),
      dataIndex: 'description',
      align: 'left',
      width: 200,
      resizable: true,
      sorter: (last, next) => {
        return last.description?.localeCompare(next.description) ?? -1;
      },
    },
  ];
}
