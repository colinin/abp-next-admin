import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';

const { L } = useLocalization('AbpOpenIddict');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:ClientId'),
      dataIndex: 'clientId',
      align: 'left',
      width: 150,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:ClientSecret'),
      dataIndex: 'clientSecret',
      align: 'left',
      width: 200,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
    {
      title: L('DisplayName:ConsentType'),
      dataIndex: 'consentType',
      align: 'left',
      width: 200,
      sorter: true,
      resizable: true,
    },
  ];
}
