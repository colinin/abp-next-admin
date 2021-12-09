import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';

const { L } = useLocalization('ApiGateway');

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

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      colProps: { span: 24 },
      show: false,
    },
    {
      field: 'isActive',
      component: 'Switch',
      label: L('DisplayName:IsActive'),
      colProps: { span: 24 },
      defaultValue: true,
    },
    {
      field: 'appId',
      component: 'Input',
      label: L('DisplayName:AppId'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'appName',
      component: 'Input',
      label: L('DisplayName:AppName'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'appIpAddress',
      component: 'Input',
      label: L('DisplayName:AppAddress'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
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
