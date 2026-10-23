<script setup lang="ts">
import type {
  ApprovalFlowBranch,
  ApprovalFlowCondition,
  FormFieldDto,
} from '../../types/approval-flow';

import { ref, watch } from 'vue';

import {
  Button,
  Input,
  message,
  Modal,
  Select,
} from 'ant-design-vue';

const props = defineProps<{
  branch: ApprovalFlowBranch | null;
  fields: FormFieldDto[];
  open: boolean;
}>();

const emit = defineEmits<{
  'update:open': [boolean];
  'update:branch': [ApprovalFlowBranch];
}>();

const conditionOperators = [
  { label: '等于', value: '=' },
  { label: '不等于', value: '!=' },
  { label: '大于', value: '>' },
  { label: '大于等于', value: '>=' },
  { label: '小于', value: '<' },
  { label: '小于等于', value: '<=' },
  { label: '包含', value: 'contains' },
  { label: '不包含', value: 'notcontains' },
  { label: '为空', value: 'empty' },
  { label: '不为空', value: 'notempty' },
];

const logicOptions = [
  { label: '同时满足（AND）', value: 'and' },
  { label: '满足任一（OR）', value: 'or' },
];

const conditions = ref<ApprovalFlowCondition[]>([]);
const logic = ref<'and' | 'or'>('and');

watch(
  () => [props.open, props.branch] as const,
  ([open, branch]) => {
    if (!open || !branch) {
      return;
    }

    if (branch.conditions && branch.conditions.length > 0) {
      conditions.value = branch.conditions.map((condition) => ({ ...condition }));
    } else {
      conditions.value = [
        {
          compareValue: branch.compareValue ?? null,
          expression: branch.expression ?? null,
          fieldKey: branch.fieldKey ?? null,
          operator: branch.operator ?? '=',
        },
      ];
    }
    logic.value = branch.logic ?? 'and';
  },
  { immediate: true },
);

function fieldOptions() {
  return props.fields.map((field) => ({
    label: `${field.label} (${field.key})`,
    value: field.key,
  }));
}

function addCondition() {
  conditions.value = [
    ...conditions.value,
    { compareValue: null, expression: null, fieldKey: null, operator: '=' },
  ];
}

function removeCondition(index: number) {
  if (conditions.value.length <= 1) {
    return;
  }
  conditions.value = conditions.value.filter((_, i) => i !== index);
}

function isValueHidden(operator?: null | string): boolean {
  return operator === 'empty' || operator === 'notempty';
}

function fieldType(key?: null | string): string {
  return props.fields.find((field) => field.key === key)?.type ?? 'text';
}

function composeCondition(condition: ApprovalFlowCondition): null | string {
  const key = condition.fieldKey;
  if (!key) {
    return null;
  }

  const operator = condition.operator ?? '=';
  if (operator === 'empty') {
    return `(form.${key} == null || form.${key} === '')`;
  }
  if (operator === 'notempty') {
    return `(form.${key} != null && form.${key} !== '')`;
  }

  const raw = condition.compareValue ?? '';
  if (raw === '') {
    return null;
  }

  const type = fieldType(key);
  const isNumber = type === 'number';
  const isBoolean = type === 'switch';
  const literal = isNumber || isBoolean
    ? raw
    : `'${String(raw).replace(/'/g, "\\'")}'`;

  if (operator === 'contains') {
    return `String(form.${key} ?? '').includes(${literal})`;
  }
  if (operator === 'notcontains') {
    return `!String(form.${key} ?? '').includes(${literal})`;
  }

  const jsOperator = operator === '=' ? '==' : operator;
  return `form.${key} ${jsOperator} ${literal}`;
}

function save() {
  if (!props.branch) {
    return;
  }

  const expressions = conditions.value
    .map((condition) => composeCondition(condition))
    .filter((expression): expression is string => !!expression);

  if (expressions.length !== conditions.value.length) {
    message.warning('请填写完整的条件字段、运算符和比较值');
    return;
  }

  const expression = expressions.length === 1
    ? expressions[0]!
    : expressions
        .map((item) => `(${item})`)
        .join(logic.value === 'and' ? ' && ' : ' || ');

  const first = conditions.value[0];
  emit('update:branch', {
    ...props.branch,
    compareValue: first?.compareValue ?? null,
    conditions: conditions.value.map((condition) => ({ ...condition })),
    expression,
    fieldKey: first?.fieldKey ?? null,
    logic: logic.value,
    operator: first?.operator ?? '=',
  });
  emit('update:open', false);
}
</script>

<template>
  <Modal
    :open="open"
    :title="branch?.name ? `设置${branch.name}` : '设置条件'"
    :width="680"
    @cancel="emit('update:open', false)"
    @ok="save"
  >
    <div class="condition-form">
      <div v-if="conditions.length > 1" class="logic-row">
        <span class="condition-label">条件关系</span>
        <Select v-model:value="logic" :options="logicOptions" class="w-full" />
      </div>

      <div class="condition-table">
        <div class="condition-table-head">
          <span>条件字段</span>
          <span>运算符</span>
          <span>比较值</span>
          <span></span>
        </div>

        <div
          v-for="(condition, index) in conditions"
          :key="index"
          class="condition-row"
        >
          <Select
            :options="fieldOptions()"
            :value="condition.fieldKey ?? undefined"
            class="w-full"
            placeholder="选择字段"
            @update:value="(value) => (condition.fieldKey = value ? String(value) : null)"
          />
          <Select
            :options="conditionOperators"
            :value="condition.operator ?? undefined"
            class="w-full"
            @update:value="(value) => (condition.operator = value ? String(value) : '=')"
          />
          <Input
            v-if="!isValueHidden(condition.operator)"
            :value="condition.compareValue ?? ''"
            class="w-full"
            placeholder="比较值"
            @update:value="(value) => (condition.compareValue = value)"
          />
          <span v-else class="empty-value">—</span>
          <Button
            :disabled="conditions.length <= 1"
            danger
            size="small"
            type="text"
            @click="removeCondition(index)"
          >
            删除
          </Button>
        </div>
      </div>

      <Button block type="dashed" @click="addCondition">＋ 添加条件</Button>
    </div>
  </Modal>
</template>

<style scoped>
.condition-form {
  display: flex;
  flex-direction: column;
  gap: 14px;
  padding: 8px 0;
}

.logic-row {
  display: grid;
  grid-template-columns: 80px 1fr;
  gap: 10px;
  align-items: center;
}

.condition-label {
  font-size: 13px;
  color: #606266;
}

.condition-table {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.condition-table-head,
.condition-row {
  display: grid;
  grid-template-columns: 1.4fr 0.9fr 1.4fr 52px;
  gap: 8px;
  align-items: center;
}

.condition-table-head {
  font-size: 12px;
  color: #909399;
}

.condition-row .ant-select,
.condition-row :deep(.ant-select),
.condition-row :deep(.ant-input),
.w-full,
:deep(.ant-select) {
  width: 100%;
}

.empty-value {
  color: #c0c4cc;
  text-align: center;
}
</style>
