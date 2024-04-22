import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn } from '/@/components/Table';
import { sorter } from '/@/utils/table';

const { L } = useLocalization(['Notifications']);

export function getDataColumns(): BasicColumn[] {
  return [
    {
      title: L('DisplayName:Name'),
      dataIndex: 'name',
      align: 'left',
      width: 350,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'name'),
    },
    {
      title: L('DisplayName:DisplayName'),
      dataIndex: 'displayName',
      align: 'left',
      width: 180,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'displayName'),
    },
    {
      title: L('DisplayName:Description'),
      dataIndex: 'description',
      align: 'left',
      width: 250,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'description'),
    },
    {
      title: L('DisplayName:AllowSubscriptionToClients'),
      dataIndex: 'allowSubscriptionToClients',
      align: 'center',
      width: 150,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'allowSubscriptionToClients'),
    },
    {
      title: L('DisplayName:Template'),
      dataIndex: 'template',
      align: 'center',
      width: 150,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'template'),
    },
    {
      title: L('DisplayName:NotificationLifetime'),
      dataIndex: 'notificationLifetime',
      align: 'center',
      width: 150,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'notificationLifetime'),
    },
    {
      title: L('DisplayName:NotificationType'),
      dataIndex: 'notificationType',
      align: 'center',
      width: 150,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'notificationType'),
    },
    {
      title: L('DisplayName:ContentType'),
      dataIndex: 'contentType',
      align: 'center',
      width: 150,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'contentType'),
    },
    {
      title: L('DisplayName:Providers'),
      dataIndex: 'providers',
      align: 'center',
      width: 150,
      resizable: true,
      sorter: (a, b) => sorter(a, b, 'providers'),
    },
    {
      title: L('DisplayName:IsStatic'),
      dataIndex: 'isStatic',
      align: 'center',
      width: 150,
      resizable: true,
      defaultHidden: true,
      sorter: (a, b) => sorter(a, b, 'isStatic'),
    },
  ];
}
