<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type {
  ApprovalInstanceDto,
  ApprovalRecordDto,
  ApprovalTemplateDto,
} from '../../types/approval-flow';

import { ref } from 'vue';

import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import { Button, message, Modal, Space, Tag } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

import { useApprovalInstancesApi } from '../../api/useApprovalInstancesApi';
import { useApprovalTemplatesApi } from '../../api/useApprovalTemplatesApi';
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
      label: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Search'),
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<ApprovalInstanceDto> = {
  columns: [
    {
      field: 'title',
      minWidth: 220,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Title'),
    },
    {
      field: 'templateName',
      minWidth: 160,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Template'),
    },
    {
      field: 'status',
      slots: { default: 'status' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Status'),
      width: 110,
    },
    {
      field: 'startedAt',
      slots: { default: 'startedAt' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:StartedAt'),
      width: 180,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Actions'),
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
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Time'),
    width: 180,
  },
  {
    field: 'actionKey',
    slots: { default: 'actionKey' },
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Action'),
    width: 120,
  },
  {
    field: 'actorDisplayName',
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Actor'),
    width: 120,
  },
  {
    field: 'firstViewedAt',
    slots: { default: 'firstViewedAt' },
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:FirstViewedAt'),
    width: 180,
  },
  {
    field: 'completedAt',
    slots: { default: 'completedAt' },
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:CompletedAt'),
    width: 180,
  },
  {
    field: 'duration',
    slots: { default: 'duration' },
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Duration'),
    width: 160,
  },
  {
    field: 'comment',
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Comment'),
    width: 200,
  },
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
    message.error(
      $t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:TemplateLoadFailed'),
    );
  }
}

function withdraw(instance: ApprovalInstanceDto) {
  Modal.confirm({
    content: $t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:WithdrawConfirm'),
    onOk: async () => {
      await instancesApi.withdrawApi(instance.id);
      message.success(
        $t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:Withdrawn'),
      );
      await gridApi.query();
    },
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:WithdrawTitle'),
  });
}

async function onRestarted() {
  await gridApi.query();
}

async function openRecords(instance: ApprovalInstanceDto) {
  try {
    const result = await instancesApi.getRecordsApi(instance.id);
    records.value = result?.items ?? [];
    recordsOpen.value = true;
  } catch {
    message.error(
      $t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:RecordsLoadFailed'),
    );
  }
}
</script>

<template>
  <Grid :table-title="$t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:Title')">
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
          {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Detail') }}
        </Button>
        <Button
          size="small"
          type="link"
          @click="openRecords(row as ApprovalInstanceDto)"
        >
          {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:Records') }}
        </Button>
        <Button
          v-if="row.status === 'Running'"
          danger
          size="small"
          type="link"
          @click="withdraw(row as ApprovalInstanceDto)"
        >
          {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:Withdraw') }}
        </Button>
        <Button
          v-if="row.status === 'Returned'"
          size="small"
          type="link"
          @click="openRestart(row as ApprovalInstanceDto)"
        >
          {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:Restart') }}
        </Button>
      </Space>
    </template>
  </Grid>

  <Modal
    v-model:open="recordsOpen"
    :footer="null"
    :title="$t('ElsaNextApprovalFlow.ApprovalFlow:Initiated:Records')"
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
