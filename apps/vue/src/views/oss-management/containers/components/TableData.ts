import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpOssManagement');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
    {
      title: L('DisplayName:CreationDate'),
      dataIndex: 'creationDate',
      align: 'left',
      width: 150,
      sorter: true,
      format: (text) => {
        return text ? formatToDateTime(text, 'YYYY-MM-DD') : text;
      },
    },
    {
      title: L('DisplayName:LastModifiedDate'),
      dataIndex: 'lastModifiedDate',
      align: 'left',
      width: 150,
      sorter: true,
      format: (text) => {
        return text ? formatToDateTime(text, 'YYYY-MM-DD') : text;
      },
    },
    {
      title: L('DisplayName:Size'),
      dataIndex: 'size',
      align: 'left',
      width: 120,
      sorter: true,
    },
  ];
}
