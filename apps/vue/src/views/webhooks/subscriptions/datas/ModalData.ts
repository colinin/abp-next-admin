import { useLocalization } from '/@/hooks/abp/useLocalization';
import { FormProps } from '/@/components/Form';
import { getList } from '/@/api/saas/tenant';
import { getAllAvailableWebhooks } from '/@/api/webhooks/subscriptions';

const { L } = useLocalization(['WebhooksManagement', 'AbpUi']);

function getAllAvailables(): Promise<any> {
  return getAllAvailableWebhooks().then((res) => {
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
    schemas: [
      {
        field: 'tenantId',
        component: 'ApiSelect',
        label: L('DisplayName:TenantId'),
        colProps: { span: 6 },
        componentProps: {
          api: getList,
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
        colProps: { span: 12 },
      },
      {
        field: 'secret',
        component: 'Input',
        label: L('DisplayName:Secret'),
        colProps: { span: 6 },
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
        field: 'webhooks',
        component: 'ApiSelect',
        label: L('DisplayName:Webhooks'),
        colProps: { span: 6 },
        componentProps: {
          api: () => getAllAvailables(),
          showSearch: true,
          filterOption: (onputValue: string, option: any) => {
            return option.label.includes(onputValue);
          },
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
        colProps: { span: 24 },
      },
    ],
  };
}
