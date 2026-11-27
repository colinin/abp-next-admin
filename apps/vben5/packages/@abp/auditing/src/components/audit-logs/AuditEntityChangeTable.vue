<script setup lang="ts">
import type { TableProps } from 'ant-design-vue';

import type { EntityChangeDto } from '../../types/entity-changes';

import { h } from 'vue';

import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';
import { downloadFileFromUrl } from '@vben/utils';

import { useAbpEditorModal } from '@abp/components/crud/modal';
import { useAbpDataTable } from '@abp/components/crud/table';
import { formatToDateTime } from '@abp/core';
import { FileExcelOutlined } from '@ant-design/icons-vue';
import { Button, Table, Tag } from 'ant-design-vue';

import { useEntityChangesApi } from '../../api/useEntityChangesApi';
import { AuditLogPermissions } from '../../constants/permissions';
import { useAuditlogs } from '../../hooks/useAuditlogs';
import { ChangeType } from '../../types/entity-changes';

const { getApi, getExportApiUrl, getPagedListApi } = useEntityChangesApi();
const { getChangeTypeColor, getChangeTypeValue } = useAuditlogs();

const propertyChangeColumns: TableProps['columns'] = [
  {
    align: 'left',
    dataIndex: 'propertyName',
    title: $t('AbpAuditLogging.PropertyName'),
    width: 120,
  },
  {
    align: 'left',
    className: 'font-medium text-green-600',
    dataIndex: 'newValue',
    title: $t('AbpAuditLogging.NewValue'),
    width: 200,
  },
  {
    align: 'left',
    className: 'font-medium text-red-600',
    dataIndex: 'originalValue',
    title: $t('AbpAuditLogging.OriginalValue'),
    width: 200,
  },
  {
    align: 'left',
    dataIndex: 'propertyTypeFullName',
    title: $t('AbpAuditLogging.PropertyTypeFullName'),
    width: 220,
  },
];

const [Modal, modalApi] = useAbpEditorModal<EntityChangeDto, EntityChangeDto>({
  modalOptions: {
    class: 'w-[800px]',
  },
  idKey(dto) {
    return dto.id;
  },
  title(_mode, dto) {
    return dto?.entityTypeFullName;
  },
  description(_mode, dto) {
    return `Id: ${dto?.entityId}`;
  },
  formOptions: {
    schema: [
      {
        component: 'Input',
        fieldName: 'propertyChanges',
        labelWidth: 0,
      },
    ],
  },
  get: getApi,
});

const [DataTable, tableApi] = useAbpDataTable<EntityChangeDto>({
  editAuth: [AuditLogPermissions.Default],
  edit(dto) {
    modalApi.view(dto.id);
  },
  editIcon: createIconifyIcon('ant-design:search-outlined'),
  editText: $t('AbpAuditLogging.ChangeDetails'),
  formOptions: {
    codec: {
      decode(values) {
        let executionTime: [string, string] | undefined;
        if (values.startTime !== undefined && values.endTime !== undefined) {
          executionTime = [
            formatToDateTime(values.startTime),
            formatToDateTime(values.endTime),
          ];
        }
        return {
          ...values,
          executionTime,
        };
      },
      encode(values) {
        return {
          ...values,
          startTime: values.executionTime?.[0].valueOf(),
          endTime: values.executionTime?.[1].valueOf(),
          executionTime: undefined,
        };
      },
    },
    collapsedRows: 2,
    schema: [
      {
        component: 'Select',
        componentProps: {
          allowClear: true,
          options: [
            { label: $t('AbpAuditLogging.Created'), value: ChangeType.Created },
            { label: $t('AbpAuditLogging.Updated'), value: ChangeType.Updated },
            { label: $t('AbpAuditLogging.Deleted'), value: ChangeType.Deleted },
          ],
        },
        fieldName: 'changeType',
        label: $t('AbpAuditLogging.ChangeType'),
      },
      {
        component: 'RangePicker',
        componentProps: {
          showTime: true,
          valueFormat: 'YYYY-MM-DD HH:mm:ss',
        },
        fieldName: 'executionTime',
        formItemClass: 'col-span-2 items-baseline',
        label: $t('AbpAuditLogging.ChangeTime'),
      },
      {
        component: 'Input',
        componentProps: {
          allowClear: true,
        },
        fieldName: 'entityId',
        label: $t('AbpAuditLogging.EntityId'),
      },
      {
        component: 'Input',
        componentProps: {
          allowClear: true,
        },
        fieldName: 'entityTypeFullName',
        formItemClass: 'col-span-2 items-baseline',
        label: $t('AbpAuditLogging.EntityTypeFullName'),
      },
    ],
  },
  gridOptions: {
    columns: [
      {
        align: 'center',
        field: 'changeType',
        slots: { default: 'type' },
        sortable: true,
        title: $t('AbpAuditLogging.ChangeType'),
        width: 100,
      },
      {
        align: 'left',
        field: 'changeTime',
        cellType: 'date',
        formatter: 'AutoFormatDate',
        sortable: true,
        title: $t('AbpAuditLogging.ChangeTime'),
        width: 200,
      },
      {
        align: 'left',
        field: 'entityTypeFullName',
        sortable: true,
        title: $t('AbpAuditLogging.EntityTypeFullName'),
        minWidth: 300,
      },
      {
        align: 'left',
        field: 'entityId',
        sortable: true,
        title: $t('AbpAuditLogging.EntityId'),
        width: 280,
      },
      {
        align: 'left',
        field: 'entityTenantId',
        sortable: true,
        title: $t('AbpAuditLogging.TenantId'),
        width: 280,
      },
    ],
  },
  load(params) {
    return getPagedListApi({
      sorting: params.sorting,
      skipCount: params.pager.skipCount,
      maxResultCount: params.pager.maxResultCount,
      ...params.searchValues,
    });
  },
});

async function onExportToExcel() {
  const input = await tableApi.formApi.getValues();
  const downloadUrl = getExportApiUrl(input);
  downloadFileFromUrl({ source: downloadUrl });
}
</script>

<template>
  <DataTable>
    <template #toolbar-tools>
      <Button
        :icon="h(FileExcelOutlined)"
        ghost
        type="primary"
        v-access:code="[AuditLogPermissions.Export]"
        @click="onExportToExcel"
      >
        {{ $t('AbpAuditLogging.ExportToExcel') }}
      </Button>
    </template>
    <template #type="{ row }">
      <Tag :color="getChangeTypeColor(row.changeType)">
        {{ getChangeTypeValue(row.changeType) }}
      </Tag>
    </template>
  </DataTable>
  <Modal>
    <template #form-property-changes="{ values }">
      <Table
        :columns="propertyChangeColumns"
        :data-source="values.propertyChanges"
      />
    </template>
  </Modal>
</template>

<style scoped></style>
