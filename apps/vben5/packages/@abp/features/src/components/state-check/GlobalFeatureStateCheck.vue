<script setup lang="ts">
import { computed } from 'vue';

import { useAbpStore } from '@abp/core';
import { Checkbox, FormItemRest, Select } from 'ant-design-vue';

const emits = defineEmits(['blur', 'change']);

const modelValue = defineModel<{
  globalFeatureNames: string[];
  requiresAll: boolean;
}>({
  default: {
    globalFeatureNames: [],
    requiresAll: false,
  },
});

const abpStore = useAbpStore();

const getFeatureOptions = computed(() => {
  if (!abpStore.application?.globalFeatures) {
    return [];
  }
  return abpStore.application.globalFeatures.enabledFeatures.map((feature) => {
    return {
      label: feature,
      value: feature,
    };
  });
});

function onChange() {
  emits('change', modelValue.value);
}

function onFeaturesChange(value: any) {
  modelValue.value.globalFeatureNames = value;
  emits('change', modelValue.value);
}
</script>

<template>
  <div class="flex w-full flex-col gap-5">
    <FormItemRest>
      <Checkbox
        v-model:checked="modelValue.requiresAll"
        @blur="emits('blur')"
        @change="onChange"
      >
        {{ $t('component.simple_state_checking.requireFeatures.requiresAll') }}
      </Checkbox>
      <Select
        :value="modelValue.globalFeatureNames"
        :options="getFeatureOptions"
        show-search
        mode="tags"
        @blur="emits('blur')"
        @change="onFeaturesChange"
      />
    </FormItemRest>
  </div>
</template>

<style scoped></style>
