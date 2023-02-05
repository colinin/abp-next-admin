import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';

const { L } = useLocalization(['Platform', 'AbpUi']);

export function getSearchFormProps(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: {span: 24},
      },
      {
        field: 'name',
        component: 'Input',
        label: L('DisplayName:Name'),
        colProps: {span: 8},
      },
      {
        field: 'note',
        component: 'Input',
        label: L('DisplayName:Note'),
        colProps: {span: 8},
      },
      {
        field: 'version',
        component: 'Input',
        label: L('DisplayName:Version'),
        colProps: {span: 8},
      },
      {
        field: 'description',
        component: 'Input',
        label: L('DisplayName:Description'),
        colProps: {span: 8},
      },
      {
        field: 'forceUpdate',
        component: 'Input',
        label: L('DisplayName:ForceUpdate'),
        colProps: {span: 8},
      },
      {
        field: 'authors',
        component: 'Input',
        label: L('DisplayName:Authors'),
        colProps: {span: 8},
      }
    ],
  };
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'note',
      component: 'Input',
      label: L('DisplayName:Note'),
      colProps: {span: 24},
      componentProps: {},
    },
    {
      field: 'description',
      component: 'Input',
      label: L('DisplayName:Description'),
      colProps: {span: 24},
      componentProps: {},
    },
    {
      field: 'forceUpdate',
      component: 'Input',
      label: L('DisplayName:ForceUpdate'),
      colProps: {span: 24},
      componentProps: {},
    },
    {
      field: 'authors',
      component: 'Input',
      label: L('DisplayName:Authors'),
      colProps: {span: 24},
      componentProps: {},
    },
    {
      field: 'name',
      component: 'Input',
      label: L('DisplayName:Name'),
      colProps: {span: 24},
      componentProps: {},
    },
    {
      field: 'version',
      component: 'Input',
      label: L('DisplayName:Version'),
      colProps: {span: 24},
      componentProps: {},
    },
    {
      field: 'id',
      component: 'Input',
      label: 'id',
      show: false,
      dynamicDisabled: true,
      colProps: {span: 24},
      componentProps: {},
    },
    {
      field: 'concurrencyStamp',
      component: 'Input',
      label: 'concurrencyStamp',
      show: false,
      dynamicDisabled: true,
      colProps: {span: 24},
      componentProps: {},
    },
  ];
}
