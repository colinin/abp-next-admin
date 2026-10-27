<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { ApprovalTemplateDto } from '../../types/approval-flow';

import { ref } from 'vue';

import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import { Button, message, Modal, Space, Tag } from 'ant-design-vue';

import { useApprovalTemplatesApi } from '../../api/useApprovalTemplatesApi';
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
      label: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Search'),
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<ApprovalTemplateDto> = {
  columns: [
    {
      field: 'displayName',
      minWidth: 220,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Name'),
    },
    {
      field: 'name',
      minWidth: 180,
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Code'),
    },
    {
      field: 'category',
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Category'),
      width: 120,
    },
    {
      field: 'status',
      slots: { default: 'status' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Status'),
      width: 110,
    },
    {
      field: 'form',
      slots: { default: 'boundForm' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Form'),
      width: 90,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Actions'),
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
  message.success($t('ElsaNextApprovalFlow.ApprovalFlow:Templates:Started'));
}

async function removeTemplate(template: ApprovalTemplateDto) {
  Modal.confirm({
    content: $t(
      'ElsaNextApprovalFlow.ApprovalFlow:Templates:DeleteConfirm',
    ).replace('{0}', template.displayName),
    onOk: async () => {
      await templatesApi.deleteApi(template.id);
      message.success(
        $t('ElsaNextApprovalFlow.ApprovalFlow:Templates:Deleted'),
      );
      await gridApi.query();
    },
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Templates:DeleteTitle'),
  });
}
</script>

<template>
  <Grid :table-title="$t('ElsaNextApprovalFlow.ApprovalFlow:Templates:Title')">
    <template #toolbar-tools>
      <Button type="primary" @click="openWizard()">
        {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Templates:Create') }}
      </Button>
    </template>

    <template #status="{ row }">
      <Tag :color="row.isPublished ? 'green' : 'orange'">
        {{
          row.isPublished
            ? $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Published')
            : $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Draft')
        }}
      </Tag>
    </template>
    <template #boundForm="{ row }">
      <Tag v-if="row.formDefinitionId" color="blue">
        {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Bound') }}
      </Tag>
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
          {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Start') }}
        </Button>
        <Button
          size="small"
          type="link"
          @click="openWizard(row as ApprovalTemplateDto)"
        >
          {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Design') }}
        </Button>
        <Button
          danger
          size="small"
          type="link"
          @click="removeTemplate(row as ApprovalTemplateDto)"
        >
          {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Delete') }}
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
