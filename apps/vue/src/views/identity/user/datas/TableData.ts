import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('AbpIdentity');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:UserName'),
      dataIndex: 'userName',
      align: 'left',
      width: 280,
      sorter: true,
    },
    {
      title: L('EmailAddress'),
      dataIndex: 'email',
      align: 'left',
      width: 260,
      sorter: true,
    },
    {
      title: L('DisplayName:Surname'),
      dataIndex: 'surname',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 270,
      sorter: true,
    },
    {
      title: L('PhoneNumber'),
      dataIndex: 'phoneNumber',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
}

export function getClaimColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:ClaimType'),
      dataIndex: 'claimType',
      align: 'left',
      width: 150,
      sortOrder: true,
    },
    {
      title: L('DisplayName:ClaimValue'),
      dataIndex: 'claimValue',
      align: 'left',
      width: 'auto',
      sortOrder: true,
    },
  ];
}
