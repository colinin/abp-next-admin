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
      title: L('DisplayName:RoleName'),
      dataIndex: 'name',
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
