<script setup lang="ts">
import type { ParticipantRef } from '../../types/approval-flow';
import type { AntFlowNode } from './types';

import { computed, reactive, ref, watch } from 'vue';

import { $t } from '@vben/locales';

import {
  Alert,
  Button,
  Checkbox,
  CheckboxGroup,
  Drawer,
  Input,
  message,
  Modal,
  Radio,
  RadioGroup,
  Select,
  Space,
  Tabs,
  Tag,
} from 'ant-design-vue';

import ButtonPerm from './ButtonPerm.vue';
import { NodeType } from './types';

interface AntFlowAssigneeLike {
  displayName?: null | string;
  id?: null | string;
  name?: null | string;
  provider?: null | string;
  targetId?: null | string;
  type?: null | number | string;
  userId?: null | string;
}

interface NodePropertyLike {
  assigneeList?: AntFlowAssigneeLike[];
  assigneeType?: null | number;
  buttons?: Record<string, number[]>;
  directorLevel?: null | number;
  fieldPrems?: unknown[];
  noHeaderAction?: null | number | string;
  onEmpty?: null | string;
  ruleKind?: null | string;
  signType?: null | number | string;
}

const props = withDefaults(
  defineProps<{
    modelValue?: boolean;
    node?: AntFlowNode | null;
    open?: boolean;
    participants?: ParticipantRef[];
    readonly?: boolean;
  }>(),
  {
    modelValue: false,
    node: null,
    open: false,
    participants: () => [],
    readonly: false,
  },
);

const emit = defineEmits<{
  'update:modelValue': [boolean];
  'update:node': [AntFlowNode];
  'update:open': [boolean];
}>();

const visible = computed(() => props.open || props.modelValue);
const activeTab = ref('approver');
const pickerOpen = ref(false);
const pickerSelected = ref<string[]>([]);

const signTypeOptions = [
  { label: $t('antflow.approvalNode.orSign'), value: 1 },
  { label: $t('antflow.approvalNode.countersign'), value: 2 },
  { label: $t('antflow.approvalNode.sequentialCountersign'), value: 3 },
];

const noHeaderActionOptions = [
  { label: $t('antflow.approvalNode.noHeaderActionNone'), value: 0 },
  { label: $t('antflow.approvalNode.noHeaderActionSkip'), value: 1 },
  { label: $t('antflow.approvalNode.noHeaderActionAdmin'), value: 2 },
];

const directorLevelOptions = Array.from({ length: 5 }, (_, index) => ({
  label:
    index === 0
      ? $t('antflow.approvalNode.directSupervisor')
      : $t('antflow.approvalNode.levelSupervisor').replace(
          '{0}',
          String(index + 1),
        ),
  value: index + 1,
}));

const draft = reactive({
  assigneeList: [] as AntFlowAssigneeLike[],
  assigneeType: 1,
  buttons: {} as Record<string, number[]>,
  directorLevel: 1,
  fieldPrems: [] as unknown[],
  noHeaderAction: 0,
  nodeName: '',
  nodeType: 0,
  signType: 1,
});

const isApprove = computed(
  () =>
    draft.nodeType === NodeType.ApproveNode ||
    draft.nodeType === NodeType.ParallelApproveNode,
);
const isCopy = computed(() => draft.nodeType === NodeType.CopyNode);
const drawerTitle = computed(() => {
  if (isCopy.value) {
    return $t('antflow.approvalNode.ccPerson');
  }
  if (draft.nodeType === NodeType.ParallelApproveNode) {
    return $t('antflow.approvalNode.parallelApprover');
  }
  return $t('antflow.approvalNode.approver');
});

const assigneeTypeOptions = computed(() => {
  if (isCopy.value) {
    return [
      { label: $t('antflow.approvalNode.assigneeMember'), value: 1 },
      { label: $t('antflow.approvalNode.assigneeRole'), value: 2 },
    ];
  }
  return [
    { label: $t('antflow.approvalNode.assigneeMember'), value: 1 },
    { label: $t('antflow.approvalNode.assigneeRole'), value: 2 },
    { label: $t('antflow.approvalNode.assigneeLeader'), value: 3 },
    { label: $t('antflow.approvalNode.assigneeInitiator'), value: 5 },
    { label: $t('antflow.approvalNode.assigneeLevel'), value: 6 },
    { label: $t('antflow.approvalNode.assigneeChain'), value: 7 },
    { label: $t('antflow.approvalNode.assigneeSelfSelect'), value: 8 },
  ];
});

const visibleSignTypeOptions = computed(() =>
  signTypeOptions.filter(
    (item) => item.value !== 3 || draft.assigneeType === 1,
  ),
);

const pickerOptions = computed(() => {
  const roleMode = draft.assigneeType === 2;
  return props.participants
    .filter((item) =>
      roleMode
        ? item.provider === 'role' || item.type === 'Group'
        : item.type === 'User',
    )
    .map((item) => ({
      label: item.displayName || item.id,
      value: item.id,
    }));
});

const personLabel = computed(() =>
  draft.assigneeType === 2
    ? $t('antflow.approvalNode.addRole')
    : $t('antflow.approvalNode.addPerson'),
);

const tabs = computed(() => {
  const items = [
    {
      key: 'approver',
      label: isCopy.value
        ? $t('antflow.approvalNode.ccSetting')
        : $t('antflow.approvalNode.approverSetting'),
    },
  ];
  if (isApprove.value) {
    items.push({
      key: 'button',
      label: $t('antflow.approvalNode.buttonPermission'),
    });
  }
  return items;
});

watch(
  () => [props.open, props.modelValue, props.node] as const,
  () => {
    if (visible.value) {
      resetDraft();
    }
  },
  { immediate: true },
);

function propertyOf(node: AntFlowNode | null | undefined): NodePropertyLike {
  return (node?.nodeProperty ?? {}) as unknown as NodePropertyLike;
}

function deepClone<T>(value: T): T {
  // eslint-disable-next-line unicorn/prefer-structured-clone
  return JSON.parse(JSON.stringify(value)) as T;
}

function assigneeId(assignee: AntFlowAssigneeLike): string {
  return (
    assignee.targetId ?? assignee.userId ?? assignee.id ?? assignee.name ?? ''
  );
}

function assigneeLabel(assignee: AntFlowAssigneeLike): string {
  return assignee.name ?? assignee.displayName ?? assigneeId(assignee);
}

function toNumber(value: unknown, fallback: number): number {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
}

function resetDraft() {
  const node = props.node;
  const property = propertyOf(node);

  draft.nodeType = node?.nodeType ?? 0;
  draft.nodeName = node?.nodeName ?? '';
  draft.assigneeType = toNumber(property.assigneeType, 1);
  draft.assigneeList = deepClone(
    Array.isArray(property.assigneeList) ? property.assigneeList : [],
  );
  draft.signType = toNumber(property.signType, 1);
  draft.noHeaderAction = toNumber(property.noHeaderAction, 0);
  draft.directorLevel = toNumber(property.directorLevel, 1);
  draft.buttons = {
    ...deepClone(property.buttons ?? {}),
    approvalPage: Array.isArray(property.buttons?.approvalPage)
      ? [...property.buttons.approvalPage]
      : [3, 4],
  };
  draft.fieldPrems = deepClone(property.fieldPrems ?? []);
  activeTab.value = 'approver';
}

function changeAssigneeType() {
  draft.assigneeList = [];
  draft.signType = 1;
  draft.noHeaderAction = 0;
}

function removeAssignee(id: string) {
  draft.assigneeList = draft.assigneeList.filter(
    (item) => assigneeId(item) !== id,
  );
}

function openPicker() {
  pickerSelected.value = draft.assigneeList
    .map((item) => assigneeId(item))
    .filter(Boolean);
  pickerOpen.value = true;
}

function confirmPicker() {
  draft.assigneeList = pickerSelected.value.map((id) => {
    const participant = props.participants.find((item) => item.id === id);
    const isRole =
      draft.assigneeType === 2 ||
      participant?.provider === 'role' ||
      participant?.type === 'Group';
    return {
      displayName: participant?.displayName ?? id,
      id,
      name: participant?.displayName ?? id,
      provider: participant?.provider,
      targetId: id,
      type: isRole ? 2 : 1,
    };
  });
  pickerOpen.value = false;
}

function generateDisplayName(): string {
  const names = draft.assigneeList
    .map((item) => item.name ?? item.displayName)
    .filter(Boolean);

  if ([1, 2].includes(draft.assigneeType) && names.length > 0) {
    if (draft.signType === 2) {
      return $t('antflow.approvalNode.signSummary', [
        String(names.length),
        names.join(','),
      ]);
    }
    if (draft.signType === 3) {
      return $t('antflow.approvalNode.sequentialSignSummary', [
        String(names.length),
        names.join(','),
      ]);
    }
    return names.join(',');
  }
  if (draft.assigneeType === 6) {
    return $t('antflow.approvalNode.levelSummary', [
      String(draft.directorLevel),
    ]);
  }
  return (
    assigneeTypeOptions.value.find((item) => item.value === draft.assigneeType)
      ?.label ?? draft.nodeName
  );
}

function validate(): boolean {
  if (!draft.nodeName.trim()) {
    message.warning($t('antflow.approvalNode.nodeNameRequired'));
    return false;
  }
  if ([1, 2].includes(draft.assigneeType) && draft.assigneeList.length === 0) {
    message.warning(
      draft.assigneeType === 2
        ? $t('antflow.approvalNode.selectRole')
        : $t('antflow.approvalNode.selectApprover'),
    );
    return false;
  }
  return true;
}

function close() {
  emit('update:open', false);
  emit('update:modelValue', false);
}

function onButtonsUpdate(value: { approvalPage?: number[] }) {
  draft.buttons = { ...draft.buttons, ...value };
}
function confirm() {
  const node = props.node;
  if (!node || !validate()) {
    return;
  }

  const cloned = deepClone(node);
  const previous = propertyOf(cloned);
  const property: NodePropertyLike = {
    ...previous,
    assigneeList:
      draft.assigneeType === 1 || draft.assigneeType === 2
        ? draft.assigneeList.map((item) => ({ ...item }))
        : [],
    assigneeType: draft.assigneeType,
    buttons: { ...draft.buttons },
    directorLevel: draft.directorLevel,
    fieldPrems: [...draft.fieldPrems],
  };

  delete (property as Record<string, unknown>).ruleKind;

  if (isApprove.value) {
    property.signType = draft.signType;
    property.noHeaderAction = draft.noHeaderAction;
    property.onEmpty =
      draft.noHeaderAction === 2 ? 'EscalateToManager' : 'Skip';
  }

  (cloned as unknown as { nodeProperty: NodePropertyLike }).nodeProperty =
    property;
  cloned.nodeName = draft.nodeName.trim();
  cloned.nodeDisplayName = generateDisplayName();
  cloned.error = false;

  emit('update:node', cloned);
  close();
}
</script>

<template>
  <Drawer
    :open="visible"
    placement="right"
    :title="drawerTitle"
    :width="600"
    @close="close"
  >
    <Tabs v-model:active-key="activeTab" size="small">
      <Tabs.TabPane v-for="tab in tabs" :key="tab.key" :tab="tab.label" />
    </Tabs>

    <section v-if="activeTab === 'approver'" class="approver-panel">
      <div class="form-item">
        <label class="form-label">{{
          $t('antflow.approvalNode.nodeName')
        }}</label>
        <Input
          v-model:value="draft.nodeName"
          :disabled="readonly"
          :maxlength="50"
          :placeholder="$t('antflow.approvalNode.nodeNamePlaceholder')"
        />
      </div>

      <RadioGroup
        v-model:value="draft.assigneeType"
        class="assignee-types"
        :disabled="readonly"
        @change="changeAssigneeType"
      >
        <Radio
          v-for="item in assigneeTypeOptions"
          :key="item.value"
          :value="item.value"
        >
          {{ item.label }}
        </Radio>
      </RadioGroup>

      <div
        v-if="[1, 2].includes(draft.assigneeType)"
        class="person-select-wrap"
      >
        <Button v-if="!readonly" type="primary" ghost @click="openPicker">
          {{ personLabel }}
        </Button>
        <div class="person-tags">
          <Tag
            v-for="person in draft.assigneeList"
            :key="assigneeId(person)"
            :closable="!readonly"
            color="blue"
            @close="removeAssignee(assigneeId(person))"
          >
            {{ assigneeLabel(person) }}
          </Tag>
        </div>
      </div>

      <Alert
        v-else-if="draft.assigneeType === 3"
        :description="$t('antflow.approvalNode.directLeaderDesc')"
        show-icon
        :title="$t('antflow.approvalNode.directLeaderTitle')"
        type="warning"
      />
      <Alert
        v-else-if="draft.assigneeType === 5"
        :description="$t('antflow.approvalNode.initiatorDesc')"
        show-icon
        :title="$t('antflow.approvalNode.initiatorTitle')"
        type="warning"
      />
      <div v-else-if="draft.assigneeType === 6" class="director-line">
        <span>{{ $t('antflow.approvalNode.initiatorPrefix') }}</span>
        <Select
          v-model:value="draft.directorLevel"
          :disabled="readonly"
          :options="directorLevelOptions"
          style="width: 220px"
        />
        <p class="director-tip">{{ $t('antflow.approvalNode.directorTip') }}</p>
      </div>
      <Alert
        v-else-if="draft.assigneeType === 7"
        :description="$t('antflow.approvalNode.chainDesc')"
        show-icon
        :title="$t('antflow.approvalNode.chainTitle')"
        type="warning"
      />
      <Alert
        v-else-if="draft.assigneeType === 8"
        :description="$t('antflow.approvalNode.selfSelectDesc')"
        show-icon
        :title="$t('antflow.approvalNode.selfSelectTitle')"
        type="warning"
      />

      <template v-if="isApprove">
        <div class="section-block">
          <h3>✍{{ $t('antflow.approvalNode.multiApprovalMode') }}</h3>
          <RadioGroup
            v-model:value="draft.signType"
            class="radio-stack"
            :disabled="readonly"
          >
            <Radio
              v-for="item in visibleSignTypeOptions"
              :key="item.value"
              :value="item.value"
            >
              {{ item.label }}
            </Radio>
          </RadioGroup>
        </div>

        <div class="section-block">
          <h3>✍{{ $t('antflow.approvalNode.emptyApprover') }}</h3>
          <RadioGroup
            v-model:value="draft.noHeaderAction"
            class="radio-stack"
            :disabled="readonly"
          >
            <Radio
              v-for="item in noHeaderActionOptions"
              :key="item.value"
              :value="item.value"
            >
              {{ item.label }}
            </Radio>
          </RadioGroup>
        </div>
      </template>
    </section>

    <section v-else-if="activeTab === 'button'" class="section-block">
      <ButtonPerm
        :model-value="draft.buttons"
        :readonly="readonly"
        @update:model-value="onButtonsUpdate"
      />
    </section>

    <template #footer>
      <div class="drawer-footer">
        <Space>
          <Button @click="close">
            {{
              readonly
                ? $t('antflow.approvalNode.close')
                : $t('antflow.approvalNode.cancel')
            }}
          </Button>
          <Button v-if="!readonly" type="primary" @click="confirm">
            {{ $t('antflow.approvalNode.confirm') }}
          </Button>
        </Space>
      </div>
    </template>
  </Drawer>

  <Modal
    v-model:open="pickerOpen"
    :title="
      draft.assigneeType === 2
        ? $t('antflow.approvalNode.selectRoleTitle')
        : $t('antflow.approvalNode.selectPersonTitle')
    "
    @ok="confirmPicker"
  >
    <CheckboxGroup v-model:value="pickerSelected" class="picker-list">
      <Checkbox
        v-for="item in pickerOptions"
        :key="item.value"
        :value="item.value"
      >
        {{ item.label }}
      </Checkbox>
    </CheckboxGroup>
    <div v-if="pickerOptions.length === 0" class="muted">
      {{ $t('antflow.common.noData') }}
    </div>
  </Modal>
</template>

<style scoped>
.approver-panel {
  padding-top: 14px;
}

.form-item {
  display: grid;
  grid-template-columns: 90px minmax(0, 1fr);
  gap: 12px;
  align-items: center;
  margin-bottom: 16px;
}

.form-label {
  font-size: 14px;
  color: #4f5b6f;
  text-align: right;
}

.assignee-types {
  display: grid;
  grid-template-columns: repeat(3, minmax(160px, 1fr));
  gap: 10px;
  margin-bottom: 16px;
}

.assignee-types :deep(.ant-radio-wrapper) {
  margin-inline-end: 0;
}

.person-select-wrap {
  margin-bottom: 12px;
}

.person-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-top: 10px;
}

.section-block {
  padding: 14px 0 16px;
  margin-top: 8px;
  border-top: 1px solid #ebedf0;
}

.section-block h3 {
  margin: 0 0 14px;
  font-size: 16px;
  font-weight: 500;
  color: #303133;
}

.radio-stack {
  display: flex;
  flex-direction: column;
  gap: 8px;
  align-items: flex-start;
}

.radio-stack :deep(.ant-radio-wrapper) {
  margin-inline-end: 0;
}

.director-line {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  align-items: center;
  margin-bottom: 12px;
}

.director-tip {
  width: 100%;
  margin: 0;
  font-size: 12px;
  color: #e6a23c;
}

.drawer-footer {
  display: flex;
  justify-content: flex-end;
  width: 100%;
}

.picker-list {
  display: flex;
  flex-direction: column;
  gap: 8px;
  max-height: 420px;
  overflow: auto;
}

.muted {
  font-size: 12px;
  color: #909399;
}

@media (max-width: 640px) {
  .assignee-types {
    grid-template-columns: repeat(2, minmax(120px, 1fr));
  }
}
</style>
