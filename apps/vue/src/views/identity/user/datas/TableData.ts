import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { formatToDateTime } from '/@/utils/dateUtil';

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
      width: 180,
      sorter: true,
    },
    {
      title: L('LockoutEnd'),
      dataIndex: 'lockoutEnd',
      align: 'left',
      width: 180,
      sorter: true,
      format: (text) => {
        return text ? formatToDateTime(text) : '';
      },
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
      sorter: (last, next) => {
        return last.claimType.localeCompare(next.claimType);
      },
    },
    {
      title: L('DisplayName:ClaimValue'),
      dataIndex: 'claimValue',
      align: 'left',
      width: 'auto',
      sorter: (last, next) => {
        return last.claimValue.localeCompare(next.claimValue);
      },
    },
  ];
}
