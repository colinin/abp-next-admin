<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { AuditLogDto } from '../../types/audit-logs';

import { defineAsyncComponent, h, ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';
import { downloadFileFromUrl } from '@vben/utils';

import { useAbpDataTable } from '@abp/components/crud/table';
import { formatToDateTime } from '@abp/core';
import { DeleteOutlined, FileExcelOutlined } from '@ant-design/icons-vue';
import { Button, Tag } from 'ant-design-vue';

import { useAuditLogsApi } from '../../api/useAuditLogsApi';
import { AuditLogPermissions } from '../../constants/permissions';
import { useAuditlogs } from '../../hooks/useAuditlogs';
import { httpMethodOptions, httpStatusCodeOptions } from './mapping';

defineOptions({
  name: 'AuditLogTable',
});

const { deleteApi, deleteManyApi, getExportApiUrl, getPagedListApi } =
  useAuditLogsApi();
const { getHttpMethodColor, getHttpStatusCodeColor } = useAuditlogs();

const selectedKeys = ref<string[]>([]);
const [AuditLogDrawer, logDrawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(
    () => import('./AuditLogDrawer.vue'),
  ),
});

const searchFormOptions: VbenFormProps = {
  collapsed: true,
  collapsedRows: 2,
  fieldMappingTime: [
    [
      'executionTime',
      ['startTime', 'endTime'],
      (value) => formatToDateTime(value),
    ],
  ],
  schema: [
    {
      component: 'RangePicker',
      componentProps: {
        showTime: true,
        valueFormat: 'YYYY-MM-DD HH:mm:ss',
      },
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
        render: () => h('span', $t('AbpAuditLogging.HasException')),
      },
      fieldName: 'hasException',
      label: $t('AbpAuditLogging.HasException'),
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
  wrapperClass: 'grid-cols-4',
};

const gridOptions: VxeGridProps<AuditLogDto> = {
  columns: [
    {
      align: 'center',
      type: 'checkbox',
    },
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
      formatter: ({ cellValue }) =>
        cellValue ? formatToDateTime(cellValue) : cellValue,
      field: 'executionTime',
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
      slots: { default: 'clientIpAddress' },
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
  ],
  exportConfig: {},
  keepSource: true,
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: true,
    refreshOptions: { code: 'query' },
    zoom: true,
  },
};

function showLog(row: AuditLogDto) {
  logDrawerApi.setData(row);
  logDrawerApi.open();
}

async function onBulkDelete() {
  await tableApi.deleteMany(selectedKeys.value);
}

async function onExportToExcel() {
  const input = await tableApi.formApi.getValues();
  downloadFileFromUrl({ source: getExportApiUrl(input) });
}

function onFilter(field: string, value: any) {
  tableApi.formApi.setFieldValue(field, value);
  tableApi.formApi.validateAndSubmit();
}

const [DataTable, tableApi] = useAbpDataTable<AuditLogDto>({
  actionColumn: {
    title: $t('AbpUi.Actions'),
    width: 220,
  },
  editAuth: [AuditLogPermissions.Default],
  editText: $t('AbpAuditLogging.ShowLogDialog'),
  edit: showLog,
  deleteAuth: [AuditLogPermissions.Delete],
  delete: (dto) => deleteApi(dto.id),
  deleteMany: (ids) => deleteManyApi({ ids }),
  deleteWarnMessage: () => $t('AbpUi.ItemWillBeDeletedMessage'),
  formOptions: searchFormOptions,
  gridEvents: {
    onCheckboxChange(rows) {
      selectedKeys.value = rows.map((x) => x.id);
    },
  },
  gridOptions,
  rowKey: 'id',
  load: ({ pager, searchValues, sortable }) =>
    getPagedListApi({
      maxResultCount: pager.maxResultCount,
      skipCount: pager.skipCount,
      sorting: sortable?.sorting,
      ...searchValues,
    }),
});
</script>

<template>
  <DataTable :table-title="$t('AbpAuditLogging.AuditLog')">
    <template #toolbar-tools>
      <div class="flex flex-row gap-2">
        <Button
          v-if="selectedKeys.length > 0"
          :icon="h(DeleteOutlined)"
          danger
          ghost
          type="primary"
          v-access:code="[AuditLogPermissions.Delete]"
          @click="onBulkDelete"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Button
          :icon="h(FileExcelOutlined)"
          ghost
          type="primary"
          v-access:code="[AuditLogPermissions.Export]"
          @click="onExportToExcel"
        >
          {{ $t('AbpAuditLogging.ExportToExcel') }}
        </Button>
      </div>
    </template>

    <template #clientIpAddress="{ row }">
      <Tag v-if="row.extraProperties?.Location" color="blue">
        {{ row.extraProperties?.Location }}
      </Tag>
      <span>{{ row.clientIpAddress }}</span>
    </template>

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
      >
        {{ row.url }}
      </a>
    </template>
  </DataTable>

  <AuditLogDrawer />
</template>

<style lang="scss" scoped></style>
