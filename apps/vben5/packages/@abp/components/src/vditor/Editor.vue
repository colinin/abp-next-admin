<script setup lang="ts">
import type { PropType } from 'vue';

import { computed, nextTick, onBeforeUnmount, onMounted, watch } from 'vue';

import { preferences } from '@vben/preferences';

import Vditor from 'vditor';

import { toolbar as defaultToolbar } from './vditor';

import 'vditor/dist/index.css';

const props = defineProps({
  cacheKey: {
    default: undefined,
    type: String,
  },
  height: {
    default: 500,
    type: Number,
  },
  mode: {
    default: 'wysiwyg',
    type: String as PropType<'ir' | 'sv' | 'wysiwyg'>,
  },
  modelValue: {
    default: '',
    type: String,
  },
  readonly: {
    default: false,
    type: Boolean,
  },
  tab: {
    default: '\t',
    type: String,
  },
  toolbar: {
    default: defaultToolbar,
    type: Array as PropType<string[]>,
  },
  typewriterMode: {
    default: false,
    type: Boolean,
  },
  value: {
    default: '',
    type: String,
  },
});
const emits = defineEmits<{
  (event: 'change', content: any): void;
  (event: 'update:modelValue', content: any): void;
}>();
let editor: Vditor;

const langName = computed(() => {
  switch (preferences.app.locale) {
    case 'zh-CN': {
      return 'zh_CN';
    }
    default: {
      return 'en_US';
    }
  }
});

const skinName = computed(() => {
  return preferences.theme.mode === 'light' ? 'classic' : 'dark';
});

function initEditor() {
  editor = new Vditor('vditor', {
    cache: {
      enable: !!props.cacheKey,
      id: props.cacheKey,
    },
    counter: {
      enable: true,
      type: 'markdown',
    },
    height: props.height,
    input(value) {
      emits('update:modelValue', value);
      emits('change', value);
    },
    lang: langName.value,
    mode: props.mode,
    preview: {
      delay: 0,
      hljs: {
        lineNumber: true,
        style: 'monokai',
      },
    },
    tab: props.tab,
    theme: skinName.value,
    toolbar: props.toolbar,
    toolbarConfig: {
      pin: true,
    },
    typewriterMode: props.typewriterMode,
    value: props.value,
  });
}

function resetEditor() {
  nextTick(() => {
    setTimeout(() => {
      initEditor();
    }, 30);
  });
}

function destory() {
  if (editor !== null) {
    editor?.destroy();
  }
}

function setValue(editor: Vditor, val?: string, prevVal?: string) {
  if (
    editor &&
    typeof val === 'string' &&
    val !== prevVal &&
    val !== editor.getValue()
  ) {
    editor.setValue(val);
  }
}

watch(
  () => props.modelValue,
  (val?: string, prevVal?: string) => {
    setValue(editor, val, prevVal);
  },
);

watch(
  () => props.value,
  (val?: string, prevVal?: string) => {
    setValue(editor, val, prevVal);
  },
  {
    immediate: true,
  },
);

watch(
  () => props.toolbar,
  () => {
    if (editor) {
      destory();
      setTimeout(resetEditor, 100);
    }
  },
);

onMounted(() => {
  resetEditor();
});

onBeforeUnmount(() => {
  destory();
});
</script>

<template>
  <div id="vditor"></div>
</template>

<style scoped></style>
