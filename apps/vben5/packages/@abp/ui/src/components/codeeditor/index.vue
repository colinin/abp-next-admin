<script lang="ts" setup>
import type { PropType } from 'vue';
import { computed } from 'vue';

import { isString } from '@vben-core/shared/utils';

import CodeMirror from '../codemirror/CodeMirror.vue';
import { MODE } from '../codemirror/types';

const props = defineProps({
  autoFormat: { default: true, type: Boolean },
  mode: {
    default: MODE.JSON,
    type: String as PropType<MODE>,
    validator(value: any) {
      // 这个值必须匹配下列字符串中的一个
      return Object.values(MODE).includes(value);
    },
  },
  readonly: { type: Boolean },
  value: { default: '', type: [String, Object] },
});

const emit = defineEmits<{
  (event: 'change', value: string): void;
  (event: 'formatError', error: string): void;
  (event: 'update:value', value: string): void;
}>();

const getValue = computed(() => {
  const { autoFormat, mode, value } = props;
  if (!autoFormat || mode !== MODE.JSON) {
    return value as string;
  }
  let result = value;
  if (isString(value)) {
    try {
      result = JSON.parse(value);
    } catch {
      emit('formatError', value);
      return value as string;
    }
  }
  return JSON.stringify(result, null, 2);
});

function handleValueChange(v: string) {
  emit('update:value', v);
  emit('change', v);
}
</script>
<template>
  <div class="h-full">
    <CodeMirror
      :mode="mode"
      :readonly="readonly"
      :value="getValue"
      @change="handleValueChange"
    />
  </div>
</template>
