import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { LogLevel } from '/@/api/logging/model/loggingModel';

const { L } = useLocalization('AbpAuditLogging');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    schemas: [
      {
        field: 'startTime',
        component: 'DatePicker',
        label: L('StartTime'),
        colProps: { span: 9 },
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
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'level',
        component: 'Select',
        label: L('Level'),
        colProps: { span: 6 },
        componentProps: {
          style: {
            width: '100%',
          },
          options: [
            { label: 'None', value: LogLevel.None },
            { label: 'Debug', value: LogLevel.Debug },
            { label: 'Information', value: LogLevel.Information },
            { label: 'Warning', value: LogLevel.Warning },
            { label: 'Error', value: LogLevel.Error },
            { label: 'Trace', value: LogLevel.Trace },
            { label: 'Critical', value: LogLevel.Critical },
          ],
        },
      },
      {
        field: 'hasException',
        component: 'Select',
        label: L('HasException'),
        colProps: { span: 6 },
        componentProps: {
          style: {
            width: '100%',
          },
          options: [
            { label: 'None', value: null },
            { label: 'True', value: true },
            { label: 'False', value: false },
          ],
        },
      },
      {
        field: 'machineName',
        component: 'Input',
        label: L('MachineName'),
        colProps: { span: 6 },
      },
      {
        field: 'environment',
        component: 'Input',
        label: L('Environment'),
        colProps: { span: 6 },
      },
      {
        field: 'application',
        component: 'Input',
        label: L('Application'),
        colProps: { span: 6 },
      },
      {
        field: 'requestId',
        component: 'Input',
        label: L('RequestId'),
        colProps: { span: 8 },
      },
      {
        field: 'requestPath',
        component: 'Input',
        label: L('RequestPath'),
        colProps: { span: 8 },
      },
      {
        field: 'correlationId',
        component: 'Input',
        label: L('CorrelationId'),
        colProps: { span: 8 },
      },
    ],
  };
}
