<script lang="ts" setup>
import type { Nullable } from '@vben-core/typings';

import type { PropType } from 'vue';
import {
  nextTick,
  onMounted,
  onUnmounted,
  ref,
  unref,
  watch,
  watchEffect,
} from 'vue';

import { usePreferences } from '@vben-core/preferences';

import { useWindowSizeFn } from '@abp/core';
import { useDebounceFn } from '@vueuse/core';
import CodeMirror from 'codemirror';

import { MODE } from './types';

// css
import './codemirror.css';
import 'codemirror/theme/idea.css';
import 'codemirror/theme/material-palenight.css';

// modes
import 'codemirror/mode/javascript/javascript';
import 'codemirror/mode/css/css';
import 'codemirror/mode/htmlmixed/htmlmixed';

const props = defineProps({
  mode: {
    default: MODE.JSON,
    type: String as PropType<MODE>,
    validator(value: any) {
      // 这个值必须匹配下列字符串中的一个
      return Object.values(MODE).includes(value);
    },
  },
  readonly: { default: false, type: Boolean },
  value: { default: '', type: String },
});

const emit = defineEmits(['change']);

const el = ref();
let editor: Nullable<CodeMirror.Editor>;

const debounceRefresh = useDebounceFn(refresh, 100);

const { theme } = usePreferences();

watch(
  () => props.value,
  async (value) => {
    await nextTick();
    const oldValue = editor?.getValue();
    if (value !== oldValue) {
      const jsonVal = value ? JSON.stringify(JSON.parse(value), null, 2) : '';
      editor?.setValue(jsonVal || '');
      setTimeout(refresh, 50);
    }
  },
  { flush: 'post' },
);

watchEffect(() => {
  editor?.setOption('mode', props.mode);
});

watch(
  () => theme.value,
  async () => {
    setTheme();
  },
  {
    deep: true,
    immediate: true,
  },
);

function setTheme() {
  unref(editor)?.setOption(
    'theme',
    theme.value === 'light' ? 'idea' : 'material-palenight',
  );
}

function refresh() {
  editor?.refresh();
}

async function init() {
  const addonOptions = {
    autoCloseBrackets: true,
    autoCloseTags: true,
    foldGutter: true,
    gutters: ['CodeMirror-linenumbers'],
  };

  editor = CodeMirror(el.value!, {
    fixedGutter: true,
    lineNumbers: true,
    lineWrapping: true,
    mode: props.mode,
    readOnly: props.readonly,
    smartIndent: true,
    tabSize: 2,
    theme: 'material-palenight',
    value: '',
    ...addonOptions,
  });
  editor?.setValue(props.value);
  setTheme();
  editor?.on('change', () => {
    emit('change', editor?.getValue());
  });
}

onMounted(async () => {
  await nextTick();
  init();
  useWindowSizeFn<void>(debounceRefresh);
});

onUnmounted(() => {
  editor = null;
});
</script>

<template>
  <div ref="el" class="relative !h-full w-full overflow-hidden"></div>
</template>
