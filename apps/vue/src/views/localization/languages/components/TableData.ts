import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('AbpLocalization');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:CultureName'),
      dataIndex: 'cultureName',
      align: 'left',
      width: 180,
      resizable: true,
      sorter: (last, next) => {
        return last.cultureName.localeCompare(next.cultureName);
      },
    },
    {
      title: L('DisplayName:UiCultureName'),
      dataIndex: 'uiCultureName',
      align: 'left',
      width: 180,
      resizable: true,
      sorter: (last, next) => {
        return last.uiCultureName.localeCompare(next.uiCultureName);
      },
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      align: 'left',
      width: 200,
      resizable: true,
      sorter: (last, next) => {
        return last.displayName.localeCompare(next.displayName);
      },
    },
    {
      title: L('DisplayName:FlagIcon'),
      dataIndex: 'flagIcon',
      align: 'left',
      width: 150,
      resizable: true,
      sorter: (last, next) => {
        return last.flagIcon?.localeCompare(next.flagIcon) ?? -1;
      },
    },
  ];
}
