<script lang="ts" setup>
import type { Editor, RawEditorSettings } from 'tinymce';

import type { PropType } from 'vue';

import type { Nullable, Recordable } from '@vben/types';

import {
  computed,
  nextTick,
  onActivated,
  onBeforeUnmount,
  onDeactivated,
  onMounted,
  ref,
  unref,
  useAttrs,
  watch,
} from 'vue';

import { preferences } from '@vben/preferences';

import isNumber from 'lodash.isnumber';
import tinymce from 'tinymce/tinymce';

import { bindHandlers } from './helper';
import {
  plugins as defaultPlugins,
  toolbar as defaultToolbar,
} from './tinymce';
import { useTinymce } from './useTinymce';

import 'tinymce/themes/silver';
import 'tinymce/icons/default/icons';
import 'tinymce/plugins/advlist';
import 'tinymce/plugins/anchor';
import 'tinymce/plugins/autolink';
import 'tinymce/plugins/autosave';
import 'tinymce/plugins/code';
import 'tinymce/plugins/codesample';
import 'tinymce/plugins/directionality';
import 'tinymce/plugins/fullscreen';
import 'tinymce/plugins/hr';
import 'tinymce/plugins/insertdatetime';
import 'tinymce/plugins/link';
import 'tinymce/plugins/lists';
import 'tinymce/plugins/media';
import 'tinymce/plugins/nonbreaking';
import 'tinymce/plugins/noneditable';
import 'tinymce/plugins/pagebreak';
import 'tinymce/plugins/paste';
import 'tinymce/plugins/preview';
import 'tinymce/plugins/print';
import 'tinymce/plugins/save';
import 'tinymce/plugins/searchreplace';
import 'tinymce/plugins/spellchecker';
import 'tinymce/plugins/tabfocus';
// import 'tinymce/plugins/table';
import 'tinymce/plugins/template';
import 'tinymce/plugins/textpattern';
import 'tinymce/plugins/visualblocks';
import 'tinymce/plugins/visualchars';
import 'tinymce/plugins/wordcount';

defineOptions({
  name: 'Tinymce',
});

const props = defineProps({
  height: {
    default: 400,
    required: false,
    type: [Number, String] as PropType<number | string>,
  },
  menubar: {
    default: 'file edit insert view format table',
    type: String,
  },
  modelValue: {
    default: '',
    type: String,
  },
  options: {
    default: () => ({}),
    type: Object as PropType<Partial<RawEditorSettings>>,
  },
  plugins: {
    default: defaultPlugins,
    type: Array as PropType<string[]>,
  },
  readonly: {
    default: false,
    type: Boolean,
  },
  toolbar: {
    default: defaultToolbar,
    type: Array as PropType<string[]>,
  },
  value: {
    default: '',
    type: String,
  },
  width: {
    default: 'auto',
    required: false,
    type: [Number, String] as PropType<number | string>,
  },
});
const emits = defineEmits<{
  (event: 'change', content: any): void;
  (event: 'update:modelValue', content: any): void;
  (event: 'inited', editor: Editor | Editor[]): void;
  (event: 'initError', error: any): void;
}>();
const { buildShortUUID } = useTinymce();
const attrs = useAttrs();

let mounted = false;
const editorRef = ref<Nullable<Editor>>(null);
const fullscreen = ref(false);
const tinymceId = ref<string>(buildShortUUID('tiny-vue'));
const elRef = ref<Nullable<HTMLElement>>(null);

const containerWidth = computed(() => {
  const width = props.width;
  if (isNumber(width)) {
    return `${width}px`;
  }
  return width;
});

const skinName = computed(() => {
  return preferences.theme.mode === 'light' ? 'oxide' : 'oxide-dark';
});

const langName = computed(() => {
  return preferences.app.locale;
});

const initOptions = computed((): RawEditorSettings => {
  const { height, menubar, options, plugins, toolbar } = props;
  const publicPath = import.meta.env.VITE_BASE || '/';
  return {
    auto_focus: true,
    branding: false,
    content_css: `${publicPath}resource/tinymce/skins/ui/${
      skinName.value
    }/content.min.css`,
    default_link_target: '_blank',
    height,
    language: langName.value,
    language_url: `${publicPath}resource/tinymce/langs/${langName.value}.js`,
    link_title: false,
    menubar, // 菜单栏配置
    object_resizing: false,
    plugins,
    selector: `#${unref(tinymceId)}`,
    skin: skinName.value,
    skin_url: `${publicPath}resource/tinymce/skins/ui/${skinName.value}`,
    toolbar, // 工具栏
    ...options,
    setup: (editor: Editor) => {
      editorRef.value = editor;
      editor.on('init', (e) => initSetup(e));
    },
  };
});

function resetEditor() {
  if (!initOptions.value.inline) {
    tinymceId.value = buildShortUUID('tiny-vue');
  }
  nextTick(() => {
    setTimeout(() => {
      initEditor();
      mounted = true;
    }, 30);
  });
}

onMounted(resetEditor);

onActivated(() => {
  if (mounted) {
    resetEditor();
  }
});

onBeforeUnmount(() => {
  destory();
});

onDeactivated(() => {
  destory();
});

function destory() {
  if (tinymce !== null) {
    tinymce?.remove?.(unref(initOptions).selector!);
  }
}

function initEditor() {
  const el = unref(elRef);
  if (el) {
    el.style.visibility = '';
  }
  tinymce
    .init(unref(initOptions))
    .then((editor) => {
      emits('inited', editor);
    })
    .catch((error) => {
      emits('initError', error);
    });
}

function initSetup(e: any) {
  const editor = unref(editorRef);
  if (!editor) {
    return;
  }
  const value = props.modelValue || '';

  editor.setContent(value);
  bindModelHandlers(editor);
  bindHandlers(e, attrs, unref(editorRef));
}

function setValue(editor: Recordable<any>, val?: string, prevVal?: string) {
  if (
    editor &&
    typeof val === 'string' &&
    val !== prevVal &&
    val !== editor.getContent({ format: attrs.outputFormat })
  ) {
    editor.setContent(val);
  }
}

function bindModelHandlers(editor: any) {
  const modelEvents = attrs.modelEvents ?? null;
  const normalizedEvents = Array.isArray(modelEvents)
    ? modelEvents.join(' ')
    : modelEvents;

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
    () => props.readonly,
    (readonly) => {
      const editor = unref(editorRef);
      if (!editor) {
        return;
      }
      editor.setMode(readonly ? 'readonly' : 'design');
    },
    {
      immediate: true,
    },
  );

  editor.on(normalizedEvents || 'change keyup undo redo', () => {
    const content = editor.getContent({ format: attrs.outputFormat });
    emits('update:modelValue', content);
    emits('change', content);
  });

  editor.on('FullscreenStateChanged', (e: any) => {
    fullscreen.value = e.state;
  });
}
</script>

<template>
  <div class="tinymce-container" :style="{ width: containerWidth }">
    <textarea
      :id="tinymceId"
      ref="elRef"
      :style="{ visibility: 'hidden' }"
      v-if="!initOptions.inline"
    ></textarea>
    <slot v-else></slot>
  </div>
</template>

<style lang="less" scoped>
.tinymce-container {
  position: relative;
  line-height: normal;

  textarea {
    z-index: -1;
    visibility: hidden;
  }
}
</style>
