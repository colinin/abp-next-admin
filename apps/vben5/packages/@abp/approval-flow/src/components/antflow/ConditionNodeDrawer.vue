<script setup lang="ts">
import type { AntFlowNode } from './types';

import { computed, reactive, toRaw, watch } from 'vue';

import {
  Alert,
  Button,
  Card,
  Drawer,
  Empty,
  Input,
  message,
  Select,
  Switch,
  Tag,
} from 'ant-design-vue';

/**
 * Optional form field descriptor used to offer the available field keys and
 * their value control type (input/number/select/date...).
 */
interface FormFieldOption {
  key: string;
  label?: null | string;
  type?: null | string;
}

interface AntFlowConditionItemLike {
  columnDbName?: null | string;
  columnType?: null | string;
  fixedDownBoxValue?: null | string;
  key?: null | string;
  label?: null | string;
  opt1?: null | string;
  opt2?: null | string;
  /** Either the numeric designer operator (1..6) or a symbolic operator. */
  optType?: null | string;
  type?: null | string;
  zdy1?: unknown;
  zdy2?: unknown;
}

interface AntFlowConditionGroupLike {
  conditionList?: AntFlowConditionItemLike[];
  condRelation?: boolean | number | string;
  groupId?: null | string;
  groupName?: null | string;
  sort?: null | number;
}

interface ConditionPropertyLike {
  conditionList?: AntFlowConditionItemLike[];
  condRelation?: boolean | number | string;
  /** Cached expression; cleared when the groups are edited. */
  expression?: null | string;
  groupConditions?: AntFlowConditionGroupLike[];
  groupRelation?: boolean | number | string;
  isDefault?: null | number | string;
}

interface ConditionRowDraft {
  key: string;
  operator: string;
  uid: string;
  value: string;
}

interface ConditionGroupDraft {
  condRelation: boolean;
  groupId: string;
  groupName: string;
  rows: ConditionRowDraft[];
  uid: string;
}

const props = withDefaults(
  defineProps<{
    formFields?: FormFieldOption[];
    /** AntFlow style visibility alias of `open`. */
    modelValue?: boolean;
    node?: AntFlowNode | null;
    open?: boolean;
    readonly?: boolean;
  }>(),
  {
    formFields: () => [],
    modelValue: false,
    node: null,
    open: false,
    readonly: false,
  },
);

const emit = defineEmits<{
  'update:modelValue': [boolean];
  'update:node': [AntFlowNode];
  'update:open': [boolean];
}>();

const visible = computed(() => props.open || props.modelValue);

const operatorOptions = [
  { label: '等于', value: '=' },
  { label: '不等于', value: '!=' },
  { label: '大于', value: '>' },
  { label: '大于等于', value: '>=' },
  { label: '小于', value: '<' },
  { label: '小于等于', value: '<=' },
];

const legacyOperatorMap: Record<string, string> = {
  '1': '<',
  '2': '>',
  '3': '<=',
  '4': '=',
  '5': '>=',
  '6': '=',
  '<': '<',
  '<=': '<=',
  '<>': '!=',
  '=': '=',
  '==': '=',
  '===': '=',
  '>': '>',
  '>=': '>=',
  between: '=',
  eq: '=',
  equals: '=',
  gt: '>',
  gte: '>=',
  in: '=',
  le: '<=',
  lt: '<',
  lte: '<=',
  ne: '!=',
  neq: '!=',
  notin: '!=',
};

let uidSequence = 0;

const draft = reactive({
  groupRelation: true,
  groups: [] as ConditionGroupDraft[],
  isDefault: false,
  nodeName: '',
});

const locked = computed(() => props.readonly || draft.isDefault);

const fieldOptions = computed(() =>
  props.formFields.map((field) => ({
    label: field.label ? `${field.label}（${field.key}）` : field.key,
    value: field.key,
  })),
);

const hasFieldOptions = computed(() => fieldOptions.value.length > 0);

const conditionCount = computed(() =>
  draft.groups.reduce(
    (total, group) =>
      total + group.rows.filter((row) => row.key.trim().length > 0).length,
    0,
  ),
);

watch(
  () => [props.open, props.modelValue, props.node] as const,
  () => {
    if (visible.value) {
      resetDraft();
    }
  },
  { immediate: true },
);

function nextUid(prefix: string): string {
  uidSequence += 1;
  return `${prefix}-${Date.now().toString(36)}-${uidSequence}`;
}

function deepClone<T>(value: T): T {
  // `toRaw` keeps the structured clone algorithm from receiving a reactive proxy.
  return structuredClone(toRaw(value));
}

function propertyOf(
  node: AntFlowNode | null | undefined,
): ConditionPropertyLike {
  return (node?.nodeProperty ?? {}) as unknown as ConditionPropertyLike;
}

function toBoolean(
  value: boolean | null | number | string | undefined,
  fallback: boolean,
): boolean {
  if (value === null || value === undefined || value === '') {
    return fallback;
  }
  if (typeof value === 'boolean') {
    return value;
  }
  if (typeof value === 'number') {
    return value !== 0;
  }
  const normalized = value.trim().toLowerCase();
  if (['1', 'and', 'true', '且'].includes(normalized)) {
    return true;
  }
  if (['0', 'false', 'or', '或'].includes(normalized)) {
    return false;
  }
  return fallback;
}

function normalizeOperator(value?: null | string): string {
  if (!value) {
    return '=';
  }
  return legacyOperatorMap[value.trim().toLowerCase()] ?? '=';
}

function valueToText(value: unknown): string {
  if (value === null || value === undefined) {
    return '';
  }
  if (Array.isArray(value)) {
    return value.map(String).join(',');
  }
  if (typeof value === 'object') {
    return JSON.stringify(value);
  }
  return String(value);
}

function createRow(item?: AntFlowConditionItemLike): ConditionRowDraft {
  return {
    key: item?.key ?? item?.columnDbName ?? '',
    operator: normalizeOperator(item?.optType),
    uid: nextUid('condition'),
    value: valueToText(item?.zdy1),
  };
}

function createGroup(
  item?: AntFlowConditionGroupLike,
  index = 0,
): ConditionGroupDraft {
  const conditionList = Array.isArray(item?.conditionList)
    ? item.conditionList
    : [];
  return {
    condRelation: toBoolean(item?.condRelation, true),
    groupId: item?.groupId ?? nextUid('group'),
    groupName: item?.groupName ?? `条件组 ${index + 1}`,
    rows: conditionList.map((condition) => createRow(condition)),
    uid: nextUid('group-row'),
  };
}

function resetDraft() {
  const node = props.node;
  const property = propertyOf(node);

  draft.nodeName = node?.nodeName ?? '';
  draft.isDefault = Number(property.isDefault) === 1;

  let groups = Array.isArray(property.groupConditions)
    ? property.groupConditions
    : [];
  // Older payloads keep a flat condition list instead of groups.
  if (
    groups.length === 0 &&
    Array.isArray(property.conditionList) &&
    property.conditionList.length > 0
  ) {
    groups = [
      {
        condRelation: property.condRelation,
        conditionList: property.conditionList,
      },
    ];
  }

  draft.groupRelation = toBoolean(property.groupRelation, true);
  draft.groups = groups.map((group, index) => createGroup(group, index));
}

function close() {
  emit('update:open', false);
  emit('update:modelValue', false);
}

function addGroup() {
  draft.groups.push(createGroup(undefined, draft.groups.length));
}

function removeGroup(group: ConditionGroupDraft) {
  draft.groups = draft.groups.filter((item) => item.uid !== group.uid);
}

function addCondition(group: ConditionGroupDraft) {
  group.rows.push(createRow());
}

function removeCondition(group: ConditionGroupDraft, row: ConditionRowDraft) {
  group.rows = group.rows.filter((item) => item.uid !== row.uid);
}

function fieldLabel(key: string): string {
  return props.formFields.find((field) => field.key === key)?.label ?? key;
}

function fieldType(key: string): string {
  return props.formFields.find((field) => field.key === key)?.type ?? 'input';
}

function buildConditionItem(row: ConditionRowDraft): AntFlowConditionItemLike {
  const key = row.key.trim();
  return {
    columnDbName: key,
    columnType: fieldType(key),
    key,
    label: fieldLabel(key),
    opt1: '',
    opt2: '',
    optType: row.operator,
    type: fieldType(key),
    zdy1: row.value.trim(),
    zdy2: '',
  };
}

function confirm() {
  const node = props.node;
  if (!node) {
    close();
    return;
  }

  if (!draft.isDefault) {
    if (conditionCount.value === 0) {
      message.warning('请至少配置一个条件');
      return;
    }

    const invalidGroup = draft.groups.find((group) =>
      group.rows.some(
        (row) => row.key.trim().length === 0 && row.value.trim().length > 0,
      ),
    );
    if (invalidGroup) {
      message.warning('请选择条件字段');
      return;
    }

    const missingValue = draft.groups.some((group) =>
      group.rows.some(
        (row) => row.key.trim().length > 0 && row.value.trim().length === 0,
      ),
    );
    if (missingValue) {
      message.warning('请填写条件值');
      return;
    }
  }

  const cloned = deepClone(node);
  const nodeName = draft.nodeName.trim() || cloned.nodeName;
  cloned.nodeName = nodeName;
  cloned.nodeDisplayName = nodeName;

  const property: ConditionPropertyLike = {
    ...propertyOf(cloned),
    // A cached expression would win over the edited groups in `antFlowToOld`.
    expression: null,
    // `conditionList` is the legacy flat form; groups are authoritative.
    conditionList: [],
    groupConditions: draft.isDefault
      ? []
      : draft.groups
          .map((group, index) => ({
            condRelation: group.condRelation,
            conditionList: group.rows
              .filter((row) => row.key.trim().length > 0)
              .map((row) => buildConditionItem(row)),
            groupId: group.groupId || `group-${index + 1}`,
            groupName: group.groupName || `条件组 ${index + 1}`,
            sort: index,
          }))
          .filter((group) => group.conditionList.length > 0),
    groupRelation: draft.groupRelation,
    isDefault: draft.isDefault ? 1 : 0,
  };

  (cloned as unknown as { nodeProperty: ConditionPropertyLike }).nodeProperty =
    property;
  cloned.error = false;

  emit('update:node', cloned);
  close();
}
</script>

<template>
  <Drawer
    :open="visible"
    placement="right"
    title="条件设置"
    :width="620"
    @close="close"
  >
    <div class="condition-drawer">
      <div class="form-item">
        <label class="form-label">分支名称</label>
        <Input
          v-model:value="draft.nodeName"
          :disabled="locked"
          :maxlength="50"
          placeholder="请输入条件分支名称"
        />
      </div>

      <template v-if="draft.isDefault">
        <Alert
          class="hint-alert"
          description="默认条件分支不允许设置条件，它是兜底分支：其它条件都未命中时进入该分支。如需添加条件，请先添加普通条件分支，再删除默认分支。"
          message="默认条件分支"
          show-icon
          type="warning"
        />
      </template>

      <template v-else>
        <div class="global-relation">
          <span class="relation-tip">当审批单满足以下条件时进入此流程</span>
          <span class="relation-label">条件组关系：</span>
          <Switch
            v-model:checked="draft.groupRelation"
            checked-children="且"
            :disabled="readonly"
            un-checked-children="或"
          />
        </div>

        <Card
          v-for="(group, groupIndex) in draft.groups"
          :key="group.uid"
          class="group-card"
          size="small"
        >
          <template #title>
            <div class="group-title">
              <Input
                v-model:value="group.groupName"
                class="group-name-input"
                :disabled="readonly"
                :maxlength="30"
                size="small"
              />
              <span class="relation-label">组内关系：</span>
              <Switch
                v-model:checked="group.condRelation"
                checked-children="且"
                :disabled="readonly"
                size="small"
                un-checked-children="或"
              />
            </div>
          </template>
          <template #extra>
            <Button
              danger
              :disabled="readonly"
              size="small"
              type="text"
              @click="removeGroup(group)"
            >
              删除条件组
            </Button>
          </template>

          <div class="rows">
            <Empty
              v-if="group.rows.length === 0"
              description="该条件组还没有条件"
            />
            <div v-for="row in group.rows" :key="row.uid" class="condition-row">
              <Select
                v-if="hasFieldOptions"
                v-model:value="row.key"
                class="field-key"
                :disabled="readonly"
                option-filter-prop="label"
                :options="fieldOptions"
                placeholder="选择字段"
                show-search
              />
              <Input
                v-else
                v-model:value="row.key"
                class="field-key"
                :disabled="readonly"
                placeholder="字段 key，例如 amount"
              />

              <Select
                v-model:value="row.operator"
                class="field-operator"
                :disabled="readonly"
                :options="operatorOptions"
              />

              <Input
                v-model:value="row.value"
                class="field-value"
                :disabled="readonly"
                placeholder="请输入比较值"
              />

              <Button
                danger
                :disabled="readonly"
                size="small"
                type="text"
                @click="removeCondition(group, row)"
              >
                删除
              </Button>
            </div>
          </div>

          <Button
            v-if="!readonly"
            class="add-condition-btn"
            size="small"
            type="dashed"
            @click="addCondition(group)"
          >
            + 添加条件
          </Button>

          <div
            v-if="groupIndex < draft.groups.length - 1"
            class="between-relation"
          >
            {{ draft.groupRelation ? '且满足' : '或满足' }}
          </div>
        </Card>

        <Button
          v-if="!readonly"
          class="add-group-btn"
          type="dashed"
          @click="addGroup"
        >
          + 添加条件组
        </Button>

        <div class="condition-summary">
          共 {{ draft.groups.length }} 个条件组 / {{ conditionCount }} 条条件
          <Tag v-if="draft.isDefault" color="orange">默认分支</Tag>
        </div>
      </template>

      <div v-if="readonly && !draft.isDefault" class="readonly-tip">
        当前为只读模式，无法修改条件配置。
      </div>
    </div>

    <template #footer>
      <div class="drawer-footer">
        <Button @click="close">{{ locked ? '关闭' : '取消' }}</Button>
        <Button v-if="!locked" type="primary" @click="confirm">确定</Button>
      </div>
    </template>
  </Drawer>
</template>

<style scoped>
.condition-drawer {
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.form-item {
  display: grid;
  grid-template-columns: 96px minmax(0, 1fr);
  gap: 12px;
  align-items: center;
}

.form-label {
  font-size: 14px;
  color: #4f5b6f;
  text-align: right;
}

.global-relation {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  align-items: center;
  padding: 8px 12px;
  background: #edf5ff;
  border: 1px solid #77b6ff;
  border-radius: 4px;
}

.relation-tip {
  font-size: 13px;
  color: #2f88ff;
}

.relation-label {
  margin-left: auto;
  font-size: 13px;
  color: #f59f42;
}

.group-card {
  background: #fbfcff;
  border-color: #dfe5f0;
  border-radius: 6px;
}

.group-card :deep(.ant-card-head) {
  min-height: 44px;
  padding: 0 10px;
}

.group-card :deep(.ant-card-body) {
  padding: 10px;
}

.group-title {
  display: flex;
  gap: 8px;
  align-items: center;
}

.group-title .relation-label {
  margin-left: 0;
  white-space: nowrap;
}

.group-name-input {
  width: 160px;
}

.rows {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.condition-row {
  display: grid;
  grid-template-columns: minmax(0, 1.2fr) 110px minmax(0, 1fr) 48px;
  gap: 8px;
  align-items: center;
  padding-bottom: 8px;
  border-bottom: 1px solid #ebeef5;
}

.condition-row:last-child {
  border-bottom: none;
}

.field-key,
.field-operator,
.field-value {
  width: 100%;
}

.add-condition-btn {
  width: 100%;
  margin-top: 8px;
}

.add-group-btn {
  width: 100%;
  height: 36px;
  font-weight: 600;
  color: #8f98a8;
  background: #eceff3;
  border: none;
}

.add-group-btn:hover {
  color: #6f7b8f;
  background: #e3e7ee;
}

.between-relation {
  margin: 6px 0 2px 6px;
  font-size: 12px;
  color: #f59f42;
}

.condition-summary {
  display: flex;
  gap: 8px;
  align-items: center;
  font-size: 12px;
  color: #8f98a8;
}

.readonly-tip {
  padding: 8px 12px;
  font-size: 12px;
  color: #8f98a8;
  background: #f7f9fc;
  border: 1px solid #ebf0f8;
  border-radius: 6px;
}

.hint-alert {
  border-radius: 8px;
}

.drawer-footer {
  display: flex;
  justify-content: flex-end;
  width: 100%;
}

@media (max-width: 760px) {
  .condition-row {
    grid-template-columns: 1fr;
  }

  .form-item {
    grid-template-columns: 1fr;
    gap: 6px;
  }

  .form-label {
    text-align: left;
  }
}
</style>
