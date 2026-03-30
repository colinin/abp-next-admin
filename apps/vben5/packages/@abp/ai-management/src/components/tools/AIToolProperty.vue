<script setup lang="ts">
import type { PropertyInfo } from '@abp/ui';

import type { AIToolPropertyDescriptorDto } from '../../types/tools';

import { PropertyTable } from '@abp/ui';
import {
  Checkbox,
  FormItemRest,
  Input,
  InputNumber,
  Select,
} from 'ant-design-vue';

const props = defineProps<{
  model: Record<string, any>;
  property: AIToolPropertyDescriptorDto;
}>();
const emit = defineEmits<{
  (event: 'change', data: Record<string, any>): void;
  (event: 'update:value', data: Record<string, any>): void;
}>();
const onValueChange = (value?: any) => {
  const prop = props.model;
  prop[props.property.name] = value;
  emit('change', prop);
  emit('update:value', prop);
};
const onPropChange = (propertyInfo: PropertyInfo) => {
  const prop = props.model;
  prop[props.property.name] ??= {};
  prop[props.property.name][propertyInfo.key] = propertyInfo.value;
  emit('change', prop);
  emit('update:value', prop);
};
const onPropDelete = (propertyInfo: PropertyInfo) => {
  const prop = props.model;
  prop[props.property.name] ??= {};
  delete prop[props.property.name][propertyInfo.key];
  emit('change', prop);
  emit('update:value', prop);
};
</script>

<template>
  <InputNumber
    v-if="property.valueType === 'Number'"
    class="w-full"
    :min="0"
    :value="model[property.name]"
    @change="onValueChange($event)"
  />
  <Input
    v-if="property.valueType === 'String'"
    class="w-full"
    :min="0"
    :value="model[property.name]"
    @change="onValueChange($event.target.value)"
  />
  <Checkbox
    v-else-if="property.valueType === 'Boolean'"
    :checked="model[property.name] === true"
    @change="onValueChange($event.target.checked)"
  >
    {{ property.displayName }}
  </Checkbox>
  <Select
    v-else-if="property.valueType === 'Select'"
    class="w-full"
    :options="property.options"
    :field-names="{ label: 'name', value: 'value' }"
    :value="model[property.name]"
    @change="onValueChange($event)"
  />
  <FormItemRest v-else-if="property.valueType === 'Dictionary'">
    <PropertyTable
      :data="model[property.name]"
      @change="onPropChange"
      @delete="onPropDelete"
    />
  </FormItemRest>
</template>

<style scoped></style>
