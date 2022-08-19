import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpOpenIddict');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:ApplicationId'),
      dataIndex: 'applicationId',
      align: 'left',
      width: 300,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:Subject'),
      dataIndex: 'subject',
      align: 'left',
      width: 300,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:Type'),
      dataIndex: 'type',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('DisplayName:Status'),
      dataIndex: 'status',
      align: 'left',
      width: 200,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:CreationDate'),
      dataIndex: 'creationDate',
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
