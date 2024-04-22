import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form';

const { L } = useLocalization(['Notifications', 'AbpUi']);

export function getSearchFormSchemas():FormSchema[] {
  return [
    {
      field: 'groupName',
      component: 'Select',
      label: L('DisplayName:GroupName'),
      colProps: { span: 6 },
      slot: 'groupName',
    },
    {
      field: 'notificationType',
      component: 'Select',
      label: L('DisplayName:NotificationType'),
      colProps: { span: 6 },
      slot: 'notificationType',
    },
    {
      field: 'template',
      component: 'Select',
      label: L('DisplayName:Template'),
      colProps: { span: 12 },
      slot: 'template',
    },
    {
      field: 'notificationLifetime',
      component: 'Select',
      label: L('DisplayName:NotificationLifetime'),
      colProps: { span: 6 },
      slot: 'notificationLifetime',
    },
    {
      field: 'contentType',
      component: 'Select',
      label: L('DisplayName:ContentType'),
      colProps: { span: 6 },
      slot: 'contentType',
    },
    {
      field: 'filter',
      component: 'Input',
      label: L('Search'),
      colProps: { span: 12 },
    },
  ];
}
