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
    {
      title: L('DisplayName:ClientType'),
      dataIndex: 'clientType',
      align: 'left',
      width: 120,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:ApplicationType'),
      dataIndex: 'applicationType',
      align: 'left',
      width: 110,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:ClientUri'),
      dataIndex: 'clientUri',
      align: 'left',
      width: 150,
      sorter: true,
      resizable: true,
    },
    {
      title: L('DisplayName:LogoUri'),
      dataIndex: 'logoUri',
      align: 'left',
      width: 120,
      sorter: true,
      resizable: true,
    },
  ];
}
