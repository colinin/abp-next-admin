<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { ApprovalTemplateDto } from '../../types/approval-flow';

import { ref } from 'vue';

import { useVbenVxeGrid } from '@abp/ui';
import { Button, message, Modal, Space, Tag } from 'ant-design-vue';

import { useApprovalTemplatesApi } from '../../api/useApprovalFlowApi';
import ApprovalFlowWizard from '../antflow/ApprovalWizard.vue';
import StartApprovalModal from './StartApprovalModal.vue';

const templatesApi = useApprovalTemplatesApi();

const wizardOpen = ref(false);
const wizardTemplateId = ref<null | string>(null);
const startOpen = ref(false);
const startTemplate = ref<ApprovalTemplateDto | null>(null);

const formOptions: VbenFormProps = {
  collapsed: false,
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

const gridOptions: VxeGridProps<ApprovalTemplateDto> = {
  columns: [
    { field: 'displayName', minWidth: 220, title: '名称' },
    { field: 'name', minWidth: 180, title: '编码' },
    { field: 'category', title: '分类', width: 120 },
    {
      field: 'status',
      slots: { default: 'status' },
      title: '状态',
      width: 110,
    },
    {
      field: 'form',
      slots: { default: 'boundForm' },
      title: '表单',
      width: 90,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: '操作',
      width: 300,
    },
  ],
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await templatesApi.getListApi({
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

const gridEvents: VxeGridListeners<ApprovalTemplateDto> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

function openWizard(template?: ApprovalTemplateDto) {
  wizardTemplateId.value = template?.id ?? null;
  wizardOpen.value = true;
}

function closeWizard() {
  wizardOpen.value = false;
  wizardTemplateId.value = null;
}

function onWizardDone() {
  closeWizard();
  gridApi.query();
}

function openStart(template: ApprovalTemplateDto) {
  startTemplate.value = template;
  startOpen.value = true;
}

function onStarted() {
  message.success('已发起审批，可在「我的审批」中查看进度');
}

async function removeTemplate(template: ApprovalTemplateDto) {
  Modal.confirm({
    content: `确定删除模板「${template.displayName}」吗？`,
    onOk: async () => {
      await templatesApi.deleteApi(template.id);
      message.success('已删除');
      await gridApi.query();
    },
    title: '删除审批模板',
  });
}
</script>

<template>
  <Grid table-title="审批模板">
    <template #toolbar-tools>
      <Button type="primary" @click="openWizard()">新建审批</Button>
    </template>

    <template #status="{ row }">
      <Tag :color="row.isPublished ? 'green' : 'orange'">
        {{ row.isPublished ? '已发布' : '草稿' }}
      </Tag>
    </template>
    <template #boundForm="{ row }">
      <Tag v-if="row.formDefinitionId" color="blue">已绑定</Tag>
      <span v-else class="text-gray-400">-</span>
    </template>
    <template #action="{ row }">
      <Space size="small">
        <Button
          v-if="row.isPublished"
          size="small"
          type="link"
          @click="openStart(row as ApprovalTemplateDto)"
        >
          发起
        </Button>
        <Button
          size="small"
          type="link"
          @click="openWizard(row as ApprovalTemplateDto)"
        >
          设计
        </Button>
        <Button
          danger
          size="small"
          type="link"
          @click="removeTemplate(row as ApprovalTemplateDto)"
        >
          删除
        </Button>
      </Space>
    </template>
  </Grid>

  <ApprovalFlowWizard
    v-if="wizardOpen"
    :template-id="wizardTemplateId"
    @cancel="closeWizard"
    @done="onWizardDone"
  />
  <StartApprovalModal
    v-model:open="startOpen"
    :template="startTemplate"
    @started="onStarted"
  />
</template>
