<script setup lang="ts">
import type { SortOrder } from '@abp/core';
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { LogDto } from '../../types/loggings';

import { defineAsyncComponent, h, reactive, ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { EditOutlined } from '@ant-design/icons-vue';
import { Button, Tag } from 'ant-design-vue';

import { useLoggingsApi } from '../../api/useLoggingsApi';
import { SystemLogPermissions } from '../../constants/permissions';
import { LogLevel } from '../../types/loggings';

defineOptions({
  name: 'LoggingTable',
});
const { getPagedListApi } = useLoggingsApi();

const selectedKeys = ref<string[]>([]);
const logLevelOptions = reactive([
  {
    color: 'purple',
    label: 'Trace',
    value: LogLevel.Trace,
  },
  {
    color: 'blue',
    label: 'Debug',
    value: LogLevel.Debug,
  },
  {
    color: 'green',
    label: 'Information',
    value: LogLevel.Information,
  },
  {
    color: 'orange',
    label: 'Warning',
    value: LogLevel.Warning,
  },
  {
    color: 'red',
    label: 'Error',
    value: LogLevel.Error,
  },
  {
    color: '#f50',
    label: 'Critical',
    value: LogLevel.Critical,
  },
  {
    color: '',
    label: 'None',
    value: LogLevel.None,
  },
]);
const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: true,
  collapsedRows: 2,
  fieldMappingTime: [
    [
      'timeStamp',
      ['startTime', 'endTime'],
      ['YYYY-MM-DD HH:mm:ss', 'YYYY-MM-DD HH:mm:ss'],
    ],
  ],
  schema: [
    {
      component: 'Select',
      componentProps: {
        options: logLevelOptions,
      },
      fieldName: 'level',
      label: $t('AbpAuditLogging.Level'),
    },
    {
      component: 'RangePicker',
      componentProps: {
        showTime: true,
      },
      fieldName: 'timeStamp',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpAuditLogging.TimeStamp'),
    },
    {
      component: 'Input',
      fieldName: 'application',
      label: $t('AbpAuditLogging.ApplicationName'),
    },
    {
      component: 'Input',
      fieldName: 'machineName',
      label: $t('AbpAuditLogging.MachineName'),
    },
    {
      component: 'Input',
      fieldName: 'environment',
      label: $t('AbpAuditLogging.Environment'),
    },
    {
      component: 'Input',
      fieldName: 'requestId',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpAuditLogging.RequestId'),
    },
    {
      component: 'Input',
      fieldName: 'requestPath',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpAuditLogging.RequestPath'),
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

const gridOptions: VxeGridProps<LogDto> = {
  columns: [
    {
      align: 'left',
      field: 'applicationName',
      formatter: ({ row }) => {
        return row.fields?.application;
      },
      sortable: true,
      title: $t('AbpAuditLogging.ApplicationName'),
      width: 150,
    },
    {
      align: 'left',
      field: 'timeStamp',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      sortable: true,
      title: $t('AbpAuditLogging.TimeStamp'),
      width: 150,
    },
    {
      align: 'left',
      field: 'level',
      slots: { default: 'level' },
      sortable: true,
      title: $t('AbpAuditLogging.Level'),
      width: 120,
    },
    {
      align: 'left',
      field: 'message',
      sortable: true,
      title: $t('AbpAuditLogging.Message'),
      width: 500,
    },
    {
      align: 'left',
      field: 'machineName',
      formatter: ({ row }) => {
        return row.fields?.machineName;
      },
      sortable: true,
      title: $t('AbpAuditLogging.MachineName'),
      width: 140,
    },
    {
      align: 'left',
      field: 'environment',
      formatter: ({ row }) => {
        return row.fields?.environment;
      },
      sortable: true,
      title: $t('AbpAuditLogging.Environment'),
      width: 150,
    },
    {
      align: 'left',
      field: 'requestId',
      formatter: ({ row }) => {
        return row.fields?.requestId;
      },
      sortable: true,
      title: $t('AbpAuditLogging.RequestId'),
      width: 200,
    },
    {
      align: 'left',
      field: 'requestPath',
      formatter: ({ row }) => {
        return row.fields?.requestPath;
      },
      sortable: true,
      title: $t('AbpAuditLogging.RequestPath'),
      width: 300,
    },
    {
      align: 'left',
      field: 'connectionId',
      formatter: ({ row }) => {
        return row.fields?.connectionId;
      },
      sortable: true,
      title: $t('AbpAuditLogging.ConnectionId'),
      width: 150,
    },
    {
      align: 'left',
      field: 'correlationId',
      formatter: ({ row }) => {
        return row.fields?.correlationId;
      },
      sortable: true,
      title: $t('AbpAuditLogging.CorrelationId'),
      width: 240,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 180,
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

const gridEvents: VxeGridListeners<LogDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((x) => x.fields.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((x) => x.fields.id);
  },
  sortChange: onSort,
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});
const [LoggingDrawer, logDrawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(() => import('./LoggingDrawer.vue')),
});

function onUpdate(row: LogDto) {
  logDrawerApi.setData(row);
  logDrawerApi.open();
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
  <Grid :table-title="$t('AbpAuditLogging.Logging')">
    <template #level="{ row }">
      <Tag :color="logLevelOptions[row.level]?.color">
        <a
          class="link"
          href="javaScript:void(0);"
          @click="onFilter('level', row.level)"
          >{{ logLevelOptions[row.level]?.label }}
        </a>
      </Tag>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[SystemLogPermissions.Default]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpAuditLogging.ShowLogDialog') }}
        </Button>
      </div>
    </template>
  </Grid>
  <LoggingDrawer :log-level-options="logLevelOptions" />
</template>

<style lang="scss" scoped></style>
