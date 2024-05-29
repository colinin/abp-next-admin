import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { LogLevel } from '/@/api/logging/logs/model';

const { L } = useLocalization('AbpAuditLogging');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    alwaysShowLines: 3,
    fieldMapToTime: [
      ['timeStamp', ['startTime', 'endTime'], ['YYYY-MM-DD HH:mm:ss', 'YYYY-MM-DD HH:mm:ss']],
    ],
    schemas: [
      {
        field: 'machineName',
        component: 'Input',
        label: L('MachineName'),
        colProps: { span: 6 },
      },
      {
        field: 'application',
        component: 'Input',
        label: L('Application'),
        colProps: { span: 6 },
      },
      {
        field: 'environment',
        component: 'Input',
        label: L('Environment'),
        colProps: { span: 6 },
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
        field: 'timeStamp',
        component: 'RangePicker',
        label: L('TimeStamp'),
        colProps: { span: 8 },
        componentProps: {
          showTime: true,
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'requestId',
        component: 'Input',
        label: L('RequestId'),
        colProps: { span: 6 },
      },
      {
        field: 'requestPath',
        component: 'Input',
        label: L('RequestPath'),
        colProps: { span: 10 },
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
        field: 'connectionId',
        component: 'Input',
        label: L('ConnectionId'),
        colProps: { span: 6 },
      },
      {
        field: 'correlationId',
        component: 'Input',
        label: L('CorrelationId'),
        colProps: { span: 12 },
      },
      {
        field: 'processId',
        component: 'InputNumber',
        label: L('ProcessId'),
        colProps: { span: 6 },
      },
      {
        field: 'threadId',
        component: 'InputNumber',
        label: L('ThreadId'),
        colProps: { span: 6 },
      },
      {
        field: 'context',
        component: 'Input',
        label: L('Context'),
        colProps: { span: 12 },
      },
    ],
  };
}
