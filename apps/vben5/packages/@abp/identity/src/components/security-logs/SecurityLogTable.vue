<script setup lang="ts">
import type { SortOrder } from '@abp/core';
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { SecurityLogDto } from '../../types/security-logs';

import { defineAsyncComponent, h, ref, toValue } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
import { Button, message, Modal, Tag } from 'ant-design-vue';

import { useSecurityLogsApi } from '../../api/useSecurityLogsApi';
import { SecurityLogPermissions } from '../../constants/permissions';

defineOptions({
  name: 'SecurityLogTable',
});

const { cancel, deleteApi, deleteManyApi, getPagedListApi } =
  useSecurityLogsApi();

const selectedKeys = ref<string[]>([]);
const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  fieldMappingTime: [['creationTime', ['startTime', 'endTime'], 'YYYY-MM-DD']],
  schema: [
    {
      component: 'RangePicker',
      fieldName: 'creationTime',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpAuditLogging.CreationTime'),
    },
    {
      component: 'Input',
      fieldName: 'applicationName',
      label: $t('AbpAuditLogging.ApplicationName'),
    },
    {
      component: 'Input',
      fieldName: 'userName',
      label: $t('AbpAuditLogging.UserName'),
    },
    {
      component: 'Input',
      fieldName: 'clientId',
      label: $t('AbpAuditLogging.ClientId'),
    },
    {
      component: 'Input',
      fieldName: 'identity',
      label: $t('AbpAuditLogging.Identity'),
    },
    {
      component: 'Input',
      fieldName: 'action',
      label: $t('AbpAuditLogging.ActionName'),
    },
    {
      component: 'Input',
      fieldName: 'correlationId',
      label: $t('AbpAuditLogging.CorrelationId'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
  wrapperClass: 'grid-cols-4',
};

const gridOptions: VxeGridProps<SecurityLogDto> = {
  columns: [
    {
      align: 'center',
      type: 'checkbox',
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      sortable: true,
      title: $t('AbpAuditLogging.CreationTime'),
      width: 180,
    },
    {
      align: 'left',
      field: 'identity',
      sortable: true,
      title: $t('AbpAuditLogging.Identity'),
      width: 180,
    },
    {
      align: 'left',
      field: 'userName',
      sortable: true,
      title: $t('AbpAuditLogging.UserName'),
      width: 150,
    },
    {
      align: 'left',
      field: 'clientId',
      sortable: true,
      title: $t('AbpAuditLogging.ClientId'),
      width: 200,
    },
    {
      align: 'left',
      field: 'clientIpAddress',
      slots: { default: 'clientIpAddress' },
      sortable: true,
      title: $t('AbpAuditLogging.ClientIpAddress'),
      width: 200,
    },
    {
      align: 'left',
      field: 'applicationName',
      sortable: true,
      title: $t('AbpAuditLogging.ApplicationName'),
      width: 200,
    },
    {
      align: 'left',
      field: 'tenantName',
      sortable: true,
      title: $t('AbpAuditLogging.TenantName'),
      width: 180,
    },
    {
      align: 'left',
      field: 'action',
      sortable: true,
      title: $t('AbpAuditLogging.Actions'),
      width: 180,
    },
    {
      align: 'left',
      field: 'correlationId',
      sortable: true,
      title: $t('AbpAuditLogging.CorrelationId'),
      width: 200,
    },
    {
      align: 'left',
      field: 'browserInfo',
      sortable: true,
      title: $t('AbpAuditLogging.BrowserInfo'),
      width: 500,
    },
    {
      field: 'actions',
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

const gridEvents: VxeGridListeners<SecurityLogDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((x) => x.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((x) => x.id);
  },
  sortChange: onSort,
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});
const [SecurityLogDrawer, drawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(
    () => import('./SecurityLogDrawer.vue'),
  ),
});

function onUpdate(row: SecurityLogDto) {
  drawerApi.setData(row);
  drawerApi.open();
}

function onDelete(row: SecurityLogDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessage'),
    onCancel: () => {
      cancel('User closed cancel delete modal.');
    },
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onBulkDelete() {
  Modal.confirm({
    centered: true,
    content: $t('component.table.selectedItemWellBeDeleted'),
    onOk: async () => {
      await deleteManyApi({
        ids: toValue(selectedKeys),
      });
      selectedKeys.value = [];
      message.success($t('AbpUi.DeletedSuccessfully'));
      gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onSort(params: { field: string; order: SortOrder }) {
  const sorting = params.order ? `${params.field} ${params.order}` : undefined;
  gridApi.query({ sorting });
}
</script>

<template>
  <Grid :table-title="$t('AbpAuditLogging.SecurityLog')">
    <template #toolbar-tools>
      <Button
        v-if="selectedKeys.length > 0"
        :icon="h(DeleteOutlined)"
        danger
        ghost
        type="primary"
        v-access:code="[SecurityLogPermissions.Delete]"
        @click="onBulkDelete"
      >
        {{ $t('AbpUi.Delete') }}
      </Button>
    </template>
    <template #clientIpAddress="{ row }">
      <Tag v-if="row.extraProperties?.Location" color="blue">
        {{ row.extraProperties?.Location }}
      </Tag>
      <span>{{ row.clientIpAddress }}</span>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[SecurityLogPermissions.Default]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[SecurityLogPermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <SecurityLogDrawer />
</template>
