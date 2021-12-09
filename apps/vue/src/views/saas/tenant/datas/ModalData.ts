import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';

const { L } = useLocalization('AbpTenantManagement');

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
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:TenantName'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'adminEmailAddress',
      component: 'Input',
      label: L('DisplayName:AdminEmailAddress'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
    },
    {
      field: 'adminPassword',
      component: 'InputPassword',
      label: L('DisplayName:AdminPassword'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
    },
    {
      field: 'useSharedDatabase',
      component: 'Checkbox',
      label: '',
      colProps: { span: 24 },
      defaultValue: true,
      ifShow: ({ values }) => {
        return values.id ? false : true;
      },
      renderComponentContent: L('DisplayName:UseSharedDatabase'),
    },
    {
      field: 'defaultConnectionString',
      component: 'InputTextArea',
      label: L('DisplayName:DefaultConnectionString'),
      colProps: { span: 24 },
      required: true,
      ifShow: ({ values }) => {
        return !values.useSharedDatabase;
      },
      componentProps: {
        rows: 4,
      },
    },
  ];
}

export function getConnectionFormSchemas(): FormSchema[] {
  return [
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'value',
      component: 'Input',
      label: L('DisplayName:Value'),
      colProps: { span: 24 },
      required: true,
    },
  ];
}
