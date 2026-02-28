<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { WebhookSendRecordDto } from '../../types/sendAttempts';

import { defineAsyncComponent, h, ref } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useHttpStatusCodeMap } from '@abp/request';
import { useTenantsApi } from '@abp/saas';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal, Tag } from 'ant-design-vue';
import debounce from 'lodash.debounce';

import { useSendAttemptsApi } from '../../api/useSendAttemptsApi';
import { WebhooksSendAttemptsPermissions } from '../../constants/permissions';

defineOptions({
  name: 'WebhookSendAttemptTable',
});

const MenuItem = Menu.Item;
const SendMessageIcon = createIconifyIcon('ant-design:send-outlined');

const { hasAccessByCodes } = useAccess();
const { getPagedListApi: getTenantsApi } = useTenantsApi();
const {
  bulkDeleteApi,
  bulkReSendApi,
  cancel,
  deleteApi,
  getPagedListApi,
  reSendApi,
} = useSendAttemptsApi();
const { getHttpStatusColor, httpStatusCodeMap } = useHttpStatusCodeMap();

const tenantFilter = ref<string>();
const selectedKeys = ref<string[]>([]);

const httpStatusOptions = Object.keys(httpStatusCodeMap).map((key) => {
  return {
    label: httpStatusCodeMap[Number(key)],
    value: key,
  };
});

const formOptions: VbenFormProps = {
  collapsed: true,
  collapsedRows: 3,
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
      component: 'RangePicker',
      fieldName: 'creationTime',
      label: $t('WebhooksManagement.DisplayName:CreationTime'),
    },
    {
      component: 'Select',
      componentProps: {
        allowClear: true,
        options: [
          {
            label: $t('WebhooksManagement.ResponseState:Successed'),
            value: 'true',
          },
          {
            label: $t('WebhooksManagement.ResponseState:Failed'),
            value: 'false',
          },
        ],
      },
      fieldName: 'state',
      label: $t('WebhooksManagement.DisplayName:State'),
    },
    {
      component: 'Select',
      componentProps: {
        options: httpStatusOptions,
      },
      fieldName: 'responseStatusCode',
      label: $t('WebhooksManagement.DisplayName:ResponseStatusCode'),
    },
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
      },
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<WebhookSendRecordDto> = {
  columns: [
    {
      align: 'center',
      type: 'checkbox',
      width: 40,
    },
    {
      align: 'left',
      field: 'tenantId',
      minWidth: 150,
      sortable: true,
      title: $t('WebhooksManagement.DisplayName:TenantId'),
    },
    {
      align: 'left',
      field: 'responseStatusCode',
      minWidth: 150,
      slots: { default: 'responseStatusCode' },
      sortable: true,
      title: $t('WebhooksManagement.DisplayName:ResponseStatusCode'),
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      sortable: true,
      title: $t('WebhooksManagement.DisplayName:CreationTime'),
    },
    {
      align: 'center',
      field: 'response',
      minWidth: 300,
      sortable: true,
      title: $t('WebhooksManagement.DisplayName:Response'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 220,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await getPagedListApi({
          sorting,
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
    refresh: {
      code: 'query',
    },
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<WebhookSendRecordDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [WebhookSendAttemptDrawer, drawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(
    () => import('./WebhookSendAttemptDrawer.vue'),
  ),
});

function onUpdate(row: WebhookSendRecordDto) {
  drawerApi.setData(row);
  drawerApi.open();
}

function onDelete(row: WebhookSendRecordDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [
      $t('WebhooksManagement.SelectedItems'),
    ]),
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
async function onMenuClick(row: WebhookSendRecordDto, info: MenuInfo) {
  switch (info.key) {
    case 'send': {
      await onSend(row);
      break;
    }
  }
}
async function onSend(row: WebhookSendRecordDto) {
  Modal.confirm({
    centered: true,
    content: $t('WebhooksManagement.ItemWillBeResendMessageWithFormat', [
      $t('WebhooksManagement.SelectedItems'),
    ]),
    onOk: async () => {
      try {
        gridApi.setLoading(true);
        await reSendApi(row.id);
        message.success($t('WebhooksManagement.SuccessfullySent'));
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
async function onSendMany(keys: string[]) {
  Modal.confirm({
    centered: true,
    content: `${$t('WebhooksManagement.ItemWillBeResendMessageWithFormat', [$t('WebhooksManagement.SelectedItems')])}`,
    onOk: async () => {
      try {
        gridApi.setLoading(true);
        await bulkReSendApi({ recordIds: keys });
        message.success($t('WebhooksManagement.SuccessfullySent'));
        // 重新查询
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
async function onDeleteMany(keys: string[]) {
  Modal.confirm({
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [$t('WebhooksManagement.SelectedItems')])}`,
    onOk: async () => {
      try {
        gridApi.setLoading(true);
        await bulkDeleteApi({ recordIds: keys });
        message.success($t('AbpUi.DeletedSuccessfully'));
        // 清理选中的key
        selectedKeys.value = selectedKeys.value.filter(
          (key) => !keys.includes(key),
        );
        // 重新查询
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Grid :table-title="$t('WebhooksManagement.SendAttempts')">
    <template #toolbar-tools>
      <div v-if="selectedKeys.length > 0" class="flex flex-row gap-2">
        <Button
          v-if="hasAccessByCodes([WebhooksSendAttemptsPermissions.Resend])"
          :icon="h(SendMessageIcon)"
          type="primary"
          @click="onSendMany(selectedKeys)"
        >
          {{ $t('WebhooksManagement.Resend') }}
        </Button>
        <Button
          v-if="hasAccessByCodes([WebhooksSendAttemptsPermissions.Delete])"
          :icon="h(DeleteOutlined)"
          type="primary"
          danger
          @click="onDeleteMany(selectedKeys)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
    <template #responseStatusCode="{ row }">
      <Tag :color="getHttpStatusColor(row.responseStatusCode)">
        {{ httpStatusCodeMap[row.responseStatusCode] }}
      </Tag>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="hasAccessByCodes([WebhooksSendAttemptsPermissions.Default])"
          :icon="h(EditOutlined)"
          type="link"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="hasAccessByCodes([WebhooksSendAttemptsPermissions.Delete])"
          :icon="h(DeleteOutlined)"
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem
                v-if="
                  hasAccessByCodes([WebhooksSendAttemptsPermissions.Resend])
                "
                key="send"
              >
                <div class="flex flex-row items-center gap-[4px]">
                  <SendMessageIcon color="green" />
                  {{ $t('WebhooksManagement.Resend') }}
                </div>
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <WebhookSendAttemptDrawer />
</template>

<style lang="scss" scoped></style>
