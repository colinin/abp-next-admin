<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';
import type { RuleObject } from 'ant-design-vue/es/form';

import { computed, reactive, ref, watch } from 'vue';

import { $t } from '@vben/locales';

import { AutoComplete, Form, FormItem, Input } from 'ant-design-vue';

/**
 * Basic setting payload of an approval template (AntFlow step 1).
 *
 * The field set mirrors the backend `ApprovalTemplateDto`:
 * `name` / `displayName` / `category` / `icon` / `businessKeyHint` / `description`.
 * `id` is optional so an already persisted template can disable the code field
 * without the caller having to pass a second flag.
 *
 * Every field is normalised to a string locally (the API accepts `null`, the
 * wizard maps empty strings back to `null`), which keeps the antdv `v-model`
 * bindings free of nullable value types.
 */
interface BasicSettingModel {
  businessKeyHint: string;
  category: string;
  description: string;
  displayName: string;
  icon: string;
  id?: null | string;
  name: string;
}

const props = withDefaults(
  defineProps<{
    /** AntFlow compatible alias of `modelValue`. */
    basic?: BasicSettingModel | null;
    categories?: string[];
    disabled?: boolean;
    /** Disables the flow code (`name`) because the template already exists. */
    isEdit?: boolean;
    modelValue?: BasicSettingModel | null;
  }>(),
  {
    basic: null,
    categories: () => [],
    disabled: false,
    isEdit: false,
    modelValue: null,
  },
);

const emit = defineEmits<{
  update: [BasicSettingModel];
  'update:basic': [BasicSettingModel];
  'update:modelValue': [BasicSettingModel];
}>();

const formRef = ref<FormInstance | null>(null);
const form = reactive<BasicSettingModel>(createEmpty());

const rules: Record<string, RuleObject[]> = {
  displayName: [
    {
      message: $t('antflow.basic.nameRequired'),
      required: true,
      trigger: 'blur',
    },
  ],
  name: [
    {
      message: $t('antflow.basic.codeRequired'),
      required: true,
      trigger: 'blur',
    },
  ],
};

const categoryOptions = computed(() =>
  props.categories.filter(Boolean).map((value) => ({ value })),
);

const isReadonly = computed(() => props.disabled);

const codeDisabled = computed(
  () => props.disabled || props.isEdit || !!form.id,
);

const iconPreview = computed(() => {
  const icon = (form.icon ?? '').trim();
  return icon.length > 0 ? icon.slice(0, 2) : '★';
});

watch(
  () => props.modelValue ?? props.basic,
  (value) => {
    const next = normalize(value);
    // The parent echoes our own `update` events back through the prop, so only
    // assign when the payload really changed to avoid a feedback loop.
    if (JSON.stringify(next) !== JSON.stringify(normalize(form))) {
      Object.assign(form, next);
    }
  },
  { deep: true, immediate: true },
);

watch(
  form,
  () => {
    const value = normalize(form);
    emit('update', { ...value });
    emit('update:basic', { ...value });
    emit('update:modelValue', { ...value });
  },
  { deep: true },
);

function createEmpty(): BasicSettingModel {
  return {
    businessKeyHint: '',
    category: '',
    description: '',
    displayName: '',
    icon: '',
    name: '',
  };
}

function normalize(value?: BasicSettingModel | null): BasicSettingModel {
  return {
    ...createEmpty(),
    ...value,
    businessKeyHint: value?.businessKeyHint ?? '',
    category: value?.category ?? '',
    description: value?.description ?? '',
    displayName: value?.displayName ?? '',
    icon: value?.icon ?? '',
    name: value?.name ?? '',
  };
}

async function validate(): Promise<boolean> {
  try {
    await formRef.value?.validate();
    return true;
  } catch {
    return false;
  }
}

function getModel(): BasicSettingModel {
  return normalize(form);
}

function reset(value?: BasicSettingModel | null) {
  Object.assign(form, normalize(value));
}

defineExpose({ getModel, reset, validate });
</script>

<template>
  <section class="basic-setting-page">
    <div class="basic-setting-wrap">
      <Form
        ref="formRef"
        class="setting-form"
        :label-col="{ span: 5 }"
        :model="form"
        :rules="rules"
        :wrapper-col="{ span: 19 }"
      >
        <FormItem :label="$t('antflow.basic.codeLabel')" name="name">
          <Input
            v-model:value="form.name"
            :disabled="codeDisabled"
            :maxlength="50"
            :placeholder="$t('antflow.basic.codePlaceholder')"
          >
            <template #suffix>
              <span class="field-suffix">{{
                codeDisabled
                  ? $t('antflow.basic.fixed')
                  : $t('antflow.basic.unique')
              }}</span>
            </template>
          </Input>
        </FormItem>

        <FormItem :label="$t('antflow.basic.nameLabel')" name="displayName">
          <Input
            v-model:value="form.displayName"
            :disabled="isReadonly"
            :maxlength="20"
            :placeholder="$t('antflow.basic.namePlaceholder')"
          />
        </FormItem>

        <FormItem :label="$t('antflow.basic.categoryLabel')" name="category">
          <AutoComplete
            v-model:value="form.category"
            :disabled="isReadonly"
            :options="categoryOptions"
            :placeholder="$t('antflow.basic.categoryPlaceholder')"
          >
            <template #notFoundContent>
              <span class="empty-tip">{{
                $t('antflow.basic.categoryTip')
              }}</span>
            </template>
          </AutoComplete>
        </FormItem>

        <FormItem :label="$t('antflow.basic.iconLabel')" name="icon">
          <div class="icon-row">
            <span class="icon-preview">{{ iconPreview }}</span>
            <Input
              v-model:value="form.icon"
              class="icon-input"
              :disabled="isReadonly"
              :maxlength="64"
              :placeholder="$t('antflow.basic.iconPlaceholder')"
            />
          </div>
        </FormItem>

        <FormItem
          :label="$t('antflow.basic.businessKeyLabel')"
          name="businessKeyHint"
        >
          <Input
            v-model:value="form.businessKeyHint"
            :disabled="isReadonly"
            :maxlength="100"
            :placeholder="$t('antflow.basic.businessKeyPlaceholder')"
          />
        </FormItem>

        <FormItem
          class="is-textarea"
          :label="$t('antflow.basic.descriptionLabel')"
          name="description"
        >
          <Input.TextArea
            v-model:value="form.description"
            :disabled="isReadonly"
            :maxlength="200"
            :rows="4"
            :placeholder="$t('antflow.basic.descriptionPlaceholder')"
            show-count
          />
        </FormItem>
      </Form>
    </div>
  </section>
</template>

<style scoped>
.basic-setting-page {
  min-height: 100%;
  padding: 0 64px;
}

.basic-setting-wrap {
  box-sizing: border-box;
  width: 100%;
  min-width: 320px;
  max-width: 760px;
  min-height: calc(100vh - 140px);
  padding: 26px 64px;
  margin: 0 auto;
  background: #fff;
}

.setting-form {
  max-width: 540px;
  margin: 0 auto;
}

.setting-form :deep(.ant-form-item) {
  margin-bottom: 18px;
}

.setting-form :deep(.ant-form-item-label > label) {
  font-size: 14px;
  color: #4f5b6f;
}

.setting-form :deep(.ant-input),
.setting-form :deep(.ant-input-affix-wrapper),
.setting-form :deep(.ant-select-selector) {
  background: #fff;
  border-color: #d9dde5;
  border-radius: 2px;
}

.setting-form :deep(.ant-input:disabled),
.setting-form :deep(.ant-input-affix-wrapper-disabled),
.setting-form :deep(.ant-input-disabled) {
  color: #6b7a90;
  background: #eef1f5;
  border-color: #d9dde5;
}

.setting-form :deep(.ant-input-data-count) {
  color: #7d8797;
}

.setting-form :deep(.ant-form-item-required::before) {
  color: #ff4d4f;
}

.icon-row {
  display: flex;
  gap: 10px;
  align-items: center;
}

.icon-input {
  flex: 1;
}

.icon-preview {
  display: inline-flex;
  flex-shrink: 0;
  align-items: center;
  justify-content: center;
  width: 30px;
  height: 30px;
  font-size: 15px;
  color: #fff;
  background: #45c97a;
  border-radius: 4px;
}

.field-suffix {
  font-size: 12px;
  color: #9aa4b2;
}

.empty-tip {
  font-size: 12px;
  color: #9aa4b2;
}

@media (max-width: 900px) {
  .basic-setting-page {
    padding: 0 10px;
  }

  .basic-setting-wrap {
    padding: 16px 12px;
  }

  .setting-form {
    max-width: 100%;
  }
}
</style>
