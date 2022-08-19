import { createVNode } from 'vue';
import { JsonPreview } from '/@/components/CodeEditor';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormSchema } from '/@/components/Form';

const { L } = useLocalization(['CachingManagement']);

export function getSearchFormSchemas(): FormSchema[] {
  return [
    {
      field: 'marker',
      component: 'Input',
      label: L('DisplayName:Marker'),
      colProps: { span: 6 },
    },
    {
      field: 'filter',
      component: 'Input',
      label: L('Search'),
      colProps: { span: 18 },
    },
  ];
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'key',
      component: 'InputTextArea',
      label: L('DisplayName:Key'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
        autoSize: {
          minRows: 5,
        },
      },
    },
    {
      field: 'type',
      component: 'Input',
      label: L('DisplayName:Type'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'size',
      component: 'Input',
      label: L('DisplayName:Size'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'expiration',
      component: 'Input',
      label: L('DisplayName:Ttl'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'values',
      component: 'Input',
      label: L('DisplayName:Values'),
      colProps: { span: 24 },
      render: ({ values }) => {
        return createVNode(JsonPreview, {
          data: values.values,
        });
      },
    },
  ];
}
