<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { CarbonCopyDto } from '../../types/carbon-copies';

import { ref } from 'vue';

import { useVbenVxeGrid } from '@abp/ui';
import { Button, Tag } from 'ant-design-vue';

import { useApprovalCarbonCopiesApi } from '../../api/useApprovalFlowApi';
import { formatDateTime } from './approval-table-utils';
import ApprovalInstanceDetailModal from './ApprovalInstanceDetailModal.vue';

const carbonCopiesApi = useApprovalCarbonCopiesApi();

const detailOpen = ref(false);
const detailInstanceId = ref<null | string>(null);

const formOptions: VbenFormProps = {
  collapsed: true,
  schema: [
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: '搜索',
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<CarbonCopyDto> = {
  columns: [
    { field: 'title', minWidth: 240, title: '标题' },
    { field: 'summary', minWidth: 200, title: '摘要' },
    { field: 'senderDisplayName', minWidth: 140, title: '发起人' },
    {
      field: 'notifiedAt',
      slots: { default: 'notifiedAt' },
      title: '抄送时间',
      width: 180,
    },
    {
      field: 'isRead',
      slots: { default: 'readState' },
      title: '状态',
      width: 90,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: '操作',
      width: 100,
    },
  ],
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await carbonCopiesApi.getListApi({
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
    refresh: true,
    refreshOptions: { code: 'query' },
    zoom: true,
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridOptions,
});

async function openCarbonCopy(item: CarbonCopyDto) {
  if (!item.instanceId) {
    return;
  }
  try {
    if (!item.isRead) {
      await carbonCopiesApi.markReadApi(item.id);
    }
  } catch {
    // ignore read-state errors and still open the detail
  }
  detailInstanceId.value = item.instanceId;
  detailOpen.value = true;
  await gridApi.query();
}
</script>

<template>
  <Grid table-title="抄送我的">
    <template #notifiedAt="{ row }">
      {{ formatDateTime(row.notifiedAt) }}
    </template>
    <template #readState="{ row }">
      <Tag :color="row.isRead ? 'default' : 'warning'">
        {{ row.isRead ? '已读' : '未读' }}
      </Tag>
    </template>
    <template #action="{ row }">
      <Button
        size="small"
        type="link"
        @click="openCarbonCopy(row as CarbonCopyDto)"
      >
        查看
      </Button>
    </template>
  </Grid>

  <ApprovalInstanceDetailModal
    v-model:open="detailOpen"
    :instance-id="detailInstanceId"
  />
</template>
