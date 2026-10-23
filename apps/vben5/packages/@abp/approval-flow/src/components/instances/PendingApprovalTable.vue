<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { ApprovalPendingTaskDto } from '../../types/approval-flow';

import { onMounted, ref } from 'vue';

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
    { field: 'instanceTitle', minWidth: 220, title: '审批标题' },
    { field: 'title', minWidth: 180, title: '任务' },
    { field: 'nodeName', minWidth: 140, title: '节点' },
    {
      field: 'createdAt',
      slots: { default: 'createdAt' },
      title: '到达时间',
      width: 180,
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
    message.error('待办加载失败');
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
  <Grid table-title="待办审批">
    <template #createdAt="{ row }">
      {{ formatDateTime(row.createdAt) }}
    </template>
    <template #action="{ row }">
      <Button
        size="small"
        type="link"
        @click="openDetail(row as ApprovalPendingTaskDto)"
      >
        查看
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
