import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';

const { L } = useLocalization('AbpAuditLogging');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'applicationName',
        component: 'Input',
        label: L('ApplicationName'),
        colProps: { span: 6 },
      },
      {
        field: 'userName',
        component: 'Input',
        label: L('UserName'),
        colProps: { span: 6 },
      },
      {
        field: 'clientId',
        component: 'Input',
        label: L('ClientId'),
        colProps: { span: 6 },
      },
      {
        field: 'identity',
        component: 'Input',
        label: L('Identity'),
        colProps: { span: 6 },
      },
      {
        field: 'actionName',
        component: 'Input',
        label: L('ActionName'),
        colProps: { span: 6 },
      },
      {
        field: 'correlationId',
        component: 'Input',
        label: L('CorrelationId'),
        colProps: { span: 6 },
      },
      {
        field: 'startTime',
        component: 'DatePicker',
        label: L('StartTime'),
        colProps: { span: 6 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'endTime',
        component: 'DatePicker',
        label: L('EndTime'),
        colProps: { span: 6 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
    ],
  };
}
