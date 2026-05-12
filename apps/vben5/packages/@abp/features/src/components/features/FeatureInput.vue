<script setup lang="ts">
import type { Validator } from '@abp/core';

import type { FeatureDto } from '../../types/features';

import { computed } from 'vue';

import { useValidation } from '@abp/core';
import { Checkbox, Form, Input, InputNumber, Select } from 'ant-design-vue';

const props = defineProps<{
  feature: FeatureDto;
  featureIndex: number;
  groupIndex: number;
}>();

const emit = defineEmits<{
  (event: 'change', feature: FeatureDto, groupIndex: number): void;
}>();

const FormItem = Form.Item;

const {
  fieldMustBeetWeen,
  fieldMustBeStringWithMinimumLengthAndMaximumLength,
  fieldRequired,
} = useValidation();

function createRules(field: string, validator: Validator) {
  const featureRules: any[] = [];
  if (validator?.properties) {
    switch (validator.name) {
      case 'NUMERIC': {
        featureRules.push(
          ...fieldMustBeetWeen({
            end: Number(validator.properties.MaxValue),
            name: field,
            start: Number(validator.properties.MinValue),
            trigger: 'change',
          }),
        );
        break;
      }
      case 'STRING': {
        if (validator.properties.AllowNull?.toLowerCase() === 'true') {
          featureRules.push(
            ...fieldRequired({
              name: field,
              trigger: 'blur',
            }),
          );
        }
        featureRules.push(
          ...fieldMustBeStringWithMinimumLengthAndMaximumLength({
            maximum: Number(validator.properties.MaxLength),
            minimum: Number(validator.properties.MinLength),
            name: field,
            trigger: 'blur',
          }),
        );
        break;
      }
    }
  }
  return featureRules;
}

function handleChange() {
  emit('change', props.feature, props.groupIndex);
}

const isCheckbox = computed(
  () =>
    props.feature.valueType?.name === 'ToggleStringValueType' &&
    props.feature.valueType?.validator?.name === 'BOOLEAN',
);

const isFreeText = computed(
  () => props.feature.valueType?.name === 'FreeTextStringValueType',
);

const isNumeric = computed(
  () => props.feature.valueType?.validator?.name === 'NUMERIC',
);

const isSelection = computed(
  () => props.feature.valueType?.name === 'SelectionStringValueType',
);
</script>

<template>
  <FormItem
    v-if="feature.valueType !== null"
    :name="['groups', groupIndex, 'features', featureIndex, 'value']"
    :label="feature.displayName"
    :extra="feature.description"
    :rules="createRules(feature.displayName, feature.valueType.validator)"
  >
    <!-- 复选框类型 -->
    <Checkbox
      v-if="isCheckbox"
      v-model:checked="feature.value"
      @change="handleChange"
    >
      {{ feature.displayName }}
    </Checkbox>

    <!-- 文本/数字类型 -->
    <div v-else-if="isFreeText">
      <InputNumber
        v-if="isNumeric"
        style="width: 100%"
        v-model:value="feature.value"
        @change="handleChange"
      />
      <Input
        v-else
        v-model:value="feature.value"
        autocomplete="off"
        @change="handleChange"
      />
    </div>

    <!-- 选择类型 -->
    <Select
      v-else-if="isSelection"
      v-model:value="feature.value"
      :options="feature.valueType.itemSource?.items"
      :field-names="{ label: 'displayName', value: 'value' }"
      @change="handleChange"
    />
  </FormItem>
</template>
