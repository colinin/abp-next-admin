<!-- eslint-disable prettier/prettier -->
<script setup lang="ts">
import type {
  ApprovalTemplateDto,
  FormFieldDto,
} from '../../types/approval-flow';

import { computed, reactive, ref, watch } from 'vue';

import { $t } from '@vben/locales';

import {
  DatePicker,
  Form,
  FormItem,
  Input,
  InputNumber,
  message,
  Modal,
  RadioGroup,
  Select,
  Switch,
} from 'ant-design-vue';

import { useApprovalFormsApi } from '../../api/useApprovalFormsApi';
import { useApprovalInstancesApi } from '../../api/useApprovalInstancesApi';
import { calculateFormula, parseFormula } from '../designer/formula';

const props = defineProps<{
  initialData?: null | Record<string, unknown>;
  open: boolean;
  template?: ApprovalTemplateDto | null;
}>();

const emit = defineEmits<{
  started: [];
  'update:open': [boolean];
}>();

const instancesApi = useApprovalInstancesApi();
const formsApi = useApprovalFormsApi();

const fields = ref<FormFieldDto[]>([]);
const data = reactive<Record<string, any>>({});
const title = ref('');
const businessKey = ref('');
const loading = ref(false);
const submitting = ref(false);

const useForm = computed(() => !!props.template?.formDefinitionId);

const calculatedValues = computed(() => {
  const values: Record<string, null | number | string> = {};
  for (const field of fields.value) {
    if (field.type === 'calculated') {
      values[field.key] = calculateFormula(
        parseFormula(field.optionsJson),
        data,
      );
    }
  }
  return values;
});

watch(
  () => props.open,
  async (open) => {
    if (!open) {
      return;
    }

    title.value = props.template?.displayName ?? '';
    businessKey.value = '';
    for (const key of Object.keys(data)) {
      delete data[key];
    }
    fields.value = [];

    if (props.template?.formDefinitionId) {
      loading.value = true;
      try {
        const form = await formsApi.getApi(props.template.formDefinitionId);
        fields.value = form.fields ?? [];
        for (const field of fields.value) {
          const initial = props.initialData?.[field.key];
          data[field.key] =
            initial === undefined
              ? (field.type === 'switch'
                ? false
                : undefined)
              : initial;
        }
      } catch {
        message.error($t('ElsaNextApprovalFlow.ApprovalFlow:Start:FormLoadFailed'));
      } finally {
        loading.value = false;
      }
    }
  },
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
  return field.permission === 'R';
}

async function submit() {
  if (!props.template) {
    return;
  }
  if (!title.value.trim()) {
    message.warning($t('ElsaNextApprovalFlow.ApprovalFlow:Start:TitleRequired'));
    return;
  }

  for (const field of fields.value) {
    if (field.permission === 'H') {
      continue;
    }
    if (field.type === 'calculated') {
      continue;
    }
    const value = data[field.key];
    if (
      field.isRequired &&
      (value === undefined || value === null || value === '')
    ) {
      message.warning($t('ElsaNextApprovalFlow.ApprovalFlow:Start:FieldRequired', [field.label]));
      return;
    }
  }

  submitting.value = true;
  try {
    const payload: Record<string, unknown> = {};
    for (const field of fields.value) {
      if (field.permission === 'H') {
        continue;
      }
      if (field.type === 'calculated') {
        payload[field.key] = calculatedValues.value[field.key];
      } else if (data[field.key] !== undefined) {
        payload[field.key] = data[field.key];
      }
    }

    await instancesApi.startApi({
      businessKey: businessKey.value || null,
      formDataJson: useForm.value ? JSON.stringify(payload) : null,
      templateId: props.template.id,
      title: title.value.trim(),
    });
    message.success($t('ElsaNextApprovalFlow.ApprovalFlow:Start:Started'));
    emit('started');
    emit('update:open', false);
  } catch {
    message.error($t('ElsaNextApprovalFlow.ApprovalFlow:Start:Failed'));
  } finally {
    submitting.value = false;
  }
}
</script>

<template>
  <Modal
    :confirm-loading="submitting"
    :open="open"
    :title="`${$t('ElsaNextApprovalFlow.ApprovalFlow:Start:Title')}: ${template?.displayName ?? ''}`"
    :ok-text="$t('ElsaNextApprovalFlow.ApprovalFlow:Common:Start')"
    width="680px"
    @cancel="emit('update:open', false)"
    @ok="submit"
  >
    <Form layout="vertical">
      <FormItem
        :label="$t('ElsaNextApprovalFlow.ApprovalFlow:Start:ApprovalTitle')"
        required
      >
        <Input
          v-model:value="title"
          :placeholder="
            $t(
              'ElsaNextApprovalFlow.ApprovalFlow:Start:ApprovalTitlePlaceholder',
            )
          "
        />
      </FormItem>
      <FormItem
        v-if="template?.businessKeyHint"
        :label="template.businessKeyHint"
      >
        <Input
          v-model:value="businessKey"
          :placeholder="
            $t('ElsaNextApprovalFlow.ApprovalFlow:Start:BusinessKeyPlaceholder')
          "
        />
      </FormItem>

      <template v-if="useForm">
        <template v-for="field in fields" :key="field.key">
          <FormItem
            v-if="field.permission !== 'H' && field.type !== 'description'"
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
              :disabled="readonlyField(field)"
              mode="multiple"
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
              v-else-if="field.type === 'calculated'"
              :value="calculatedValues[field.key] ?? ''"
              disabled
            />
            <Input
              v-else
              v-model:value="data[field.key]"
              :disabled="readonlyField(field)"
            />
          </FormItem>
        </template>
      </template>
      <div v-else class="text-sm text-gray-500">
        {{ $t('ElsaNextApprovalFlow.ApprovalFlow:Start:NoForm') }}
      </div>
    </Form>
  </Modal>
</template>
