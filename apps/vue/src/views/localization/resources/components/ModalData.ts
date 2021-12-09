import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';

const { L } = useLocalization('LocalizationManagement', 'AbpUi');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 24 },
        defaultValue: '',
      },
    ],
  };
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      colProps: { span: 24 },
      ifShow: false,
    },
    {
      field: 'enable',
      component: 'Checkbox',
      label: L('DisplayName:Enable'),
      colProps: { span: 24 },
      defaultValue: true,
      renderComponentContent: L('DisplayName:Enable'),
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'displayName',
      component: 'Input',
      label: L('DisplayName:DisplayName'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'description',
      component: 'InputTextArea',
      label: L('DisplayName:Description'),
      colProps: { span: 24 },
    },
  ];
}
