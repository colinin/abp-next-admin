import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('LocalizationManagement');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Key'),
      dataIndex: 'key',
      align: 'left',
      width: 260,
      resizable: true,
      sorter: (last, next) => {
        return last.key.localeCompare(next.key);
      },
    },
    {
      title: L('DisplayName:Value'),
      dataIndex: 'value',
      align: 'left',
      width: 180,
      resizable: true,
      sorter: (last, next) => {
        return last.value?.localeCompare(next.value) ?? -1;
      },
    },
    {
      title: L('DisplayName:TargetValue'),
      dataIndex: 'targetValue',
      align: 'left',
      width: 200,
      resizable: true,
      sorter: (last, next) => {
        return last.targetValue?.localeCompare(next.targetValue) ?? -1;
      },
    },
    {
      title: L('DisplayName:ResourceName'),
      dataIndex: 'resourceName',
      align: 'left',
      width: 200,
      resizable: true,
      sorter: (last, next) => {
        return last.resourceName.localeCompare(next.resourceName);
      },
    },
  ];
}
