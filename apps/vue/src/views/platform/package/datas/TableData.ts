import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization(['Platform', 'AbpUi']);

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Id'),
      dataIndex: 'id',
      align: 'left',
      width: 1,
      sorter: true,
      resizable: true,
      ifShow: false,
    },
    {
      title: L('DisplayName:CreationTime'),
      dataIndex: 'creationTime',
      align: 'left',
      width: 1,
      sorter: true,
      resizable: true,
      ifShow: false,
      format: (text) => {
        return text ? formatToDateTime(text) : text;
      }
    },
    {
      title: L('DisplayName:CreatorId'),
      dataIndex: 'creatorId',
      align: 'left',
      width: 1,
      sorter: true,
      resizable: true,
      ifShow: false,
    },
    {
      title: L('DisplayName:LastModificationTime'),
      dataIndex: 'lastModificationTime',
      align: 'left',
      width: 1,
      sorter: true,
      resizable: true,
      ifShow: false,
      format: (text) => {
        return text ? formatToDateTime(text) : text;
      }
    },
    {
      title: L('DisplayName:LastModifierId'),
      dataIndex: 'lastModifierId',
      align: 'left',
      width: 1,
      sorter: true,
      resizable: true,
      ifShow: false,
    },
    {
      title: L('DisplayName:ConcurrencyStamp'),
      dataIndex: 'concurrencyStamp',
      align: 'left',
      width: 1,
      sorter: true,
      resizable: true,
      ifShow: false,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 120,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:Note'),
      dataIndex: 'note',
      align: 'left',
      width: 120,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:Version'),
      dataIndex: 'version',
      align: 'left',
      width: 120,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:Description'),
      dataIndex: 'description',
      align: 'left',
      width: 120,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:ForceUpdate'),
      dataIndex: 'forceUpdate',
      align: 'left',
      width: 120,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:Authors'),
      dataIndex: 'authors',
      align: 'left',
      width: 120,
      sorter: true,
      resizable: true,
    },
  ];
}
