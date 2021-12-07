import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';

const { L } = useLocalization('AbpIdentity');

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
      field: 'concurrencyStamp',
      component: 'Input',
      label: 'concurrencyStamp',
      colProps: { span: 24 },
      ifShow: false,
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:RoleName'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'isPublic',
      component: 'Checkbox',
      label: L('DisplayName:IsPublic'),
      colProps: { span: 24 },
      renderComponentContent: L('DisplayName:IsPublic'),
    },
    {
      field: 'isDefault',
      component: 'Checkbox',
      label: L('DisplayName:IsDefault'),
      colProps: { span: 24 },
      renderComponentContent: L('DisplayName:IsDefault'),
    },
  ];
}
