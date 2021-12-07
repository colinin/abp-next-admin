import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';

const { L } = useLocalization('ApiGateway');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'reRouteId',
      dataIndex: 'reRouteId',
      width: 1,
      ifShow: false,
    },
    {
      title: 'appId',
      dataIndex: 'appId',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 250,
      sorter: true,
    },
    {
      title: L('DisplayName:UpstreamPathTemplate'),
      dataIndex: 'upstreamPathTemplate',
      align: 'left',
      width: 300,
      sorter: true,
    },
    {
      title: L('DisplayName:RouteKeys'),
      dataIndex: 'reRouteKeys',
      align: 'left',
      width: 180,
      slots: {
        customRender: 'keys',
        style: {
          margin: '16px',
        },
      },
    },
    {
      title: L('DisplayName:UpstreamHost'),
      dataIndex: 'upstreamHost',
      align: 'left',
      width: 200,
      sorter: true,
    },
    {
      title: L('DisplayName:Aggregator'),
      dataIndex: 'aggregator',
      width: 200,
      sorter: true,
    },
    {
      title: L('DisplayName:Priority'),
      dataIndex: 'priority',
      width: 100,
    },
  ];
}

export function getConfigDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:RouteKey'),
      dataIndex: 'reRouteKey',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('DisplayName:Parameter'),
      dataIndex: 'parameter',
      align: 'left',
      width: 150,
      sorter: true,
    },
    {
      title: L('DisplayName:JsonPath'),
      dataIndex: 'jsonPath',
      align: 'left',
      width: 200,
      sorter: true,
    },
  ];
}
