<script setup lang="ts">
import type { FormFieldDto } from '../../types/approval-flow';
import type { FormulaConfig } from './formula';

import { computed, ref, watch } from 'vue';

import { $t, useI18n } from '@vben/locales';

import {
  Button,
  Card,
  Empty,
  Input,
  message,
  Select,
  Space,
  Switch,
  Tag,
} from 'ant-design-vue';

import { useApprovalFormsApi } from '../../api/useApprovalFormsApi';
import {
  formulaModeOptions as formulaModeOptionDefs,
  formulaOperatorOptions as formulaOperatorOptionDefs,
  formulaUnitOptions as formulaUnitOptionDefs,
  parseFormula,
  serializeFormula,
} from './formula';

interface DesignField {
  formula?: FormulaConfig;
  hint?: null | string;
  id?: string;
  isRequired: boolean;
  key: string;
  label: string;
  optionsText?: string;
  permission: string;
  type: string;
}

const props = defineProps<{
  code?: string;
  formId?: null | string;
  name?: string;
}>();

const emit = defineEmits<{
  'update:code': [string];
  'update:formId': [null | string];
  'update:name': [string];
}>();

const formsApi = useApprovalFormsApi();
const { locale, t } = useI18n();

const formulaModeOptions = computed(() => {
  void locale.value;
  return formulaModeOptionDefs.map((option) => ({
    label: t(option.labelKey),
    value: option.value,
  }));
});

const formulaOperatorOptions = computed(() => {
  void locale.value;
  return formulaOperatorOptionDefs.map((option) => ({
    label: t(option.labelKey),
    value: option.value,
  }));
});

const formulaUnitOptions = computed(() => {
  void locale.value;
  return formulaUnitOptionDefs.map((option) => ({
    label: t(option.labelKey),
    value: option.value,
  }));
});

const palette = [
  {
    icon: 'T',
    label: $t('antflow.designer.form.singleLineText'),
    type: 'single-line-text',
  },
  {
    icon: '¶',
    label: $t('antflow.designer.form.multiLineText'),
    type: 'multi-line-text',
  },
  { icon: '#', label: $t('antflow.designer.form.number'), type: 'number' },
  { icon: '▦', label: $t('antflow.designer.form.date'), type: 'date' },
  {
    icon: '▤',
    label: $t('antflow.designer.form.dateRange'),
    type: 'date-range',
  },
  { icon: '▾', label: $t('antflow.designer.form.select'), type: 'select' },
  { icon: '●', label: $t('antflow.designer.form.radio'), type: 'radio' },
  {
    icon: '☑',
    label: $t('antflow.designer.form.multipleSelect'),
    type: 'multiple-select',
  },
  { icon: '◐', label: $t('antflow.designer.form.switch'), type: 'switch' },
  {
    icon: '📎',
    label: $t('antflow.designer.form.attachment'),
    type: 'attachment',
  },
  {
    icon: '∑',
    label: $t('antflow.designer.form.calculated'),
    type: 'calculated',
  },
  {
    icon: 'ℹ',
    label: $t('antflow.designer.form.description'),
    type: 'description',
  },
];

const permissions = [
  { label: $t('antflow.designer.form.editable'), value: 'E' },
  { label: $t('antflow.designer.form.readOnly'), value: 'R' },
  { label: $t('antflow.designer.form.hidden'), value: 'H' },
];

const formName = ref(props.name ?? '');
const formCode = ref(props.code ?? '');
const fields = ref<DesignField[]>([]);
const selectedIndex = ref<null | number>(null);
const loading = ref(false);
const saving = ref(false);
const dragType = ref<null | string>(null);
const loadedFormId = ref<null | string>(null);
const dirty = ref(false);
const formPublished = ref(false);

async function loadForm(formId: null | string, force = false) {
  if (!formId) {
    fields.value = [];
    loadedFormId.value = null;
    dirty.value = false;
    return;
  }
  if (!force && loadedFormId.value === formId) {
    return;
  }

  loading.value = true;
  try {
    const definition = await formsApi.getApi(formId);
    formName.value = definition.name;
    formCode.value = definition.code;
    fields.value = (definition.fields ?? []).map((field) => mapField(field));
    loadedFormId.value = formId;
    dirty.value = false;
    formPublished.value = !!definition.isPublished;
    selectedIndex.value = fields.value.length > 0 ? 0 : null;
  } catch {
    message.error($t('antflow.designer.form.loadFailed'));
  } finally {
    loading.value = false;
  }
}

watch(
  () => props.formId,
  (formId) => loadForm(formId ?? null),
  { immediate: true },
);

watch(formName, (value) => emit('update:name', value));
watch(formCode, (value) => emit('update:code', value));

const selectedField = computed<DesignField | null>(
  () => fields.value[selectedIndex.value ?? -1] ?? null,
);

const supportsOptions = computed(() => {
  const type = selectedField.value?.type;
  return type === 'multiple-select' || type === 'radio' || type === 'select';
});

const supportsFormula = computed(
  () => selectedField.value?.type === 'calculated',
);

const numberFields = computed(() =>
  fields.value.filter((field) => field.type === 'number'),
);
const dateFields = computed(() =>
  fields.value.filter((field) => field.type === 'date'),
);
const rangeFields = computed(() =>
  fields.value.filter((field) => field.type === 'date-range'),
);

function mapField(field: FormFieldDto): DesignField {
  return {
    hint: field.hint ?? null,
    id: field.id,
    isRequired: field.isRequired,
    key: field.key,
    label: field.label,
    formula: parseFormula(field.optionsJson) ?? undefined,
    optionsText: parseOptions(field.optionsJson),
    permission: field.permission,
    type: field.type,
  };
}

function parseOptions(optionsJson?: null | string): string {
  if (!optionsJson) {
    return '';
  }

  try {
    const parsed = JSON.parse(optionsJson) as unknown;
    if (!Array.isArray(parsed)) {
      return '';
    }

    return parsed
      .map((item) =>
        typeof item === 'string'
          ? item
          : String((item as any).label ?? (item as any).value ?? ''),
      )
      .join('\n');
  } catch {
    return '';
  }
}

function onDragStart(event: DragEvent, type: string) {
  dragType.value = type;
  event.dataTransfer?.setData('text/plain', type);
  if (event.dataTransfer) {
    event.dataTransfer.effectAllowed = 'copy';
  }
}

function onDrop(event: DragEvent) {
  event.preventDefault();
  const type =
    dragType.value ??
    event.dataTransfer?.getData('text/plain') ??
    'single-line-text';
  addField(type);
  dragType.value = null;
}

function addField(type: string) {
  const item = palette.find((entry) => entry.type === type);
  const index = fields.value.length + 1;
  dirty.value = true;
  fields.value.push({
    hint: null,
    isRequired: false,
    key: `field_${index}`,
    label: item?.label ?? type,
    permission: type === 'calculated' ? 'R' : 'E',
    type,
  });
  selectedIndex.value = fields.value.length - 1;
}

function removeField(index: number) {
  dirty.value = true;
  fields.value.splice(index, 1);
  if (selectedIndex.value === index) {
    selectedIndex.value = fields.value.length > 0 ? 0 : null;
  }
}

function moveField(index: number, offset: number) {
  const target = index + offset;
  if (target < 0 || target >= fields.value.length) {
    return;
  }

  dirty.value = true;
  const [moved] = fields.value.splice(index, 1);
  if (moved) {
    fields.value.splice(target, 0, moved);
    selectedIndex.value = target;
  }
}

function updateSelected(patch: Partial<DesignField>) {
  const index = selectedIndex.value;
  if (index === null || !fields.value[index]) {
    return;
  }

  dirty.value = true;
  fields.value[index] = { ...fields.value[index], ...patch };
}

function updateFormula(patch: Partial<FormulaConfig>) {
  const current = selectedField.value?.formula ?? {
    left: '',
    mode: 'number' as const,
    operator: '+' as const,
    right: '',
    unit: 'day' as const,
  };
  updateSelected({ formula: { ...current, ...patch } });
}

const formulaFieldOptions = computed(() => {
  const mode = selectedField.value?.formula?.mode ?? 'number';
  let source = rangeFields.value;
  if (mode === 'number') {
    source = numberFields.value;
  } else if (mode === 'date-diff') {
    source = dateFields.value;
  }
  return source.map((field) => ({ label: field.label, value: field.key }));
});
function optionsJson(field: DesignField): null | string {
  if (field.type === 'calculated') {
    return serializeFormula(field.formula);
  }
  const lines = (field.optionsText ?? '')
    .split('\n')
    .map((line) => line.trim())
    .filter(Boolean);

  return lines.length > 0 ? JSON.stringify(lines) : null;
}

function toAddPayload(field: DesignField, index: number) {
  return {
    hint: field.hint ?? null,
    isRequired: field.isRequired,
    key: field.key,
    label: field.label,
    optionsJson: optionsJson(field),
    order: index,
    permission: field.permission,
    type: field.type,
  };
}

function toUpdatePayload(field: DesignField, index: number) {
  return {
    hint: field.hint ?? null,
    isRequired: field.isRequired,
    label: field.label,
    optionsJson: optionsJson(field),
    order: index,
    permission: field.permission,
  };
}

function typeLabel(type: string): string {
  return palette.find((item) => item.type === type)?.label ?? type;
}

async function createNewVersion(version: number): Promise<string> {
  const created = await formsApi.createApi({
    code: formCode.value.trim(),
    description: null,
    name: formName.value.trim() || formCode.value.trim(),
    version,
  });

  for (const [index, field] of fields.value.entries()) {
    await formsApi.addFieldApi(created.id, toAddPayload(field, index));
  }

  formPublished.value = false;
  emit('update:formId', created.id);
  loadedFormId.value = null;
  return created.id;
}

async function save(publishForm = true): Promise<null | string> {
  if (props.formId && !dirty.value) {
    if (publishForm && !formPublished.value) {
      await formsApi.publishApi(props.formId);
      formPublished.value = true;
    }
    return props.formId;
  }

  if (!formCode.value.trim()) {
    message.warning($t('antflow.designer.form.codeRequired'));
    return null;
  }
  if (fields.value.length === 0) {
    message.warning($t('antflow.designer.form.fieldRequired'));
    return null;
  }

  saving.value = true;
  try {
    let formId = props.formId ?? null;

    if (formId) {
      const current = await formsApi.getApi(formId);
      if (current.isPublished) {
        // Published definitions are immutable: edits create a new version so existing
        // approval instances keep pointing at the version they were started with.
        formId = await createNewVersion(current.version + 1);
      } else {
        const keptIds = new Set(
          fields.value.filter((field) => field.id).map((field) => field.id!),
        );

        for (const field of current.fields ?? []) {
          if (!keptIds.has(field.id)) {
            await formsApi.removeFieldApi(formId, field.id);
          }
        }

        for (const [index, field] of fields.value.entries()) {
          const exists =
            field.id && current.fields?.some((item) => item.id === field.id);
          await (exists
            ? formsApi.updateFieldApi(
                formId,
                field.id!,
                toUpdatePayload(field, index),
              )
            : formsApi.addFieldApi(formId, toAddPayload(field, index)));
        }
        loadedFormId.value = null;
      }
    } else {
      formId = await createNewVersion(1);
    }

    await formsApi.publishApi(formId);
    dirty.value = false;
    message.success($t('antflow.designer.form.saved'));
    return formId;
  } catch {
    message.error($t('antflow.designer.form.saveFailed'));
    return null;
  } finally {
    saving.value = false;
  }
}
function getFields(): FormFieldDto[] {
  return fields.value.map((field, index) => ({
    hint: field.hint ?? null,
    id: field.id ?? field.key,
    isRequired: field.isRequired,
    key: field.key,
    label: field.label,
    optionsJson: optionsJson(field),
    order: index,
    permission: field.permission,
    type: field.type,
  }));
}

defineExpose({ getFields, loadForm, save });
</script>

<template>
  <div class="form-designer">
    <Card :loading="loading" size="small">
      <div class="flex flex-wrap items-center gap-3">
        <Input
          v-model:value="formName"
          :addon-before="$t('antflow.designer.form.formName')"
          class="w-64"
          :placeholder="$t('antflow.designer.form.formNamePlaceholder')"
        />
        <Input
          v-model:value="formCode"
          :addon-before="$t('antflow.designer.form.formCode')"
          class="w-64"
          :placeholder="$t('antflow.designer.form.formCodePlaceholder')"
        />
        <Tag v-if="formId" color="green">
          {{ $t('antflow.designer.form.savedTag') }}
        </Tag>
        <Tag v-else color="orange">
          {{ $t('antflow.designer.form.unsavedTag') }}
        </Tag>
      </div>
    </Card>

    <div class="mt-3 flex gap-3">
      <Card
        class="w-40 shrink-0"
        size="small"
        :title="$t('antflow.designer.form.controls')"
      >
        <div class="flex flex-col gap-2">
          <div
            v-for="item in palette"
            :key="item.type"
            class="palette-item"
            draggable="true"
            @dragstart="onDragStart($event, item.type)"
            @click="addField(item.type)"
          >
            <span class="palette-icon">{{ item.icon }}</span>
            <span>{{ item.label }}</span>
          </div>
        </div>
      </Card>

      <Card
        class="min-h-72 flex-1"
        size="small"
        :title="$t('antflow.designer.form.canvas')"
      >
        <div class="drop-canvas" @dragover.prevent @drop="onDrop">
          <Empty
            v-if="fields.length === 0"
            :description="$t('antflow.designer.form.dragHint')"
          />
          <div
            v-for="(field, index) in fields"
            :key="field.id ?? `${field.key}-${index}`"
            class="field-card"
            :class="{ 'field-card-active': index === selectedIndex }"
            @click="selectedIndex = index"
          >
            <div class="flex items-center justify-between gap-2">
              <div class="min-w-0">
                <div class="flex items-center gap-2">
                  <Tag color="blue">{{ typeLabel(field.type) }}</Tag>
                  <span class="truncate font-medium">{{ field.label }}</span>
                  <Tag v-if="field.isRequired" color="red">
                    {{ $t('antflow.designer.form.required') }}
                  </Tag>
                </div>
                <div class="mt-1 text-xs text-gray-500">{{ field.key }}</div>
              </div>
              <Space size="small">
                <Button
                  size="small"
                  type="text"
                  @click.stop="moveField(index, -1)"
                >
                  ↑
                </Button>
                <Button
                  size="small"
                  type="text"
                  @click.stop="moveField(index, 1)"
                >
                  ↓
                </Button>
                <Button
                  danger
                  size="small"
                  type="text"
                  @click.stop="removeField(index)"
                >
                  {{ $t('antflow.designer.form.delete') }}
                </Button>
              </Space>
            </div>
          </div>
        </div>
      </Card>

      <Card
        class="w-64 shrink-0"
        size="small"
        :title="$t('antflow.designer.form.fieldProperties')"
      >
        <Empty
          v-if="!selectedField"
          :description="$t('antflow.designer.form.selectField')"
        />
        <template v-else>
          <div class="flex flex-col gap-3">
            <div>
              <div class="mb-1 text-xs text-gray-500">
                {{ $t('antflow.designer.form.displayName') }}
              </div>
              <Input
                :value="selectedField.label"
                @update:value="
                  (value: string) => updateSelected({ label: value })
                "
              />
            </div>
            <div>
              <div class="mb-1 text-xs text-gray-500">
                {{ $t('antflow.designer.form.fieldKey') }}
              </div>
              <Input
                :disabled="!!selectedField.id"
                :value="selectedField.key"
                @update:value="
                  (value: string) => updateSelected({ key: value })
                "
              />
            </div>
            <div class="flex items-center justify-between">
              <span class="text-xs text-gray-500">{{
                $t('antflow.designer.form.required')
              }}</span>
              <Switch
                :checked="selectedField.isRequired"
                @update:checked="
                  (value: any) => updateSelected({ isRequired: Boolean(value) })
                "
              />
            </div>
            <div>
              <div class="mb-1 text-xs text-gray-500">
                {{ $t('antflow.designer.form.permission') }}
              </div>
              <Select
                :options="permissions"
                :value="selectedField.permission"
                class="w-full"
                @update:value="
                  (value: any) => updateSelected({ permission: String(value) })
                "
              />
            </div>
            <div>
              <div class="mb-1 text-xs text-gray-500">
                {{ $t('antflow.designer.form.hint') }}
              </div>
              <Input
                :value="selectedField.hint ?? ''"
                @update:value="
                  (value: string) => updateSelected({ hint: value || null })
                "
              />
            </div>
            <div v-if="supportsOptions">
              <div class="mb-1 text-xs text-gray-500">
                {{ $t('antflow.designer.form.options') }}
              </div>
              <Input.TextArea
                :rows="4"
                :value="selectedField.optionsText ?? ''"
                @update:value="
                  (value: string) => updateSelected({ optionsText: value })
                "
              />
            </div>
            <template v-if="supportsFormula">
              <div>
                <div class="mb-1 text-xs text-gray-500">
                  {{ $t('antflow.designer.form.calcType') }}
                </div>
                <Select
                  :options="formulaModeOptions"
                  :value="selectedField.formula?.mode ?? 'number'"
                  class="w-full"
                  @update:value="(value: any) => updateFormula({ mode: value })"
                />
              </div>
              <div>
                <div class="mb-1 text-xs text-gray-500">
                  {{
                    selectedField.formula?.mode === 'date-range'
                      ? $t('antflow.designer.form.dateRange')
                      : $t('antflow.designer.form.fieldA')
                  }}
                </div>
                <Select
                  :options="formulaFieldOptions"
                  :value="selectedField.formula?.left ?? ''"
                  class="w-full"
                  @update:value="
                    (value: any) => updateFormula({ left: String(value) })
                  "
                />
              </div>
              <div v-if="selectedField.formula?.mode === 'number'">
                <div class="mb-1 text-xs text-gray-500">
                  {{ $t('antflow.designer.branch.operator') }}
                </div>
                <Select
                  :options="formulaOperatorOptions"
                  :value="selectedField.formula?.operator ?? '+'"
                  class="w-full"
                  @update:value="
                    (value: any) => updateFormula({ operator: value })
                  "
                />
              </div>
              <div v-if="selectedField.formula?.mode !== 'date-range'">
                <div class="mb-1 text-xs text-gray-500">
                  {{ $t('antflow.designer.form.fieldB') }}
                </div>
                <Select
                  :options="formulaFieldOptions"
                  :value="selectedField.formula?.right ?? ''"
                  class="w-full"
                  @update:value="
                    (value: any) => updateFormula({ right: String(value) })
                  "
                />
              </div>
              <div v-if="selectedField.formula?.mode !== 'number'">
                <div class="mb-1 text-xs text-gray-500">
                  {{ $t('antflow.designer.form.resultUnit') }}
                </div>
                <Select
                  :options="formulaUnitOptions"
                  :value="selectedField.formula?.unit ?? 'day'"
                  class="w-full"
                  @update:value="(value: any) => updateFormula({ unit: value })"
                />
              </div>
            </template>
          </div>
        </template>
      </Card>
    </div>
  </div>
</template>

<style scoped>
.palette-item {
  display: flex;
  gap: 8px;
  align-items: center;
  padding: 6px 8px;
  font-size: 12px;
  cursor: grab;
  border: 1px dashed #d1d5db;
  border-radius: 6px;
}

.palette-item:hover {
  border-color: #1677ff;
}

.palette-icon {
  width: 16px;
  color: #1677ff;
  text-align: center;
}

.drop-canvas {
  display: flex;
  flex-direction: column;
  gap: 8px;
  min-height: 240px;
}

.field-card {
  padding: 8px 10px;
  cursor: pointer;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
}

.field-card:hover,
.field-card-active {
  border-color: #1677ff;
}
</style>
