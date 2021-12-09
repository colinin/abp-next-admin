import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { getActivedList } from '/@/api/api-gateway/group';

const { L } = useLocalization('ApiGateway');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'appId',
        component: 'ApiSelect',
        label: L('DisplayName:AppId'),
        colProps: { span: 12 },
        required: true,
        componentProps: {
          api: () => getActivedList(),
          resultField: 'items',
          labelField: 'appName',
          valueField: 'appId',
        },
      },
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 12 },
        defaultValue: '',
      },
    ],
  };
}
