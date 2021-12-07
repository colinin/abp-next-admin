import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpIdentityServer');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('Client:Id'),
      dataIndex: 'clientId',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('Grants:Key'),
      dataIndex: 'key',
      align: 'left',
      width: 300,
      sorter: true,
    },
    {
      title: L('Grants:Type'),
      dataIndex: 'type',
      align: 'left',
      width: 180,
      sorter: true,
    },
    {
      title: L('Grants:SessionId'),
      dataIndex: 'sessionId',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('Grants:SubjectId'),
      dataIndex: 'subjectId',
      align: 'left',
      width: 300,
      sorter: true,
    },
    {
      title: L('Grants:ConsumedTime'),
      dataIndex: 'consumedTime',
      align: 'left',
      width: 180,
      sorter: true,
      format: (text) => {
        if (text) {
          return formatToDateTime(text);
        }
        return '';
      },
    },
    {
      title: L('CreationTime'),
      dataIndex: 'creationTime',
      align: 'left',
      width: 180,
      sorter: true,
      format: (text) => {
        return formatToDateTime(text);
      },
    },
    {
      title: L('Expiration'),
      dataIndex: 'expiration',
      align: 'left',
      width: 180,
      sorter: true,
      format: (text) => {
        if (text) {
          return formatToDateTime(text);
        }
        return '';
      },
    },
  ];
}
