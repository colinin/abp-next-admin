import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { RouteGroup } from '/@/api/api-gateway/model/groupModel';

const { L } = useLocalization('ApiGateway');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'id',
      dataIndex: 'id',
      width: 100,
      ifShow: false,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      fixed: 'left',
      width: 150,
      sorter: (a: RouteGroup, b: RouteGroup) => a.name.localeCompare(b.name),
    },
    {
      title: L('DisplayName:AppId'),
      dataIndex: 'appId',
      width: 180,
      sorter: (a: RouteGroup, b: RouteGroup) => a.appId.localeCompare(b.appId),
    },
    {
      title: L('DisplayName:AppName'),
      dataIndex: 'appName',
      width: 180,
      sorter: (a: RouteGroup, b: RouteGroup) => a.appName.localeCompare(b.appName),
    },
    {
      title: L('DisplayName:AppAddress'),
      dataIndex: 'appIpAddress',
      width: 180,
      sorter: (a: RouteGroup, b: RouteGroup) => a.appIpAddress?.localeCompare(b.appIpAddress ?? ''),
    },
    {
      title: L('DisplayName:IsActive'),
      dataIndex: 'isActive',
      width: 100,
      slots: { customRender: 'active' },
    },
    {
      title: L('DisplayName:Description'),
      dataIndex: 'description',
      width: 250,
    },
  ];
}
