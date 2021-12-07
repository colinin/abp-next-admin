import { h } from 'vue';
import { Input } from 'ant-design-vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { CodeEditor } from '/@/components/CodeEditor';
import { FormProps, FormSchema } from '/@/components/Form';
import { formatToDateTime } from '/@/utils/dateUtil';

const { L } = useLocalization('AbpIdentityServer');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 12 },
      },
      {
        field: 'subjectId',
        component: 'Input',
        label: L('Grants:SubjectId'),
        colProps: { span: 12 },
      },
    ],
  };
}

export function getModalFormSchemas(): FormSchema[] {
  return [
    {
      field: 'key',
      component: 'Input',
      label: L('Grants:Key'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(Input, {
          value: model[field],
          readonly: true,
          placeholder: '',
        });
      },
    },
    {
      field: 'type',
      component: 'Input',
      label: L('Grants:Type'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(Input, {
          value: model[field],
          readonly: true,
          placeholder: '',
        });
      },
    },
    {
      field: 'subjectId',
      component: 'Input',
      label: L('Grants:SubjectId'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(Input, {
          value: model[field],
          readonly: true,
          placeholder: '',
        });
      },
    },
    {
      field: 'sessionId',
      component: 'Input',
      label: L('Grants:SessionId'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(Input, {
          value: model[field],
          readonly: true,
          placeholder: '',
        });
      },
    },
    {
      field: 'description',
      component: 'Input',
      label: L('Description'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(Input, {
          value: model[field],
          readonly: true,
          placeholder: '',
        });
      },
    },
    {
      field: 'creationTime',
      component: 'Input',
      label: L('CreationTime'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(Input, {
          value: model[field]
            ? formatToDateTime(model[field], 'YYYY-MM-DD HH:mm:ss')
            : model[field],
          readonly: true,
          placeholder: '',
        });
      },
    },
    {
      field: 'expiration',
      component: 'Input',
      label: L('Expiration'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(Input, {
          value: model[field]
            ? formatToDateTime(model[field], 'YYYY-MM-DD HH:mm:ss')
            : model[field],
          readonly: true,
          placeholder: '',
        });
      },
    },
    {
      field: 'consumedTime',
      component: 'Input',
      label: L('Grants:ConsumedTime'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(Input, {
          value: model[field]
            ? formatToDateTime(model[field], 'YYYY-MM-DD HH:mm:ss')
            : model[field],
          readonly: true,
          placeholder: '',
        });
      },
    },
    {
      field: 'data',
      component: 'Input',
      label: L('Grants:Data'),
      colProps: { span: 24 },
      render: ({ model, field }) => {
        return h(CodeEditor, {
          value: model[field],
          readonly: true,
        });
      },
    },
  ];
}
