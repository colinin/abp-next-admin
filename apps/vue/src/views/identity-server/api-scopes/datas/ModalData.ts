import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';

const { L } = useLocalization('AbpIdentityServer');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 24 },
      },
    ],
  };
}

export function getPropertyFormSchemas(): FormSchema[] {
  return [
    {
      field: 'key',
      component: 'Input',
      label: L('Propertites:Key'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'value',
      component: 'Input',
      label: L('Propertites:Value'),
      colProps: { span: 24 },
      required: true,
    },
  ];
}
