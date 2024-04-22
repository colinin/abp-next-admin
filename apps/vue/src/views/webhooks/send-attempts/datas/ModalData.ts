import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { GetListAsyncByInput as getTenants } from '/@/api/saas/tenant';
import { GetListAsyncByInput as getSubscriptions } from '/@/api/webhooks/subscriptions';
import { httpStatusOptions } from '../../typing';

const { L } = useLocalization(['WebhooksManagement', 'AbpUi']);

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    fieldMapToTime: [
      ['creationTime', ['beginCreationTime', 'endCreationTime'],  ['YYYY-MM-DDT00:00:00', 'YYYY-MM-DDT23:59:59']]
    ],
    schemas: [
      {
        field: 'tenantId',
        component: 'ApiSelect',
        label: L('DisplayName:TenantId'),
        colProps: { span: 6 },
        componentProps: {
          api: getTenants,
          params: {
            skipCount: 0,
            maxResultCount: 100,
          },
          resultField: 'items',
          labelField: 'name',
          valueField: 'id',
        },
      },
      {
        field: 'subscriptionId',
        component: 'ApiSelect',
        label: L('DisplayName:Subscription'),
        colProps: { span: 12 },
        componentProps: {
          api: getSubscriptions,
          params: {
            skipCount: 0,
            maxResultCount: 100,
          },
          resultField: 'items',
          labelField: 'webhookUri',
          valueField: 'id',
        }
      },
      {
        field: 'state',
        component: 'Select',
        label: L('DisplayName:State'),
        colProps: { span: 6 },
        componentProps: {
          options: [
            { label: L('ResponseState:Successed'), value: true, },
            { label: L('ResponseState:Failed'), value: false, },
          ],
        },
      },
      {
        field: 'responseStatusCode',
        component: 'Select',
        label: L('DisplayName:ResponseStatusCode'),
        colProps: { span: 6 },
        componentProps: {
          options: httpStatusOptions,
        },
      },
      {
        field: 'creationTime',
        component: 'RangePicker',
        label: L('DisplayName:BeginCreationTime'),
        colProps: { span: 6 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'filter',
        component: 'Input',
        label: L('Search'),
        colProps: { span: 12 },
      },
    ],
  };
}
