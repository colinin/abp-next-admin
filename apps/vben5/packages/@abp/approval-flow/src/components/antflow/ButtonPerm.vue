<script setup lang="ts">
import { computed, ref, watch } from 'vue';

import { Checkbox, CheckboxGroup } from 'ant-design-vue';

interface ButtonOption {
  description: string;
  label: string;
  locked?: boolean;
  value: number;
}

const props = withDefaults(
  defineProps<{
    modelValue?: null | { approvalPage?: number[] };
    readonly?: boolean;
  }>(),
  {
    modelValue: null,
    readonly: false,
  },
);

const emit = defineEmits<{
  'update:modelValue': [{ approvalPage: number[] }];
}>();

const options: ButtonOption[] = [
  {
    description: '审批通过，流转到下一个节点',
    label: '同意',
    locked: true,
    value: 3,
  },
  {
    description: '当不同意任务时，当前任务被终止，并结束整个流程',
    label: '不同意',
    locked: true,
    value: 4,
  },
  {
    description: '退回到发起人或任意节点，流程重新开始或者回到当前审批人',
    label: '退回',
    value: 18,
  },
  {
    description: '转办选择审批人，转办后自己将不再进行审批',
    label: '转办',
    value: 21,
  },
  {
    description: '在当前任务上额外添加新人员，处理相关事项或提供审批意见',
    label: '加批',
    value: 19,
  },
];

const lockedValues = options
  .filter((item) => item.locked)
  .map((item) => item.value);

const selected = ref<number[]>([...lockedValues]);

const checked = computed(() =>
  [...new Set([...lockedValues, ...selected.value])].toSorted((a, b) => a - b),
);

watch(
  () => props.modelValue,
  (value) => {
    selected.value = Array.isArray(value?.approvalPage)
      ? [...value.approvalPage]
      : [];
  },
  { immediate: true },
);

function update(value: Array<boolean | number | string>) {
  selected.value = value.map(Number).filter((item) => Number.isFinite(item));
  emit('update:modelValue', { approvalPage: checked.value });
}
</script>

<template>
  <section class="button-perm">
    <header class="panel-head">
      <h3>【审批页面】按钮权限显示控制</h3>
      <p>可控制审批节点中按钮的显示权限，默认按钮不可取消。</p>
    </header>

    <CheckboxGroup
      :disabled="readonly"
      :value="checked"
      class="perm-list"
      @change="update"
    >
      <Checkbox
        v-for="option in options"
        :key="option.value"
        :disabled="readonly || option.locked"
        :value="option.value"
        class="perm-item"
      >
        <span class="perm-label">【{{ option.label }}】</span>
        <span class="perm-desc">{{ option.description }}</span>
      </Checkbox>
    </CheckboxGroup>
  </section>
</template>

<style scoped>
.button-perm {
  padding: 16px;
  background: #fff;
  border: 1px solid #ebf0f8;
  border-radius: 8px;
}

.panel-head {
  margin-bottom: 14px;
}

.panel-head h3 {
  margin: 0;
  font-size: 16px;
  font-weight: 700;
  color: #24324a;
}

.panel-head p {
  margin: 8px 0 0;
  font-size: 13px;
  color: #6b7a90;
}

.perm-list {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.perm-item {
  display: flex;
  align-items: flex-start;
  min-height: 44px;
  padding: 10px 12px;
  margin-inline-end: 0;
  background: #f8fbff;
  border: 1px solid #d9e3f4;
  border-radius: 8px;
}

.perm-item :deep(.ant-checkbox + span) {
  display: inline-flex;
  flex-wrap: wrap;
  gap: 6px;
  align-items: center;
}

.perm-label {
  font-size: 14px;
  font-weight: 600;
  color: #1f6fe5;
}

.perm-desc {
  font-size: 12px;
  line-height: 1.45;
  color: #4a5973;
}
</style>
