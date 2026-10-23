<script setup lang="ts">
import type {
  ApprovalFlowCondition,
  ApprovalFlowNode,
  ApproverKind,
  ApproverRule,
  EmptyApproverPolicy,
  FormFieldDto,
  ParticipantRef,
  SupervisorLevel,
} from '../../types/approval-flow';

import { computed, ref, watch } from 'vue';

import {
  Button,
  DatePicker,
  Divider,
  Drawer,
  Form,
  FormItem,
  Input,
  InputNumber,
  message,
  RadioGroup,
  Select,
  Space,
  Tag,
} from 'ant-design-vue';

import { useApprovalParticipantsApi } from '../../api/useApprovalParticipantsApi';
import { createRule } from './model';

const props = defineProps<{
  open: boolean;
  node: ApprovalFlowNode | null;
  formFields?: FormFieldDto[];
}>();

const emit = defineEmits<{
  'update:node': [ApprovalFlowNode];
  'update:open': [boolean];
}>();

const participantsApi = useApprovalParticipantsApi();
const local = ref<ApprovalFlowNode | null>(null);
const users = ref<ParticipantRef[]>([]);
const organizationUnits = ref<ParticipantRef[]>([]);
const roles = ref<ParticipantRef[]>([]);

const ruleKindOptions = [
  { label: '指定人员/部门', value: 'Participant' },
  { label: '发起人', value: 'Initiator' },
  { label: '发起人自选', value: 'InitiatorSelection' },
  { label: '角色', value: 'Role' },
  { label: '部门主管', value: 'Supervisor' },
  { label: '部门成员', value: 'OrganizationUnit' },
  { label: '表单字段', value: 'FormField' },
];

const participantOptions = computed(() => {
  const all = [...users.value, ...organizationUnits.value, ...roles.value];
  return all.map((participant) => ({
    label: `${participant.displayName ?? participant.id}`,
    value: encodeParticipant(participant),
  }));
});

const roleOptions = computed(() =>
  roles.value.map((role) => ({
    label: role.displayName ?? role.id,
    value: encodeParticipant(role),
  })),
);

const signModeOptions = [
  { label: '或签（一人通过即可）', value: 'Any' },
  { label: '会签（全部通过）', value: 'All' },
  { label: '顺签（依次审批）', value: 'Sequential' },
];

watch(
  () => props.node,
  (node) => {
    local.value = node ? (JSON.parse(JSON.stringify(node)) as ApprovalFlowNode) : null;
    hydrateCondition(local.value);
  },
  { immediate: true },
);

function encodeParticipant(participant: ParticipantRef): string {
  return `${participant.provider}|${participant.type}|${participant.id}`;
}

function decodeParticipant(value: string): null | ParticipantRef {
  const [provider, type, id] = value.split('|');
  if (!provider || !type || !id) {
    return null;
  }

  const candidate = [...users.value, ...organizationUnits.value, ...roles.value].find(
    (item) => item.provider === provider && item.type === type && item.id === id,
  );

  return {
    provider,
    type: type as ParticipantRef['type'],
    id,
    displayName: candidate?.displayName,
  };
}

async function loadParticipants() {
  try {
    const [userList, unitList] = await Promise.all([
      participantsApi.searchUsersAsync(),
      participantsApi.getOrganizationUnitsAsync(),
    ]);
    users.value = userList ?? [];
    organizationUnits.value = unitList ?? [];
  } catch {
    // Participant search is optional for editing; keep the designer usable offline.
  }
}

async function searchUsers(filter: string) {
  if (!filter) {
    return;
  }

  try {
    users.value = (await participantsApi.searchUsersAsync(filter)) ?? [];
  } catch {
    message.warning('人员检索失败');
  }
}

function patch(partial: Partial<ApprovalFlowNode>) {
  if (!local.value) {
    return;
  }

  local.value = { ...local.value, ...partial };
  emit('update:node', local.value);
}

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

const formFieldOptions = computed(() =>
  (props.formFields ?? []).map((field) => ({
    label: field.label,
    value: field.key,
  })),
);

function findField(key?: null | string): FormFieldDto | undefined {
  return (props.formFields ?? []).find((field) => field.key === key);
}

function isEmptyOperator(operator?: null | string): boolean {
  return operator === 'empty' || operator === 'notempty';
}

function valueKind(): string {
  const field = findField(local.value?.condition?.fieldKey);
  switch (field?.type) {
    case 'date':
    case 'date-range': {
      return 'date';
    }
    case 'number': {
      return 'number';
    }
    case 'multiple-select':
    case 'radio':
    case 'select':
    case 'switch': {
      return 'select';
    }
    default: {
      return 'text';
    }
  }
}

function valueOptions(): { label: string; value: string }[] {
  const field = findField(local.value?.condition?.fieldKey);
  if (!field) {
    return [];
  }
  if (field.type === 'switch') {
    return [
      { label: '是', value: 'true' },
      { label: '否', value: 'false' },
    ];
  }
  if (!field.optionsJson) {
    return [];
  }

  try {
    const parsed = JSON.parse(field.optionsJson) as unknown;
    if (!Array.isArray(parsed)) {
      return [];
    }
    return parsed.map((item) =>
      typeof item === 'string'
        ? { label: item, value: item }
        : {
            label: String((item as any).label ?? (item as any).value ?? ''),
            value: String((item as any).value ?? (item as any).label ?? ''),
          },
    );
  } catch {
    return [];
  }
}

function composeExpression(condition: any): null | string {
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

  const field = findField(key);
  const isNumber = field?.type === 'number';
  const isBoolean = field?.type === 'switch';
  const literal = isNumber || isBoolean
    ? raw
    : `'${String(raw).replace(/'/g, "\\'")}'`;

  if (operator === 'contains') {
    return `String(form.${key} ?? '').includes(${literal})`;
  }
  if (operator === 'notcontains') {
    return `!String(form.${key} ?? '').includes(${literal})`;
  }

  return `form.${key} ${operator} ${literal}`;
}

function hydrateCondition(node: ApprovalFlowNode | null) {
  if (!node || node.kind !== 'condition') {
    return;
  }

  const condition = node.condition ?? {
    fieldKey: null,
    operator: '=',
    compareValue: null,
    expression: null,
  };

  const expression =
    condition.expression ??
    node.conditionExpression ??
    node.branches?.[0]?.expression ??
    node.expression ??
    null;

  if (!condition.fieldKey && expression) {
    const match = /form\.(\w+)\s*(===|!==|==|!=|>=|<=|>|<)\s*'?([^']*)'?/.exec(
      expression,
    );
    if (match) {
      condition.fieldKey = match[1] ?? null;
      condition.operator = match[2] ?? '=';
      condition.compareValue = match[3] ?? null;
    }
  }

  condition.expression = expression;
  node.condition = condition;
}

function updateCondition(partial: Partial<ApprovalFlowCondition>) {
  if (!local.value) {
    return;
  }

  const condition = { ...(local.value.condition ?? {}), ...partial };
  condition.expression = composeExpression(condition);

  const branches = (local.value.branches ?? []).map((branch, index) => ({
    ...branch,
    expression: index === 0 ? condition.expression : null,
  }));

  local.value = { ...local.value, branches, condition };
  emit('update:node', local.value);
}

function setConditionField(value: any) {
  updateCondition({ fieldKey: value ? String(value) : null });
}

function setConditionOperator(value: any) {
  updateCondition({ operator: String(value ?? '=') });
}

function setConditionValue(value: any) {
  updateCondition({
    compareValue: value === null || value === undefined ? null : String(value),
  });
}
function updateRule(index: number, partial: Partial<ApproverRule>) {
  if (!local.value) {
    return;
  }

  const rules = [...local.value.rules];
  rules[index] = { ...rules[index], ...partial } as ApproverRule;
  patch({ rules });
}

function addRule() {
  if (!local.value) {
    return;
  }

  patch({ rules: [...local.value.rules, createRule()] });
}

function removeRule(index: number) {
  if (!local.value) {
    return;
  }

  patch({ rules: local.value.rules.filter((_, i) => i !== index) });
}

function onParticipantsChange(index: number, values: string[]) {
  const participants = values
    .map((value) => decodeParticipant(value))
    .filter((participant): participant is ParticipantRef => participant !== null);

  updateRule(index, { participants });
}

function addAction() {
  if (!local.value) {
    return;
  }

  patch({
    actions: [
      ...local.value.actions,
      { key: `Action${local.value.actions.length + 1}`, label: '同意', effect: 'Approve' },
    ],
  });
}

function updateActionLabel(index: number, label: string) {
  if (!local.value) {
    return;
  }

  const actions = [...local.value.actions];
  actions[index] = { ...actions[index]!, label };
  patch({ actions });
}

function removeAction(index: number) {
  if (!local.value) {
    return;
  }

  patch({ actions: local.value.actions.filter((_, i) => i !== index) });
}

function onOpenChange(value: boolean) {
  emit('update:open', value);
}

loadParticipants();
</script>

<template>
  <Drawer
    :open="open"
    :title="local ? `节点设置 - ${local.name}` : '节点设置'"
    width="460"
    @close="onOpenChange(false)"
  >
    <Form v-if="local" layout="vertical">
      <FormItem label="节点名称">
        <Input
          :value="local.name"
          placeholder="请输入节点名称"
          @update:value="(value) => patch({ name: value })"
        />
      </FormItem>

      <template v-if="local.kind === 'approval'">
        <FormItem label="审批方式">
          <RadioGroup
            :value="local.signMode"
            :options="signModeOptions"
            @update:value="(value) => patch({ signMode: value })"
          />
        </FormItem>

        <Divider orientation="left">审批人规则</Divider>

        <div
          v-for="(rule, index) in local.rules"
          :key="index"
          class="mb-3 rounded-md border border-gray-200 p-3"
        >
          <Space class="mb-2 w-full" direction="vertical">
            <Select
              :value="rule.kind"
              :options="ruleKindOptions"
              class="w-full"
              @update:value="(value: any) => updateRule(index, { kind: value as ApproverKind })"
            />
            <Select
              v-if="rule.kind === 'Participant'"
              mode="multiple"
              :filter-option="false"
              :options="participantOptions"
              :value="rule.participants.map((participant) => encodeParticipant(participant))"
              class="w-full"
              placeholder="搜索并选择人员或部门"
              @search="searchUsers"
              @update:value="(values: any) => onParticipantsChange(index, values as string[])"
            />            <Select
              v-if="rule.kind === 'Role'"
              mode="multiple"
              :options="roleOptions"
              :value="rule.participants.map((participant) => encodeParticipant(participant))"
              class="w-full"
              placeholder="选择角色"
              @update:value="(values: any) => onParticipantsChange(index, values as string[])"
            />
            <div v-if="rule.kind === 'Supervisor'" class="flex gap-2">
              <Select
                :value="rule.supervisorLevel ?? 'Nearest'"
                :options="[
                  { label: '最近一级主管', value: 'Nearest' },
                  { label: '指定层级', value: 'LevelN' },
                  { label: '最高层级', value: 'TopMost' },
                ]"
                class="flex-1"
                @update:value="(value: any) => updateRule(index, { supervisorLevel: value as SupervisorLevel })"
              />
              <InputNumber
                v-if="rule.supervisorLevel === 'LevelN'"
                :value="rule.level ?? 1"
                :min="1"
                :max="20"
                @update:value="(value: any) => updateRule(index, { level: Number(value) || 1 })"
              />
            </div>
            <Select
              v-if="rule.kind === 'FormField'"
              :value="rule.formFieldKey ?? undefined"
              :options="(formFields ?? []).map((field) => ({ label: field.label, value: field.key }))"
              class="w-full"
              placeholder="选择表单字段"
              @update:value="(value: any) => updateRule(index, { formFieldKey: String(value ?? '') })"
            />
          </Space>
          <Button danger size="small" type="link" @click="removeRule(index)">
            删除规则
          </Button>
        </div>

        <Button block type="dashed" @click="addRule">添加审批人规则</Button>

        <Divider orientation="left">空审批人处理</Divider>
        <Select
          :value="local.onEmpty"
          :options="[
            { label: '跳过该节点', value: 'Skip' },
            { label: '升级到部门主管', value: 'EscalateToManager' },
          ]"
          class="w-full"
          @update:value="(value: any) => patch({ onEmpty: value as EmptyApproverPolicy })"
        />

        <Divider orientation="left">审批动作</Divider>
        <Space
          v-for="(action, index) in local.actions"
          :key="action.key"
          class="mb-2"
        >
          <Tag color="blue">{{ action.key }}</Tag>
          <Input
            :value="action.label"
            style="width: 120px"
            @update:value="(value) => updateActionLabel(index, value)"
          />
          <Button danger size="small" type="link" @click="removeAction(index)">
            删除
          </Button>
        </Space>
        <Button block type="dashed" @click="addAction">添加动作</Button>
      </template>

      <template v-if="local.kind === 'cc'">
        <Divider orientation="left">抄送人</Divider>
        <div
          v-for="(rule, index) in local.rules"
          :key="index"
          class="mb-3 rounded-md border border-gray-200 p-3"
        >
          <Select
            mode="multiple"
            :filter-option="false"
            :options="participantOptions"
            :value="rule.participants.map((participant) => encodeParticipant(participant))"
            class="w-full"
            placeholder="搜索并选择人员或部门"
            @search="searchUsers"
            @update:value="(values: any) => onParticipantsChange(index, values as string[])"
          />
          <Button danger size="small" type="link" @click="removeRule(index)">
            删除抄送
          </Button>
        </div>
        <Button block type="dashed" @click="addRule">添加抄送人</Button>
      </template>

      <template v-if="local.kind === 'condition'">
        <Divider orientation="left">条件设置</Divider>
        <Space class="w-full" direction="vertical">
          <Select
            :options="formFieldOptions"
            :value="local.condition?.fieldKey ?? undefined"
            placeholder="选择表单字段"
            @update:value="(value: any) => setConditionField(value)"
          />
          <Space class="w-full">
            <Select
              :options="conditionOperators"
              :value="local.condition?.operator ?? '='"
              style="width: 120px"
              @update:value="(value: any) => setConditionOperator(value)"
            />
            <Select
              v-if="!isEmptyOperator(local.condition?.operator) && valueOptions().length > 0"
              :options="valueOptions()"
              :value="local.condition?.compareValue ?? undefined"
              placeholder="选择比较值"
              @update:value="(value: any) => setConditionValue(value)"
            />
            <InputNumber
              v-else-if="!isEmptyOperator(local.condition?.operator) && valueKind() === 'number'"
              :value="local.condition?.compareValue ? Number(local.condition.compareValue) : undefined"
              class="flex-1"
              placeholder="输入数字"
              @update:value="(value: any) => setConditionValue(value)"
            />
            <DatePicker
              v-else-if="!isEmptyOperator(local.condition?.operator) && valueKind() === 'date'"
              :value="local.condition?.compareValue ?? undefined"
              class="flex-1"
              placeholder="选择日期"
              value-format="YYYY-MM-DD"
              @update:value="(value: any) => setConditionValue(value)"
            />
            <Input
              v-else-if="!isEmptyOperator(local.condition?.operator)"
              :value="local.condition?.compareValue ?? ''"
              placeholder="输入比较内容"
              @update:value="(value: any) => setConditionValue(value)"
            />
          </Space>
          <div class="rounded bg-gray-50 p-2 text-xs text-gray-500">
            {{ local.condition?.expression || '请选择字段并设置条件' }}
          </div>
        </Space>

        <Divider orientation="left">分支</Divider>
        <div
          v-for="(branch, index) in local.branches ?? []"
          :key="branch.id"
          class="mb-2 rounded-md border border-gray-200 p-3"
        >
          <Tag :color="index === 0 ? 'green' : 'default'">
            {{ index === 0 ? '符合条件（true）' : '不符合条件（false）' }}
          </Tag>
          <span class="ml-2 text-xs text-gray-500">{{ branch.nodes.length }} 个节点</span>
          <div
            v-for="child in branch.nodes"
            :key="child.id"
            class="mt-1 text-xs text-gray-600"
          >
            · {{ child.name }}
          </div>
        </div>
        <div class="mt-2 text-xs text-gray-400">
          条件成立进入「符合条件」分支，否则进入「不符合条件」分支；两个分支创建时已各带一个默认审批人节点，
          可在画布中继续添加节点。
        </div>
      </template>
    </Form>
  </Drawer>
</template>
