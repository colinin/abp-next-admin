import { BasicColumn } from '/@/components/Table/src/types/table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { GlobalConfiguration } from '/@/api/api-gateway/model/globalModel';

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
      title: L('DisplayName:AppId'),
      dataIndex: 'appId',
      width: 180,
      sorter: (a: GlobalConfiguration, b: GlobalConfiguration) => a.appId.localeCompare(b.appId),
    },
    {
      title: L('DisplayName:BaseUrl'),
      dataIndex: 'baseUrl',
      width: 180,
      sorter: (a: GlobalConfiguration, b: GlobalConfiguration) =>
        a.baseUrl.localeCompare(b.baseUrl),
    },
    {
      title: L('DisplayName:DownstreamScheme'),
      dataIndex: 'downstreamScheme',
      width: 180,
      sorter: (a: GlobalConfiguration, b: GlobalConfiguration) =>
        a.downstreamScheme?.localeCompare(b.downstreamScheme ?? ''),
    },
    {
      title: L('DisplayName:RequestIdKey'),
      dataIndex: 'requestIdKey',
      width: 180,
      sorter: (a: GlobalConfiguration, b: GlobalConfiguration) =>
        a.requestIdKey?.localeCompare(b.requestIdKey ?? ''),
    },
    {
      title: L('DisplayName:DownstreamHttpVersion'),
      dataIndex: 'downstreamHttpVersion',
      width: 100,
      sorter: (a: GlobalConfiguration, b: GlobalConfiguration) =>
        a.downstreamHttpVersion?.localeCompare(b.downstreamHttpVersion ?? ''),
    },
  ];
}
