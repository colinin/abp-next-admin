import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { GetListAsyncByInput } from '/@/api/saas/tenant';
import { GetAllAvailableWebhooksAsync } from '/@/api/webhooks/subscriptions';

const { L } = useLocalization(['WebhooksManagement', 'AbpUi']);

function getAllAvailables(): Promise<any> {
  return GetAllAvailableWebhooksAsync().then((res) => {
    return res.items.map((group) => {
      return {
        label: group.displayName,
        value: group.name,
        options: group.webhooks.map((p) => {
          return {
            label: p.displayName,
            value: p.name,
          }
        }),
      }
    })
  });
}

export function getSearchFormSchemas(): Partial<FormProps> {
  return {
    labelWidth: 100,
    alwaysShowLines: 2,
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
          api: GetListAsyncByInput,
          params: {
            skipCount: 0,
            maxResultCount: 1000,
          },
          resultField: 'items',
          labelField: 'name',
          valueField: 'id',
        },
      },
      {
        field: 'webhookUri',
        component: 'Input',
        label: L('DisplayName:WebhookUri'),
        colProps: { span: 18 },
      },
      {
        field: 'secret',
        component: 'Input',
        label: L('DisplayName:Secret'),
        colProps: { span: 6 },
      },
      {
        field: 'webhooks',
        component: 'ApiSelect',
        label: L('DisplayName:Webhooks'),
        colProps: { span: 12 },
        componentProps: {
          api: getAllAvailables,
          showSearch: true,
          filterOption: (onputValue: string, option: any) => {
            return option.label.includes(onputValue);
          },
        },
      },
      {
        field: 'isActive',
        component: 'Checkbox',
        label: L('DisplayName:IsActive'),
        colProps: { span: 6 },
        defaultValue: true,
        renderComponentContent: L('DisplayName:IsActive'),
      },
      {
        field: 'creationTime',
        component: 'RangePicker',
        label: L('DisplayName:CreationTime'),
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
        colProps: { span: 18 },
      },
    ],
  };
}
