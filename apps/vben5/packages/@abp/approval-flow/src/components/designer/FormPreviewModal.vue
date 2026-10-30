<script setup lang="ts">
import type { FormFieldDto } from '../../types/approval-flow';

import { computed, reactive, watch } from 'vue';

import { $t } from '@vben/locales';

import {
  DatePicker,
  Form,
  FormItem,
  Input,
  InputNumber,
  Modal,
  RadioGroup,
  Select,
  Switch,
} from 'ant-design-vue';

const props = defineProps<{
  fields: FormFieldDto[];
  open: boolean;
}>();

const emit = defineEmits<{
  'update:open': [boolean];
}>();

const data = reactive<Record<string, any>>({});

const previewJson = computed(() => JSON.stringify(data, null, 2));

watch(
  () => props.open,
  (open) => {
    if (!open) {
      return;
    }

    for (const key of Object.keys(data)) {
      delete data[key];
    }
    for (const field of props.fields) {
      data[field.key] = field.type === 'switch' ? false : undefined;
    }
  },
);

watch(
  () => props.fields,
  (fields) => {
    for (const field of fields) {
      if (!(field.key in data)) {
        data[field.key] = field.type === 'switch' ? false : undefined;
      }
    }
  },
  { immediate: true },
);

function optionsOf(field: FormFieldDto): { label: string; value: string }[] {
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

function readonlyField(field: FormFieldDto): boolean {
  return field.permission !== 'E';
}

function onOpenChange(value: boolean) {
  emit('update:open', value);
}
</script>

<template>
  <Modal
    :open="open"
    :footer="null"
    :title="$t('antflow.designer.form.previewTitle')"
    width="680px"
    @cancel="onOpenChange(false)"
  >
    <Form layout="vertical">
      <template v-for="field in fields" :key="field.key">
        <FormItem
          v-if="field.permission !== 'H'"
          :label="field.label"
          :required="field.isRequired"
        >
          <Input
            v-if="field.type === 'single-line-text'"
            v-model:value="data[field.key]"
            :disabled="readonlyField(field)"
          />
          <Input.TextArea
            v-else-if="field.type === 'multi-line-text'"
            v-model:value="data[field.key]"
            :disabled="readonlyField(field)"
            :rows="3"
          />
          <InputNumber
            v-else-if="field.type === 'number'"
            v-model:value="data[field.key]"
            :disabled="readonlyField(field)"
            class="w-full"
          />
          <DatePicker
            v-else-if="field.type === 'date'"
            v-model:value="data[field.key]"
            :disabled="readonlyField(field)"
            class="w-full"
            value-format="YYYY-MM-DD"
          />
          <DatePicker.RangePicker
            v-else-if="field.type === 'date-range'"
            v-model:value="data[field.key]"
            :disabled="readonlyField(field)"
            class="w-full"
            value-format="YYYY-MM-DD"
          />
          <Select
            v-else-if="field.type === 'select'"
            v-model:value="data[field.key]"
            :disabled="readonlyField(field)"
            :options="optionsOf(field)"
          />
          <Select
            v-else-if="field.type === 'multiple-select'"
            v-model:value="data[field.key]"
            mode="multiple"
            :disabled="readonlyField(field)"
            :options="optionsOf(field)"
          />
          <RadioGroup
            v-else-if="field.type === 'radio'"
            v-model:value="data[field.key]"
            :disabled="readonlyField(field)"
            :options="optionsOf(field)"
          />
          <Switch
            v-else-if="field.type === 'switch'"
            v-model:checked="data[field.key]"
            :disabled="readonlyField(field)"
          />
          <Input
            v-else-if="field.type === 'attachment'"
            disabled
            :placeholder="$t('antflow.designer.form.attachmentPlaceholder')"
          />
          <span v-else-if="field.type === 'description'" class="text-gray-500">
            {{ field.hint ?? field.label }}
          </span>
          <Input v-else v-model:value="data[field.key]" />
        </FormItem>
      </template>
    </Form>

    <pre class="mt-3 max-h-60 overflow-auto rounded bg-gray-50 p-3 text-xs">{{
      previewJson
    }}</pre>
  </Modal>
</template>
