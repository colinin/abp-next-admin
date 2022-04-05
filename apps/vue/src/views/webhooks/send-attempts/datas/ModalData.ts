import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { getList as getTenants } from '/@/api/saas/tenant';
import { getList as getSubscriptions } from '/@/api/webhooks/subscriptions';
import { httpStatusOptions } from '../../typing';

const { L } = useLocalization('WebhooksManagement', 'AbpUi');

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
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
        field: 'responseStatusCode',
        component: 'Select',
        label: L('DisplayName:ResponseStatusCode'),
        colProps: { span: 6 },
        componentProps: {
          options: httpStatusOptions,
        },
      },
      {
        field: 'beginCreationTime',
        component: 'DatePicker',
        label: L('DisplayName:BeginCreationTime'),
        colProps: { span: 6 },
        componentProps: {
          style: {
            width: '100%',
          },
        },
      },
      {
        field: 'endCreationTime',
        component: 'DatePicker',
        label: L('DisplayName:EndCreationTime'),
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
