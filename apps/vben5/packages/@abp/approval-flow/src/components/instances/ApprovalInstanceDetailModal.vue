<script setup lang="ts">
import type {
  ApprovalInstanceDetailDto,
  ApprovalParticipantDto,
  ApprovalPendingTaskDto,
  FormFieldDto,
} from '../../types/approval-flow';

import { computed, ref, watch } from 'vue';

import { $t } from '@vben/locales';

import {
  Button,
  Descriptions,
  DescriptionsItem,
  Empty,
  Input,
  message,
  Modal,
  Radio,
  RadioGroup,
  Select,
  Space,
  Spin,
  Tabs,
  Tag,
  Timeline,
} from 'ant-design-vue';

import { useApprovalFormsApi } from '../../api/useApprovalFormsApi';
import { useApprovalInstancesApi } from '../../api/useApprovalInstancesApi';
import { useApprovalParticipantsApi } from '../../api/useApprovalParticipantsApi';
import { formatDuration } from './approval-table-utils';
import ApprovalFlowViewer from './ApprovalFlowViewer.vue';

const props = defineProps<{
  instanceId?: null | string;
  open: boolean;
  task?: ApprovalPendingTaskDto | null;
}>();

const emit = defineEmits<{
  decided: [];
  'update:open': [boolean];
}>();

const instancesApi = useApprovalInstancesApi();
const formsApi = useApprovalFormsApi();
const participantsApi = useApprovalParticipantsApi();

const loading = ref(false);
const submitting = ref(false);
const detail = ref<ApprovalInstanceDetailDto | null>(null);
const activeTab = ref('action');
const formFields = ref<FormFieldDto[]>([]);
const formValues = ref<Record<string, unknown>>({});
const actionKey = ref('Approve');
const comment = ref('');
const participantId = ref<string | undefined>(undefined);
const participants = ref<Array<{ label: string; value: string }>>([]);

const actionLabels: Record<string, string> = {
  AddApproval: '加批',
  Approve: '同意',
  Reject: '不同意',
  Return: '退回',
  Transfer: '转办',
};

const actionOptions = computed(() => {
  const allowed = props.task?.allowedActions ?? [];
  const keys = allowed.filter((key) => actionLabels[key]);
  if (!keys.includes('Approve')) {
    keys.unshift('Approve');
  }
  if (!keys.includes('Reject')) {
    keys.push('Reject');
  }
  return [...new Set(keys)].map((key) => ({
    label: actionLabels[key] ?? key,
    value: key,
  }));
});

const needsParticipant = computed(
  () => actionKey.value === 'Transfer' || actionKey.value === 'AddApproval',
);

watch(
  () => [props.open, props.instanceId, props.task] as const,
  async ([open, instanceId]) => {
    if (!open || !instanceId) {
      return;
    }

    loading.value = true;
    detail.value = null;
    formFields.value = [];
    formValues.value = {};
    actionKey.value = 'Approve';
    comment.value = '';
    participantId.value = undefined;
    activeTab.value = props.task ? 'action' : 'form';

    try {
      detail.value = await instancesApi.getDetailApi(instanceId);
      formValues.value = parseFormData(detail.value.instance.formDataJson);

      const formDefinitionId = detail.value.formDefinitionId;
      if (formDefinitionId) {
        try {
          const form = await formsApi.getApi(formDefinitionId);
          formFields.value = form.fields ?? [];
        } catch {
          formFields.value = [];
        }
      }

      if (props.task) {
        await loadParticipants();
      }
    } catch {
      message.error('审批详情加载失败');
    } finally {
      loading.value = false;
    }
  },
  { immediate: true },
);

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

async function loadParticipants() {
  if (participants.value.length > 0) {
    return;
  }
  try {
    const users = await participantsApi.searchUsersAsync();
    participants.value = users.map((item) => ({
      label: item.displayName ?? item.id,
      value: item.id,
    }));
  } catch {
    participants.value = [];
  }
}

function selectedParticipant(): ApprovalParticipantDto | null {
  if (!participantId.value) {
    return null;
  }
  return {
    displayName:
      participants.value.find((item) => item.value === participantId.value)
        ?.label ?? participantId.value,
    id: participantId.value,
    provider: 'identity',
    type: 'User',
  };
}

async function submit() {
  const task = props.task;
  if (!task) {
    return;
  }
  if (needsParticipant.value && !participantId.value) {
    message.warning(
      actionKey.value === 'Transfer' ? '请选择转办人' : '请选择加批人员',
    );
    return;
  }

  submitting.value = true;
  try {
    switch (actionKey.value) {
      case 'AddApproval': {
        await instancesApi.addApprovalApi(task.instanceId, {
          approver: selectedParticipant()!,
          comment: comment.value || null,
          taskId: task.taskId,
          title: null,
        });

        break;
      }
      case 'Return': {
        await instancesApi.returnApi(task.instanceId, {
          comment: comment.value || null,
          taskId: task.taskId,
        });

        break;
      }
      case 'Transfer': {
        await instancesApi.transferApi(task.instanceId, {
          comment: comment.value || null,
          taskId: task.taskId,
          transferee: selectedParticipant()!,
        });

        break;
      }
      default: {
        await instancesApi.decideApi(task.instanceId, {
          actionKey: actionKey.value,
          comment: comment.value || null,
          dataJson:
            detail.value?.instance.formDataJson ??
            JSON.stringify(formValues.value ?? {}),
          taskId: task.taskId,
        });
      }
    }

    message.success('审批已提交');
    emit('decided');
    emit('update:open', false);
  } catch {
    message.error('审批提交失败');
  } finally {
    submitting.value = false;
  }
}

function cancel() {
  emit('update:open', false);
}

function fieldValue(field: FormFieldDto): string {
  const value = formValues.value[field.key];
  if (value === null || value === undefined || value === '') {
    return '-';
  }
  if (Array.isArray(value)) {
    return value.join('、');
  }
  if (typeof value === 'object') {
    return JSON.stringify(value);
  }
  return String(value);
}

function statusColor(status?: string): string {
  switch (status) {
    case 'Approved': {
      return 'green';
    }
    case 'Rejected': {
      return 'red';
    }
    case 'Running': {
      return 'blue';
    }
    default: {
      return 'default';
    }
  }
}

function statusText(status?: string): string {
  switch (status) {
    case 'Approved': {
      return '已通过';
    }
    case 'Cancelled': {
      return '已取消';
    }
    case 'Rejected': {
      return '已拒绝';
    }
    case 'Returned': {
      return '已退回';
    }
    case 'Running': {
      return '审批中';
    }
    case 'Withdrawn': {
      return '已撤回';
    }
    default: {
      return status ?? '-';
    }
  }
}

function recordColor(effect?: string): string {
  switch (effect) {
    case 'Approve': {
      return 'green';
    }
    case 'Reject': {
      return 'red';
    }
    default: {
      return 'gray';
    }
  }
}

function actionText(actionKey?: null | string): string {
  if (!actionKey) {
    return '-';
  }
  return $t(`ElsaNextApprovalFlow.ApprovalRecordAction:${actionKey}`);
}

function format(value?: null | string): string {
  return value ? new Date(value).toLocaleString() : '-';
}
</script>

<template>
  <Modal
    :footer="null"
    :open="open"
    :width="1120"
    title="审批详情"
    @cancel="emit('update:open', false)"
  >
    <Spin :spinning="loading">
      <Tabs v-if="detail" v-model:active-key="activeTab" size="small">
        <Tabs.TabPane v-if="task" key="action" tab="审批">
          <div class="approval-layout">
            <section class="form-side">
              <div class="side-title">流程表单</div>
              <Descriptions :column="1" bordered size="small">
                <DescriptionsItem
                  v-for="field in formFields"
                  :key="field.id"
                  :label="field.label"
                >
                  {{ fieldValue(field) }}
                </DescriptionsItem>
              </Descriptions>
              <pre v-if="formFields.length === 0" class="form-json">{{
                detail.instance.formDataJson || '暂无表单内容'
              }}</pre>
            </section>

            <section class="action-side">
              <div class="side-title">审批操作</div>

              <div class="action-block">
                <RadioGroup v-model:value="actionKey" class="action-radio">
                  <Radio
                    v-for="item in actionOptions"
                    :key="item.value"
                    :value="item.value"
                  >
                    {{ item.label }}
                  </Radio>
                </RadioGroup>
              </div>

              <div v-if="needsParticipant" class="action-block">
                <Select
                  v-model:value="participantId"
                  :options="participants"
                  :placeholder="
                    actionKey === 'Transfer' ? '请选择转办人' : '请选择加批人员'
                  "
                  show-search
                  style="width: 100%"
                />
              </div>

              <div class="action-block">
                <Input.TextArea
                  v-model:value="comment"
                  :rows="5"
                  placeholder="审批意见"
                />
              </div>

              <div class="action-footer">
                <Space>
                  <Button :disabled="submitting" @click="cancel">取消</Button>
                  <Button :loading="submitting" type="primary" @click="submit">
                    提交
                  </Button>
                </Space>
              </div>
            </section>
          </div>
        </Tabs.TabPane>

        <Tabs.TabPane key="form" tab="表单">
          <section class="form-side">
            <Descriptions :column="1" bordered size="small">
              <DescriptionsItem
                v-for="field in formFields"
                :key="field.id"
                :label="field.label"
              >
                {{ fieldValue(field) }}
              </DescriptionsItem>
            </Descriptions>
            <Empty v-if="formFields.length === 0" description="暂无表单定义" />
            <pre
              v-if="formFields.length === 0 && detail.instance.formDataJson"
              class="form-json"
              >{{ detail.instance.formDataJson }}</pre>
          </section>
        </Tabs.TabPane>

        <Tabs.TabPane key="flow" tab="流程图">
          <ApprovalFlowViewer
            :flow-json="detail.flowDefinitionJson"
            :instance-status="detail.instance.status"
            :node-tasks="detail.nodeTasks"
            :records="detail.records"
          />
        </Tabs.TabPane>

        <Tabs.TabPane key="detail" tab="详情">
          <Descriptions bordered :column="2" size="small">
            <DescriptionsItem label="标题">
              {{ detail.instance.title }}
            </DescriptionsItem>
            <DescriptionsItem label="模板">
              {{ detail.instance.templateName }}
            </DescriptionsItem>
            <DescriptionsItem label="状态">
              <Tag :color="statusColor(detail.instance.status)">
                {{ statusText(detail.instance.status) }}
              </Tag>
            </DescriptionsItem>
            <DescriptionsItem label="发起人">
              {{ detail.instance.initiatorDisplayName || '-' }}
            </DescriptionsItem>
            <DescriptionsItem label="发起时间">
              {{ format(detail.instance.startedAt) }}
            </DescriptionsItem>
            <DescriptionsItem label="完成时间">
              {{ format(detail.instance.finishedAt) }}
            </DescriptionsItem>
          </Descriptions>
        </Tabs.TabPane>

        <Tabs.TabPane key="records" tab="执行记录">
          <Timeline>
            <Timeline.Item
              v-for="record in detail.records"
              :key="record.id"
              :color="recordColor(record.effect)"
            >
              <div class="text-sm">
                <span class="font-medium">{{
                  actionText(record.actionKey)
                }}</span>
                <span class="ml-2 text-gray-500">
                  {{ record.actorDisplayName || '系统' }}
                </span>
                <span class="ml-2 text-xs text-gray-400">
                  {{ format(record.occurredAt) }}
                </span>
              </div>
              <div
                v-if="record.firstViewedAt || record.completedAt"
                class="mt-1 text-xs text-gray-500"
              >
                <span v-if="record.firstViewedAt">
                  首次查看：{{ format(record.firstViewedAt) }}
                </span>
                <span v-if="record.completedAt" class="ml-3">
                  办理时间：{{ format(record.completedAt) }}
                </span>
                <span
                  v-if="record.firstViewedAt && record.completedAt"
                  class="ml-3"
                >
                  办理时长：{{
                    formatDuration(record.firstViewedAt, record.completedAt)
                  }}
                </span>
              </div>
              <div v-if="record.comment" class="mt-1 text-xs text-gray-500">
                审批意见：{{ record.comment }}
              </div>
            </Timeline.Item>
          </Timeline>
          <Empty
            v-if="detail.records.length === 0"
            description="暂无执行记录"
          />
        </Tabs.TabPane>
      </Tabs>
    </Spin>
  </Modal>
</template>

<style scoped>
.approval-layout {
  display: grid;
  grid-template-columns: minmax(0, 1fr) 360px;
  gap: 16px;
  min-height: 520px;
}

.form-side,
.action-side {
  padding: 14px;
  background: #fff;
  border: 1px solid #ebedf0;
  border-radius: 8px;
}

.side-title {
  margin-bottom: 12px;
  font-size: 15px;
  font-weight: 600;
  color: #303133;
}

.form-json {
  max-height: 420px;
  padding: 12px;
  overflow: auto;
  font-size: 12px;
  white-space: pre-wrap;
  background: #f7f8fa;
  border-radius: 6px;
}

.action-block {
  margin-bottom: 16px;
}

.action-radio {
  display: flex;
  flex-wrap: wrap;
  gap: 8px 12px;
}

.action-radio :deep(.ant-radio-wrapper) {
  margin-inline-end: 0;
}

.action-footer {
  display: flex;
  justify-content: flex-end;
  margin-top: 12px;
}

@media (max-width: 900px) {
  .approval-layout {
    grid-template-columns: 1fr;
  }
}
</style>
