<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type {
  ApprovalInstanceDto,
  ApprovalRecordDto,
  ApprovalTemplateDto,
} from '../../types/approval-flow';

import { ref } from 'vue';

import { useVbenVxeGrid } from '@abp/ui';
import { Button, message, Modal, Space, Tag } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

import {
  useApprovalInstancesApi,
  useApprovalTemplatesApi,
} from '../../api/useApprovalFlowApi';
import StartApprovalModal from '../templates/StartApprovalModal.vue';
import {
  actionText,
  formatDateTime,
  formatDuration,
  statusColor,
  statusText,
} from './approval-table-utils';
import ApprovalInstanceDetailModal from './ApprovalInstanceDetailModal.vue';

const instancesApi = useApprovalInstancesApi();
const templatesApi = useApprovalTemplatesApi();

const recordsOpen = ref(false);
const records = ref<ApprovalRecordDto[]>([]);
const detailOpen = ref(false);
const detailInstanceId = ref<null | string>(null);
const restartOpen = ref(false);
const restartTemplate = ref<ApprovalTemplateDto | null>(null);
const restartData = ref<Record<string, unknown>>({});

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

const gridOptions: VxeGridProps<ApprovalInstanceDto> = {
  columns: [
    { field: 'title', minWidth: 220, title: '标题' },
    { field: 'templateName', minWidth: 160, title: '模板' },
    {
      field: 'status',
      slots: { default: 'status' },
      title: '状态',
      width: 110,
    },
    {
      field: 'startedAt',
      slots: { default: 'startedAt' },
      title: '发起时间',
      width: 180,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: '操作',
      width: 220,
    },
  ],
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await instancesApi.getListApi({
          onlyMine: true,
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

const recordColumns: VxeGridProps<ApprovalRecordDto>['columns'] = [
  {
    field: 'occurredAt',
    slots: { default: 'occurredAt' },
    title: '时间',
    width: 180,
  },
  {
    field: 'actionKey',
    slots: { default: 'actionKey' },
    title: '动作',
    width: 120,
  },
  { field: 'actorDisplayName', title: '处理人', width: 120 },
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
    field: 'duration',
    slots: { default: 'duration' },
    title: '办理时长',
    width: 160,
  },
  { field: 'comment', title: '审批意见', width: 200 },
];

function openDetail(instance: ApprovalInstanceDto) {
  detailInstanceId.value = instance.id;
  detailOpen.value = true;
}

function parseFormData(json?: null | string): Record<string, unknown> {
  if (!json) {
    return {};
  }
  try {
    return JSON.parse(json) as Record<string, unknown>;
  } catch {
    return {};
  }
}

async function openRestart(instance: ApprovalInstanceDto) {
  try {
    restartTemplate.value = await templatesApi.getApi(instance.templateId);
    restartData.value = parseFormData(instance.formDataJson);
    restartOpen.value = true;
  } catch {
    message.error('审批模板加载失败');
  }
}

function withdraw(instance: ApprovalInstanceDto) {
  Modal.confirm({
    content: '撤销后该审批流程将终止，确定继续吗？',
    onOk: async () => {
      await instancesApi.withdrawApi(instance.id);
      message.success('审批已撤销');
      await gridApi.query();
    },
    title: '撤销审批',
  });
}

async function onRestarted() {
  await gridApi.query();
}

async function openRecords(instance: ApprovalInstanceDto) {
  try {
    records.value =
      (await instancesApi.getRecordsApi(instance.id))?.items ?? [];
    recordsOpen.value = true;
  } catch {
    message.error('审批记录加载失败');
  }
}
</script>

<template>
  <Grid table-title="我发起的">
    <template #status="{ row }">
      <Tag :color="statusColor(row.status)">{{ statusText(row.status) }}</Tag>
    </template>
    <template #startedAt="{ row }">
      {{ formatDateTime(row.startedAt) }}
    </template>
    <template #action="{ row }">
      <Space size="small">
        <Button
          size="small"
          type="link"
          @click="openDetail(row as ApprovalInstanceDto)"
        >
          详情
        </Button>
        <Button
          size="small"
          type="link"
          @click="openRecords(row as ApprovalInstanceDto)"
        >
          审批记录
        </Button>
        <Button
          v-if="row.status === 'Running'"
          danger
          size="small"
          type="link"
          @click="withdraw(row as ApprovalInstanceDto)"
        >
          撤销
        </Button>
        <Button
          v-if="row.status === 'Returned'"
          size="small"
          type="link"
          @click="openRestart(row as ApprovalInstanceDto)"
        >
          重新发起
        </Button>
      </Space>
    </template>
  </Grid>

  <Modal
    v-model:open="recordsOpen"
    :footer="null"
    title="审批记录"
    width="720px"
  >
    <VxeGrid
      :columns="recordColumns"
      :data="records"
      :pager-config="{ enabled: false }"
      border
      size="small"
    >
      <template #occurredAt="{ row }">
        {{ formatDateTime(row.occurredAt) }}
      </template>
      <template #actionKey="{ row }">{{ actionText(row.actionKey) }}</template>
      <template #firstViewedAt="{ row }">
        {{ formatDateTime(row.firstViewedAt) }}
      </template>
      <template #completedAt="{ row }">
        {{ formatDateTime(row.completedAt) }}
      </template>
      <template #duration="{ row }">
        {{ formatDuration(row.firstViewedAt, row.completedAt) }}
      </template>
    </VxeGrid>
  </Modal>

  <StartApprovalModal
    v-model:open="restartOpen"
    :initial-data="restartData"
    :template="restartTemplate"
    @started="onRestarted"
  />

  <ApprovalInstanceDetailModal
    v-model:open="detailOpen"
    :instance-id="detailInstanceId"
  />
</template>
