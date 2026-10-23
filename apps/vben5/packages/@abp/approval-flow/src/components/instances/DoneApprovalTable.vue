<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { DoneDto } from '../../types/done';

import { ref } from 'vue';

import { useVbenVxeGrid } from '@abp/ui';
import { Button } from 'ant-design-vue';

import { useApprovalTasksApi } from '../../api/useApprovalTasksApi';
import {
  actionText,
  formatDateTime,
  formatDuration,
} from './approval-table-utils';
import ApprovalInstanceDetailModal from './ApprovalInstanceDetailModal.vue';

const tasksApi = useApprovalTasksApi();

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

const gridOptions: VxeGridProps<DoneDto> = {
  columns: [
    { field: 'instanceTitle', minWidth: 240, title: '审批标题' },
    { field: 'nodeName', minWidth: 160, title: '节点' },
    {
      field: 'actionKey',
      slots: { default: 'actionKey' },
      title: '动作',
      width: 120,
    },
    { field: 'actorDisplayName', title: '处理人', width: 120 },
    { field: 'comment', minWidth: 180, title: '审批意见' },
    {
      field: 'firstViewedAt',
      slots: { default: 'firstViewedAt' },
      title: '首次查看时间',
      width: 180,
    },
    {
      field: 'completedAt',
      slots: { default: 'completedAt' },
      title: '办理时间',
      width: 180,
    },
    {
      field: 'decidedAt',
      slots: { default: 'decidedAt' },
      title: '处理时间',
      width: 180,
    },
    {
      field: 'duration',
      slots: { default: 'duration' },
      title: '办理时长',
      width: 160,
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
        return await tasksApi.getDoneListApi({
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

const [Grid] = useVbenVxeGrid({
  formOptions,
  gridOptions,
});

function openDetail(row: DoneDto) {
  detailInstanceId.value = row.instanceId;
  detailOpen.value = true;
}
</script>

<template>
  <Grid table-title="已办审批">
    <template #actionKey="{ row }">{{ actionText(row.actionKey) }}</template>
    <template #firstViewedAt="{ row }">
      {{ formatDateTime(row.firstViewedAt) }}
    </template>
    <template #completedAt="{ row }">
      {{ formatDateTime(row.completedAt) }}
    </template>
    <template #decidedAt="{ row }">
      {{ formatDateTime(row.decidedAt) }}
    </template>
    <template #duration="{ row }">
      {{ formatDuration(row.firstViewedAt, row.completedAt) }}
    </template>
    <template #action="{ row }">
      <Button size="small" type="link" @click="openDetail(row as DoneDto)">
        查看
      </Button>
    </template>
  </Grid>

  <ApprovalInstanceDetailModal
    v-model:open="detailOpen"
    :instance-id="detailInstanceId"
  />
</template>
