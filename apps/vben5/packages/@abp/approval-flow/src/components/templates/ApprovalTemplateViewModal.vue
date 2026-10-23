<script setup lang="ts">
import type { VxeGridProps } from 'vxe-table';

import type {
  ApprovalTemplateDto,
  FormFieldDto,
} from '../../types/approval-flow';

import { ref, watch } from 'vue';

import {
  Descriptions,
  DescriptionsItem,
  message,
  Modal,
  Spin,
  Tabs,
  Tag,
} from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

import { useApprovalFormsApi } from '../../api/useApprovalFormsApi';
import ApprovalFlowDesigner from '../designer/ApprovalFlowDesigner.vue';

const props = defineProps<{
  open: boolean;
  template: ApprovalTemplateDto | null;
}>();

const emit = defineEmits<{
  'update:open': [boolean];
}>();

const formsApi = useApprovalFormsApi();
const loading = ref(false);
const fields = ref<FormFieldDto[]>([]);
const activeTab = ref('basic');

const fieldColumns: VxeGridProps<FormFieldDto>['columns'] = [
  { field: 'label', minWidth: 200, title: '字段名称' },
  { field: 'key', minWidth: 160, title: '字段编码' },
  { field: 'type', title: '类型', width: 140 },
  {
    field: 'permission',
    slots: { default: 'permission' },
    title: '权限',
    width: 90,
  },
  {
    field: 'isRequired',
    slots: { default: 'isRequired' },
    title: '必填',
    width: 80,
  },
];

watch(
  () => [props.open, props.template] as const,
  async ([open, template]) => {
    if (!open || !template) {
      return;
    }

    activeTab.value = 'basic';
    fields.value = [];
    if (!template.formDefinitionId) {
      return;
    }

    loading.value = true;
    try {
      const form = await formsApi.getApi(template.formDefinitionId);
      fields.value = form.fields ?? [];
    } catch {
      message.error('表单信息加载失败');
    } finally {
      loading.value = false;
    }
  },
  { immediate: true },
);

function statusColor(isPublished: boolean): string {
  return isPublished ? 'green' : 'orange';
}

function statusText(isPublished: boolean): string {
  return isPublished ? '已发布' : '草稿';
}

function permissionText(permission: string): string {
  switch (permission) {
    case 'E': {
      return '可编辑';
    }
    case 'H': {
      return '隐藏';
    }
    case 'R': {
      return '只读';
    }
    default: {
      return permission;
    }
  }
}
</script>

<template>
  <Modal
    :footer="null"
    :open="open"
    :width="1000"
    title="审批模板详情"
    @cancel="emit('update:open', false)"
  >
    <Spin :spinning="loading">
      <template v-if="template">
        <Tabs v-model:active-key="activeTab" size="small">
          <Tabs.TabPane key="basic" tab="基础信息">
            <Descriptions bordered :column="2" size="small">
              <DescriptionsItem label="模板名称">
                {{ template.displayName }}
              </DescriptionsItem>
              <DescriptionsItem label="模板编码">
                {{ template.name }}
              </DescriptionsItem>
              <DescriptionsItem label="分类">
                {{ template.category || '-' }}
              </DescriptionsItem>
              <DescriptionsItem label="状态">
                <Tag :color="statusColor(template.isPublished)">
                  {{ statusText(template.isPublished) }}
                </Tag>
              </DescriptionsItem>
              <DescriptionsItem label="业务键提示">
                {{ template.businessKeyHint || '-' }}
              </DescriptionsItem>
              <DescriptionsItem label="表单">
                {{ template.formDefinitionId ? '已绑定' : '未绑定' }}
              </DescriptionsItem>
              <DescriptionsItem label="说明" :span="2">
                {{ template.description || '-' }}
              </DescriptionsItem>
            </Descriptions>
          </Tabs.TabPane>

          <Tabs.TabPane key="form" tab="表单设计">
            <VxeGrid
              :columns="fieldColumns"
              :data="fields"
              :pager-config="{ enabled: false }"
              border
              size="small"
            >
              <template #permission="{ row }">
                {{ permissionText(row.permission) }}
              </template>
              <template #isRequired="{ row }">
                {{ row.isRequired ? '是' : '否' }}
              </template>
            </VxeGrid>
            <div v-if="fields.length === 0" class="empty-tip">
              该模板未绑定表单或表单没有字段
            </div>
          </Tabs.TabPane>

          <Tabs.TabPane key="flow" tab="流程设计">
            <div class="flow-view">
              <ApprovalFlowDesigner
                :flow-json="template.flowDefinitionJson"
                embedded
                readonly
              />
            </div>
          </Tabs.TabPane>
        </Tabs>
      </template>
    </Spin>
  </Modal>
</template>

<style scoped>
.flow-view {
  height: 560px;
  overflow: hidden;
  background: #f5f6f8;
}

.empty-tip {
  padding: 24px 0;
  color: #909399;
  text-align: center;
}
</style>
