import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';

const { L } = useLocalization('AbpSaas');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 120,
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

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      show: false,
      dynamicDisabled: true,
    },
    {
      field: 'concurrencyStamp',
      component: 'Input',
      label: 'concurrencyStamp',
      show: false,
      dynamicDisabled: true,
    },
    {
      field: 'displayName',
      component: 'Input',
      label: L('DisplayName:EditionName'),
      colProps: { span: 24 },
      required: true,
    },
  ];
}
