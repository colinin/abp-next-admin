import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';

const { L } = useLocalization('AbpAuditLogging');

const httpStatusCodeOptions = [
  { value: 100, label: '100 - Continue' },
  { value: 101, label: '101 - Switching Protocols' },
  { value: 200, label: '200 - OK' },
  { value: 201, label: '201 - Created' },
  { value: 202, label: '202 - Accepted' },
  { value: 203, label: '203 - Non Authoritative Information' },
  { value: 204, label: '204 - No Content' },
  { value: 205, label: '205 - Reset Content' },
  { value: 206, label: '206 - Partial Content' },
  { value: 300, label: '300 - Multiple Choices' },
  { value: 301, label: '301 - Moved Permanently' },
  { value: 302, label: '302 - Found & Redirect' },
  { value: 303, label: '303 - See Other' },
  { value: 304, label: '304 - Not Modified' },
  { value: 305, label: '305 - Use Proxy' },
  { value: 306, label: '306 - Switch Proxy' },
  { value: 307, label: '307 - Temporary Redirect' },
  { value: 308, label: '308 - Permanent Redirect' },
  { value: 400, label: '400 - Bad Request' },
  { value: 401, label: '401 - Unauthorized' },
  { value: 402, label: '402 - Payment Required' },
  { value: 403, label: '403 - Forbidden' },
  { value: 404, label: '404 - Not Found' },
  { value: 405, label: '405 - Method Not Allowed' },
  { value: 406, label: '406 - Not Acceptable' },
  { value: 407, label: '407 - Proxy Authentication Required' },
  { value: 408, label: '408 - Request Timeout' },
  { value: 409, label: '409 - Conflict' },
  { value: 410, label: '410 - Gone' },
  { value: 411, label: '411 - Length Required' },
  { value: 412, label: '412 - Precondition Failed' },
  { value: 413, label: '413 - Request Entity Too Large' },
  { value: 414, label: '414 - Request Uri Too Long' },
  { value: 415, label: '415 - Unsupported Media Type' },
  { value: 416, label: '416 - Requested Range Not Satisfiable' },
  { value: 417, label: '417 - Expectation Failed' },
  { value: 426, label: '426 - Upgrade Required' },
  { value: 500, label: '500 - Internal Server Error' },
  { value: 501, label: '501 - Not mplemented' },
  { value: 502, label: '502 - Bad Gateway' },
  { value: 503, label: '503 - Service Unavailable' },
  { value: 504, label: '504 - Gateway Timeout' },
  { value: 505, label: '505 - Http Version Not Supported' },
];

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
        field: 'httpMethod',
        component: 'Input',
        label: L('HttpMethod'),
        colProps: { span: 6 },
      },
      {
        field: 'httpStatusCode',
        component: 'Select',
        label: L('HttpStatusCode'),
        colProps: { span: 6 },
        componentProps: {
          options: httpStatusCodeOptions,
        },
      },
      {
        field: 'url',
        component: 'Input',
        label: L('RequestUrl'),
        colProps: { span: 6 },
      },
      {
        field: 'minExecutionDuration',
        component: 'InputNumber',
        label: L('MinExecutionDuration'),
        labelWidth: 180,
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'maxExecutionDuration',
        component: 'InputNumber',
        label: L('MaxExecutionDuration'),
        labelWidth: 180,
        colProps: { span: 9 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
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
      {
        field: 'hasException',
        component: 'Checkbox',
        label: L('HasException'),
        colProps: { span: 6 },
        renderComponentContent: L('HasException'),
      },
    ],
  };
}
