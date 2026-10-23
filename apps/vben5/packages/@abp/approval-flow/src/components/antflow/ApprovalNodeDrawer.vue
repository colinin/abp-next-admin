<script setup lang="ts">
import type { ParticipantRef } from '../../types/approval-flow';
import type { AntFlowNode } from './types';

import { computed, reactive, ref, watch } from 'vue';

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
  { label: '或签（只需一名审批人同意或拒绝即可）', value: 1 },
  { label: '会签（需所有审批人同意，不限顺序）', value: 2 },
  { label: '顺序会签（需要所有审批人同意，根据前端传入的顺序）', value: 3 },
];

const noHeaderActionOptions = [
  { label: '不允许发起', value: 0 },
  { label: '跳过', value: 1 },
  { label: '转交给审核管理员', value: 2 },
];

const directorLevelOptions = Array.from({ length: 5 }, (_, index) => ({
  label: index === 0 ? '直接主管' : `第${index + 1}级主管`,
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
    return '抄送人';
  }
  if (draft.nodeType === NodeType.ParallelApproveNode) {
    return '并行审批人';
  }
  return '审批人';
});

const assigneeTypeOptions = computed(() => {
  if (isCopy.value) {
    return [
      { label: '指定成员', value: 1 },
      { label: '指定角色', value: 2 },
    ];
  }
  return [
    { label: '指定成员', value: 1 },
    { label: '指定角色', value: 2 },
    { label: '直属领导', value: 3 },
    { label: '发起人自己', value: 5 },
    { label: '指定层级', value: 6 },
    { label: '层层审批', value: 7 },
    { label: '发起人自选审批人', value: 8 },
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
  draft.assigneeType === 2 ? '添加/修改角色' : '添加/修改人员',
);

const tabs = computed(() => {
  const items = [
    { key: 'approver', label: isCopy.value ? '抄送人设置' : '审批人设置' },
  ];
  if (isApprove.value) {
    items.push({ key: 'button', label: '按钮权限设置' });
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
      return `${names.length}人(${names.join(',')})会签`;
    }
    if (draft.signType === 3) {
      return `${names.length}人(${names.join(',')})顺序会签`;
    }
    return names.join(',');
  }
  if (draft.assigneeType === 6) {
    return `第 ${draft.directorLevel} 级主管`;
  }
  return (
    assigneeTypeOptions.value.find((item) => item.value === draft.assigneeType)
      ?.label ?? draft.nodeName
  );
}

function validate(): boolean {
  if (!draft.nodeName.trim()) {
    message.warning('请输入节点名称');
    return false;
  }
  if ([1, 2].includes(draft.assigneeType) && draft.assigneeList.length === 0) {
    message.warning(
      draft.assigneeType === 2 ? '请选择角色' : '请选择审批人/抄送人',
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
        <label class="form-label">节点名称</label>
        <Input
          v-model:value="draft.nodeName"
          :disabled="readonly"
          :maxlength="50"
          placeholder="请输入节点名称"
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
        description="系统将自动选择流程发起人的直属领导作为审批人"
        show-icon
        title="直属领导"
        type="warning"
      />
      <Alert
        v-else-if="draft.assigneeType === 5"
        description="系统将自动选择流程发起人自己作为审批人"
        show-icon
        title="发起人自己"
        type="warning"
      />
      <div v-else-if="draft.assigneeType === 6" class="director-line">
        <span>发起人的：</span>
        <Select
          v-model:value="draft.directorLevel"
          :disabled="readonly"
          :options="directorLevelOptions"
          style="width: 220px"
        />
        <p class="director-tip">找不到主管时，由直属领导代审批</p>
      </div>
      <Alert
        v-else-if="draft.assigneeType === 7"
        description="按照组织架构的上下级关系，从低到高、一级一级往上审批，每一级的直属领导都要依次审批"
        show-icon
        title="层层审批"
        type="warning"
      />
      <Alert
        v-else-if="draft.assigneeType === 8"
        description="用户可以在流程发起时选择该节点的审批人"
        show-icon
        title="发起人自选审批人"
        type="warning"
      />

      <template v-if="isApprove">
        <div class="section-block">
          <h3>✍多人审批时采用的审批方式</h3>
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
          <h3>✍审批人为空时</h3>
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
          <Button @click="close">{{ readonly ? '关闭' : '取消' }}</Button>
          <Button v-if="!readonly" type="primary" @click="confirm">确定</Button>
        </Space>
      </div>
    </template>
  </Drawer>

  <Modal
    v-model:open="pickerOpen"
    :title="draft.assigneeType === 2 ? '选择角色' : '选择人员'"
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
    <div v-if="pickerOptions.length === 0" class="muted">暂无可选数据</div>
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
