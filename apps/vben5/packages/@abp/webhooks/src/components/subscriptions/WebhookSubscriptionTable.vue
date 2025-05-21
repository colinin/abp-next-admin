<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { WebhookSubscriptionDto } from '../../types/subscriptions';

import { defineAsyncComponent, h, ref } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useTenantsApi } from '@abp/saas';
import { useVbenVxeGrid } from '@abp/ui';
import {
  CheckOutlined,
  CloseOutlined,
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal, Tag } from 'ant-design-vue';
import debounce from 'lodash.debounce';

import { useSubscriptionsApi } from '../../api/useSubscriptionsApi';
import { WebhookSubscriptionPermissions } from '../../constants/permissions';

defineOptions({
  name: 'WebhookSubscriptionTable',
});

const { hasAccessByCodes } = useAccess();
const { getPagedListApi: getTenantsApi } = useTenantsApi();
const { cancel, deleteApi, getAllAvailableWebhooksApi, getPagedListApi } =
  useSubscriptionsApi();

const tenantFilter = ref<string>();
const selectedKeys = ref<string[]>([]);

const formOptions: VbenFormProps = {
  collapsed: true,
  collapsedRows: 2,
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  fieldMappingTime: [
    [
      'creationTime',
      ['beginCreationTime', 'endCreationTime'],
      ['YYYY-MM-DD 00:00:00', 'YYYY-MM-DD 23:59:59'],
    ],
  ],
  schema: [
    {
      component: 'ApiSelect',
      componentProps: () => {
        return {
          allowClear: true,
          api: getTenantsApi,
          filterOption: false,
          labelField: 'normalizedName',
          onSearch: debounce((filter?: string) => {
            tenantFilter.value = filter;
          }, 500),
          params: {
            filter: tenantFilter.value || undefined,
          },
          resultField: 'items',
          showSearch: true,
          valueField: 'id',
        };
      },
      fieldName: 'tenantId',
      label: $t('WebhooksManagement.DisplayName:TenantId'),
    },
    {
      component: 'ApiSelect',
      componentProps: {
        allowClear: true,
        api: onInitWebhooks,
        filterOption: (onputValue: string, option: any) => {
          return option.label.includes(onputValue);
        },
        showSearch: true,
      },
      fieldName: 'webhooks',
      label: $t('WebhooksManagement.DisplayName:Webhooks'),
    },
    {
      component: 'RangePicker',
      fieldName: 'creationTime',
      label: $t('WebhooksManagement.DisplayName:CreationTime'),
    },
    {
      component: 'Input',
      fieldName: 'webhookUri',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('WebhooksManagement.DisplayName:WebhookUri'),
    },
    {
      component: 'Checkbox',
      fieldName: 'isActive',
      label: $t('WebhooksManagement.DisplayName:IsActive'),
    },
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
      },
      fieldName: 'filter',
      formItemClass: 'col-span-3 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<WebhookSubscriptionDto> = {
  columns: [
    {
      align: 'center',
      fixed: 'left',
      type: 'checkbox',
      width: 40,
    },
    {
      align: 'left',
      field: 'isActive',
      fixed: 'left',
      minWidth: 150,
      slots: { default: 'isActive' },
      title: $t('WebhooksManagement.DisplayName:IsActive'),
    },
    {
      align: 'left',
      field: 'tenantId',
      fixed: 'left',
      minWidth: 150,
      title: $t('WebhooksManagement.DisplayName:TenantId'),
    },
    {
      align: 'left',
      field: 'webhookUri',
      fixed: 'left',
      minWidth: 300,
      title: $t('WebhooksManagement.DisplayName:WebhookUri'),
    },
    {
      align: 'left',
      field: 'description',
      minWidth: 150,
      title: $t('WebhooksManagement.DisplayName:Description'),
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      title: $t('WebhooksManagement.DisplayName:CreationTime'),
    },
    {
      align: 'center',
      field: 'webhooks',
      minWidth: 300,
      slots: { default: 'webhooks' },
      title: $t('WebhooksManagement.DisplayName:Webhooks'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 200,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        return await getPagedListApi({
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    custom: true,
    export: true,
    // import: true,
    refresh: true,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<WebhookSubscriptionDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [WebhookSubscriptionModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./WebhookSubscriptionModal.vue'),
  ),
});

async function onInitWebhooks() {
  if (hasAccessByCodes([WebhookSubscriptionPermissions.Default])) {
    const { items } = await getAllAvailableWebhooksApi();
    return items.map((group) => {
      return {
        label: group.displayName,
        options: group.webhooks.map((p) => {
          return {
            label: p.displayName,
            value: p.name,
          };
        }),
        value: group.name,
      };
    });
  }
  return [];
}

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

function onUpdate(row: WebhookSubscriptionDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: WebhookSubscriptionDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.title]),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Grid :table-title="$t('WebhooksManagement.Subscriptions')">
    <template #toolbar-tools>
      <Button :icon="h(PlusOutlined)" type="primary" @click="onCreate">
        {{ $t('WebhooksManagement.Subscriptions:AddNew') }}
      </Button>
    </template>
    <template #isActive="{ row }">
      <div class="flex flex-row justify-center">
        <CheckOutlined v-if="row.isActive" class="text-green-500" />
        <CloseOutlined v-else class="text-red-500" />
      </div>
    </template>
    <template #webhooks="{ row }">
      <div class="flex flex-row justify-center gap-1">
        <template v-for="webhook in row.webhooks" :key="webhook">
          <Tag color="blue">{{ webhook }}</Tag>
        </template>
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button :icon="h(EditOutlined)" type="link" @click="onUpdate(row)">
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <WebhookSubscriptionModal @change="() => gridApi.query()" />
</template>

<style lang="scss" scoped></style>
