import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization(['AbpOpenIddict', 'AbpUi']);

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:ConcurrencyStamp'),
      dataIndex: 'concurrencyStamp',
      align: 'left',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 150,
      sorter: true,
      resizable: true,
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
      resizable: true,
    },
    {
      title: L('DisplayName:CreationDate'),
      dataIndex: 'creationTime',
      align: 'left',
      width: 200,
      sorter: true,
      resizable: true,
      format: (text?: string) => {
        return text ? formatToDateTime(text) : '';
      },
    },
  ];
}
