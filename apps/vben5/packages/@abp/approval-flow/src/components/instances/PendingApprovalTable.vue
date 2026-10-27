<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { ApprovalPendingTaskDto } from '../../types/approval-flow';

import { onMounted, ref } from 'vue';

import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import { Button, message } from 'ant-design-vue';

import { useApprovalTasksApi } from '../../api/useApprovalTasksApi';
import { formatDateTime } from './approval-table-utils';
import ApprovalInstanceDetailModal from './ApprovalInstanceDetailModal.vue';

const tasksApi = useApprovalTasksApi();

const allTasks = ref<ApprovalPendingTaskDto[]>([]);
const detailOpen = ref(false);
const detailInstanceId = ref<null | string>(null);
const detailTask = ref<ApprovalPendingTaskDto | null>(null);

const gridOptions: VxeGridProps<ApprovalPendingTaskDto> = {
  columns: [
    {
      field: 'instanceTitle',
      minWidth: 220,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Pending:InstanceTitle'),
    },
    {
      field: 'title',
      minWidth: 180,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Pending:Task'),
    },
    {
      field: 'nodeName',
      minWidth: 140,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Pending:Node'),
    },
    {
      field: 'createdAt',
      slots: { default: 'createdAt' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Pending:ArrivedAt'),
      width: 180,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Actions'),
      width: 100,
    },
  ],
  keepSource: true,
  pagerConfig: {
    enabled: true,
    pageSize: 20,
    pageSizes: [10, 20, 50, 100],
  },
  proxyConfig: {
    ajax: {
      query: async ({ page }) => {
        const start = (page.currentPage - 1) * page.pageSize;
        return {
          items: allTasks.value.slice(start, start + page.pageSize),
          totalCount: allTasks.value.length,
        };
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
    refreshOptions: {
      queryMethod: loadTasks,
    },
    zoom: true,
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  gridOptions,
});

async function loadTasks() {
  try {
    const result = await tasksApi.getPendingListApi();
    allTasks.value = result.items ?? [];
    await gridApi.query();
  } catch {
    message.error($t('ElsaNextApprovalFlow.ApprovalFlow:Pending:LoadFailed'));
  }
}

async function openDetail(task: ApprovalPendingTaskDto) {
  try {
    await tasksApi.markViewedApi(task.taskId);
  } catch {
    // Viewing must not be blocked by a read-tracking failure.
  }
  detailInstanceId.value = task.instanceId;
  detailTask.value = task;
  detailOpen.value = true;
}

async function onDecided() {
  detailTask.value = null;
  await loadTasks();
}

onMounted(loadTasks);
</script>

<template>
  <Grid :table-title="$t('ElsaNextApprovalFlow.ApprovalFlow:Pending:Title')">
    <template #createdAt="{ row }">
      {{ formatDateTime(row.createdAt) }}
    </template>
    <template #action="{ row }">
      <Button
        size="small"
        type="link"
        @click="openDetail(row as ApprovalPendingTaskDto)"
      >
        {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:View') }}
      </Button>
    </template>
  </Grid>

  <ApprovalInstanceDetailModal
    v-model:open="detailOpen"
    :instance-id="detailInstanceId"
    :task="detailTask"
    @decided="onDecided"
  />
</template>
