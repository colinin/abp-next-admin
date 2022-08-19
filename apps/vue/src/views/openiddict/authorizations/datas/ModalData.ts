import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps, FormSchema } from '/@/components/Form';
import { getList as getApplications } from '/@/api/openiddict/applications';

const { L } = useLocalization(['AbpOpenIddict']);

export function getSearchFormProps(): Partial<FormProps> {
  return {
    labelWidth: 100,
    fieldMapToTime: [
      ['creationTime', ['beginCreationTime', 'endCreationTime'], 'YYYY-MM-DD'],
    ],
    schemas: [
      {
        field: 'subject',
        component: 'Input',
        label: L('DisplayName:Subject'),
        colProps: { span: 8 },
      },
      {
        field: 'status',
        component: 'Input',
        label: L('DisplayName:Status'),
        colProps: { span: 8 },
      },
      {
        field: 'type',
        component: 'Input',
        label: L('DisplayName:Type'),
        colProps: { span: 8 },
      },
      {
        field: 'clientId',
        component: 'ApiSelect',
        label: L('DisplayName:ClientId'),
        colProps: { span: 12 },
        componentProps: {
          api: getApplications,
          params: {
            maxResultCount: 100,
          },
          resultField: 'items',
          labelField: 'clientId',
          valueField: 'id',
        },
      },
      {
        field: 'creationTime',
        component: 'RangePicker',
        label: L('DisplayName:CreationTime'),
        colProps: { span: 12 },
        componentProps: {
          separator: '-',
          format: 'YYYY-MM-DD',
          style: { width: '100%', },
        },
      },
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
      field: 'applicationId',
      component: 'Input',
      label: L('DisplayName:ApplicationId'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'subject',
      component: 'Input',
      label: L('DisplayName:Subject'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
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
      field: 'creationDate',
      component: 'Input',
      label: L('DisplayName:CreationDate'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'expirationDate',
      component: 'Input',
      label: L('DisplayName:ExpirationDate'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'redemptionDate',
      component: 'Input',
      label: L('DisplayName:RedemptionDate'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'referenceId',
      component: 'Input',
      label: L('DisplayName:ReferenceId'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'status',
      component: 'Input',
      label: L('DisplayName:Status'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
      },
    },
    {
      field: 'payload',
      component: 'InputTextArea',
      label: L('DisplayName:Payload'),
      colProps: { span: 24 },
      componentProps: {
        readonly: true,
        autoSize: {
          minRows: 5,
        },
      },
    },
  ];
}
