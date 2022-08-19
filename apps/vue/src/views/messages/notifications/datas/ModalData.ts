import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { NotificationReadState } from '/@/api/messages/model/notificationsModel';

const { L } = useLocalization(['AbpMessageService', 'AbpUi']);

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'readState',
        component: 'Select',
        label: L('Notifications:State'),
        colProps: { span: 8 },
        defaultValue: NotificationReadState.UnRead,
        componentProps: {
          options: [
            { label: L('Read'), value: NotificationReadState.Read, },
            { label: L('UnRead'), value: NotificationReadState.UnRead, },
          ],
        },
      },
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 16 },
      },
    ],
  };
}
