<script setup lang="ts">
import type { VxeGridProps } from 'vxe-table';

import type {
  ApprovalTemplateDto,
  FormFieldDto,
} from '../../types/approval-flow';

import { ref, watch } from 'vue';

import { $t } from '@vben/locales';

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
  {
    field: 'label',
    minWidth: 200,
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Form:FieldName'),
  },
  {
    field: 'key',
    minWidth: 160,
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Form:FieldCode'),
  },
  {
    field: 'type',
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Form:Type'),
    width: 140,
  },
  {
    field: 'permission',
    slots: { default: 'permission' },
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Form:Permission'),
    width: 90,
  },
  {
    field: 'isRequired',
    slots: { default: 'isRequired' },
    title: $t('ElsaNextApprovalFlow.ApprovalFlow:Form:Required'),
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
      message.error(
        $t('ElsaNextApprovalFlow.ApprovalFlow:TemplateView:FormLoadFailed'),
      );
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
  return isPublished
    ? $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Published')
    : $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Draft');
}

function permissionText(permission: string): string {
  switch (permission) {
    case 'E': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Editable');
    }
    case 'H': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Hidden');
    }
    case 'R': {
      return $t('ElsaNextApprovalFlow.ApprovalFlow:Common:ReadOnly');
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
    :title="$t('ElsaNextApprovalFlow.ApprovalFlow:TemplateView:Title')"
    @cancel="emit('update:open', false)"
  >
    <Spin :spinning="loading">
      <template v-if="template">
        <Tabs v-model:active-key="activeTab" size="small">
          <Tabs.TabPane
            key="basic"
            :tab="$t('ElsaNextApprovalFlow.ApprovalFlow:TemplateView:Basic')"
          >
            <Descriptions bordered :column="2" size="small">
              <DescriptionsItem
                :label="
                  $t(
                    'ElsaNextApprovalFlow.ApprovalFlow:TemplateView:TemplateName',
                  )
                "
              >
                {{ template.displayName }}
              </DescriptionsItem>
              <DescriptionsItem
                :label="
                  $t(
                    'ElsaNextApprovalFlow.ApprovalFlow:TemplateView:TemplateCode',
                  )
                "
              >
                {{ template.name }}
              </DescriptionsItem>
              <DescriptionsItem
                :label="$t('ElsaNextApprovalFlow.ApprovalFlow:Common:Category')"
              >
                {{ template.category || '-' }}
              </DescriptionsItem>
              <DescriptionsItem
                :label="$t('ElsaNextApprovalFlow.ApprovalFlow:Common:Status')"
              >
                <Tag :color="statusColor(template.isPublished)">
                  {{ statusText(template.isPublished) }}
                </Tag>
              </DescriptionsItem>
              <DescriptionsItem
                :label="
                  $t(
                    'ElsaNextApprovalFlow.ApprovalFlow:TemplateView:BusinessKeyHint',
                  )
                "
              >
                {{ template.businessKeyHint || '-' }}
              </DescriptionsItem>
              <DescriptionsItem
                :label="$t('ElsaNextApprovalFlow.ApprovalFlow:Common:Form')"
              >
                {{
                  template.formDefinitionId
                    ? $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Bound')
                    : $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Unbound')
                }}
              </DescriptionsItem>
              <DescriptionsItem
                :label="
                  $t(
                    'ElsaNextApprovalFlow.ApprovalFlow:TemplateView:Description',
                  )
                "
                :span="2"
              >
                {{ template.description || '-' }}
              </DescriptionsItem>
            </Descriptions>
          </Tabs.TabPane>

          <Tabs.TabPane
            key="form"
            :tab="
              $t('ElsaNextApprovalFlow.ApprovalFlow:TemplateView:FormDesign')
            "
          >
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
                {{
                  row.isRequired
                    ? $t('ElsaNextApprovalFlow.ApprovalFlow:Common:Yes')
                    : $t('ElsaNextApprovalFlow.ApprovalFlow:Common:No')
                }}
              </template>
            </VxeGrid>
            <div v-if="fields.length === 0" class="empty-tip">
              {{ $t('ElsaNextApprovalFlow.ApprovalFlow:TemplateView:NoForm') }}
            </div>
          </Tabs.TabPane>

          <Tabs.TabPane
            key="flow"
            :tab="
              $t('ElsaNextApprovalFlow.ApprovalFlow:TemplateView:FlowDesign')
            "
          >
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
