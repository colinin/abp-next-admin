import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { Route } from '/@/api/api-gateway/model/routeModel';

const { L } = useLocalization('ApiGateway');

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: 'reRouteId',
      dataIndex: 'reRouteId',
      width: 100,
      ifShow: false,
    },
    {
      title: L('DisplayName:Name'),
      dataIndex: 'reRouteName',
      align: 'left',
      width: 270,
      sorter: (a: Route, b: Route) => a.reRouteName.localeCompare(b.reRouteName),
    },
    {
      title: L('DisplayName:UpstreamPathTemplate'),
      dataIndex: 'upstreamPathTemplate',
      align: 'left',
      width: 300,
      sorter: (a: Route, b: Route) => a.upstreamPathTemplate.localeCompare(b.upstreamPathTemplate),
    },
    {
      title: L('DisplayName:DownstreamPathTemplate'),
      dataIndex: 'downstreamPathTemplate',
      align: 'left',
      width: 300,
      sorter: (a: Route, b: Route) =>
        a.downstreamPathTemplate.localeCompare(b.downstreamPathTemplate),
    },
    {
      title: L('DisplayName:UpstreamHttpMethod'),
      dataIndex: 'upstreamHttpMethod',
      width: 180,
      slots: {
        customRender: 'methods',
        style: {
          margin: '16px',
        },
      },
    },
    {
      title: L('DisplayName:DownstreamScheme'),
      dataIndex: 'downstreamScheme',
      width: 180,
      sorter: (a: Route, b: Route) => a.downstreamScheme?.localeCompare(b.downstreamScheme ?? ''),
    },
    {
      title: L('DisplayName:DownstreamHostAndPorts'),
      dataIndex: 'downstreamHostAndPorts',
      width: 180,
      slots: { customRender: 'hosts' },
    },
    {
      title: L('DisplayName:Timeout'),
      dataIndex: 'timeout',
      width: 150,
    },
    {
      title: L('DisplayName:ServiceName'),
      dataIndex: 'serviceName',
      width: 180,
      sorter: (a: Route, b: Route) => a.serviceName?.localeCompare(b.serviceName ?? ''),
    },
  ];
}
