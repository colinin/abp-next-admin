<script setup lang="ts">
import type { VbenFormProps, VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { SecurityLogDto } from '../../types/security-logs';

import { h } from 'vue';

import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined } from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { deleteApi, getPagedListApi } from '../../api/security-logs';
import { SecurityLogPermissions } from '../../constants/permissions';

defineOptions({
  name: 'SecurityLogTable',
});

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
      align: 'left',
      field: 'creationTime',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      title: $t('AbpAuditLogging.CreationTime'),
      width: 180,
    },
    {
      align: 'left',
      field: 'identity',
      title: $t('AbpAuditLogging.Identity'),
      width: 180,
    },
    {
      align: 'left',
      field: 'userName',
      title: $t('AbpAuditLogging.UserName'),
      width: 150,
    },
    {
      align: 'left',
      field: 'clientId',
      title: $t('AbpAuditLogging.ClientId'),
      width: 200,
    },
    {
      align: 'left',
      field: 'clientIpAddress',
      title: $t('AbpAuditLogging.ClientIpAddress'),
      width: 200,
    },
    {
      align: 'left',
      field: 'applicationName',
      title: $t('AbpAuditLogging.ApplicationName'),
      width: 200,
    },
    {
      align: 'left',
      field: 'tenantName',
      title: $t('AbpAuditLogging.TenantName'),
      width: 180,
    },
    {
      align: 'left',
      field: 'actions',
      title: $t('AbpAuditLogging.Actions'),
      width: 180,
    },
    {
      align: 'left',
      field: 'correlationId',
      title: $t('AbpAuditLogging.CorrelationId'),
      width: 200,
    },
    {
      align: 'left',
      field: 'browserInfo',
      title: $t('AbpAuditLogging.BrowserInfo'),
      width: 'auto',
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 150,
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
  cellClick: () => {},
};
const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const handleDelete = (row: SecurityLogDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessage'),
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.SuccessfullyDeleted'));
      query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};
</script>

<template>
  <Grid :table-title="$t('AbpAuditLogging.SecurityLog')">
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[SecurityLogPermissions.Delete]"
          @click="handleDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
</template>

<style lang="scss" scoped>
.checkbox-box {
  display: flex;
  justify-content: center;
}
</style>
