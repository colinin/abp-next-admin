<script setup lang="ts">
import type { SortOrder } from '@abp/core';
import type { VbenFormProps, VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { AuditLogDto } from '../../types/audit-logs';

import { defineAsyncComponent, h } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
import { Button, message, Modal, Tag } from 'ant-design-vue';

import { deleteApi, getPagedListApi } from '../../api/audit-logs';
import { AuditLogPermissions } from '../../constants/permissions';
import { useAuditlogs } from '../../hooks/useAuditlogs';
import { httpMethodOptions, httpStatusCodeOptions } from './mapping';

defineOptions({
  name: 'AuditLogTable',
});

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: true,
  collapsedRows: 2,
  fieldMappingTime: [
    [
      'executionTime',
      ['startTime', 'endTime'],
      ['YYYY-MM-DD HH:mm:ss', 'YYYY-MM-DD HH:mm:ss'],
    ],
  ],
  schema: [
    {
      component: 'RangePicker',
      fieldName: 'executionTime',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpAuditLogging.ExecutionTime'),
    },
    {
      component: 'Input',
      fieldName: 'url',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpAuditLogging.RequestUrl'),
    },
    {
      component: 'Select',
      componentProps: {
        options: httpStatusCodeOptions,
      },
      fieldName: 'httpStatusCode',
      label: $t('AbpAuditLogging.HttpStatusCode'),
    },
    {
      component: 'Select',
      componentProps: {
        options: httpMethodOptions,
      },
      fieldName: 'httpMethod',
      label: $t('AbpAuditLogging.HttpMethod'),
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
      fieldName: 'clientIpAddress',
      label: $t('AbpAuditLogging.ClientIpAddress'),
    },
    {
      component: 'InputNumber',
      fieldName: 'minExecutionDuration',
      label: $t('AbpAuditLogging.MinExecutionDuration'),
      labelWidth: 150,
    },
    {
      component: 'InputNumber',
      fieldName: 'maxExecutionDuration',
      label: $t('AbpAuditLogging.MaxExecutionDuration'),
      labelWidth: 150,
    },
    {
      component: 'Input',
      fieldName: 'correlationId',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpAuditLogging.CorrelationId'),
    },
    {
      component: 'Checkbox',
      componentProps: {
        render: () => {
          return h('span', $t('AbpAuditLogging.HasException'));
        },
      },
      fieldName: 'hasException',
      label: $t('AbpAuditLogging.HasException'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
  wrapperClass: 'grid-cols-4',
};

const gridOptions: VxeGridProps<AuditLogDto> = {
  columns: [
    {
      align: 'left',
      field: 'url',
      slots: { default: 'url' },
      sortable: true,
      title: $t('AbpAuditLogging.RequestUrl'),
      width: 500,
    },
    {
      align: 'left',
      field: 'userName',
      sortable: true,
      title: $t('AbpAuditLogging.UserName'),
      width: 120,
    },
    {
      align: 'left',
      field: 'executionTime',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      sortable: true,
      title: $t('AbpAuditLogging.ExecutionTime'),
      width: 150,
    },
    {
      align: 'left',
      field: 'executionDuration',
      sortable: true,
      title: $t('AbpAuditLogging.ExecutionDuration'),
      width: 140,
    },
    {
      align: 'left',
      field: 'clientId',
      sortable: true,
      title: $t('AbpAuditLogging.ClientId'),
      width: 150,
    },
    {
      align: 'left',
      field: 'clientIpAddress',
      sortable: true,
      title: $t('AbpAuditLogging.ClientIpAddress'),
      width: 150,
    },
    {
      align: 'left',
      field: 'applicationName',
      sortable: true,
      title: $t('AbpAuditLogging.ApplicationName'),
      width: 160,
    },
    {
      align: 'left',
      field: 'correlationId',
      sortable: true,
      title: $t('AbpAuditLogging.CorrelationId'),
      width: 160,
    },
    {
      align: 'left',
      field: 'tenantName',
      sortable: true,
      title: $t('AbpAuditLogging.TenantName'),
      width: 100,
    },
    {
      align: 'left',
      field: 'browserInfo',
      sortable: true,
      title: $t('AbpAuditLogging.BrowserInfo'),
      width: 300,
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
  sortConfig: {
    remote: true,
  },
  toolbarConfig: {
    custom: true,
    export: true,
    // import: true,
    refresh: true,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<AuditLogDto> = {
  sortChange: onSort,
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const { getHttpMethodColor, getHttpStatusCodeColor } = useAuditlogs();
const [AuditLogDrawer, logDrawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(
    () => import('./AuditLogDrawer.vue'),
  ),
});

function onUpdate(row: AuditLogDto) {
  logDrawerApi.setData(row);
  logDrawerApi.open();
}

async function onDelete(row: AuditLogDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessage'),
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.SuccessfullyDeleted'));
      gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onSort(params: { field: string; order: SortOrder }) {
  const sorting = params.order ? `${params.field} ${params.order}` : undefined;
  gridApi.query({ sorting });
}

function onFilter(field: string, value: any) {
  gridApi.formApi.setFieldValue(field, value);
  gridApi.formApi.validateAndSubmitForm();
}
</script>

<template>
  <Grid :table-title="$t('AbpAuditLogging.AuditLog')">
    <template #url="{ row }">
      <Tag
        :color="getHttpStatusCodeColor(row.httpStatusCode)"
        class="cursor-pointer"
        @click="onFilter('httpStatusCode', row.httpStatusCode)"
      >
        {{ row.httpStatusCode }}
      </Tag>
      <Tag
        :color="getHttpMethodColor(row.httpMethod)"
        class="ml-px cursor-pointer"
        @click="onFilter('httpMethod', row.httpMethod)"
      >
        {{ row.httpMethod }}
      </Tag>
      <a
        class="link"
        href="javaScript:void(0);"
        @click="onFilter('url', row.url)"
        >{{ row.url }}
      </a>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[AuditLogPermissions.Default]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpAuditLogging.ShowLogDialog') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[AuditLogPermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <AuditLogDrawer />
</template>

<style lang="scss" scoped></style>
