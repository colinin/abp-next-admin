<template>
  <textarea style="overflow: auto;" ref="codeEditor"></textarea>
</template>

<script lang="ts" setup>
  import { computed, ref, unref, watch, nextTick, onMounted } from 'vue';
  import CodeMirror from 'codemirror';
  import '../codemirror/codemirror.css';
  import 'codemirror/theme/idea.css';
  import 'codemirror/theme/dracula.css';

  import 'codemirror/mode/javascript/javascript'; 
  import 'codemirror/mode/htmlmixed/htmlmixed'; 
  import 'codemirror/mode/css/css';

  import 'codemirror/addon/fold/foldcode'; 
  import 'codemirror/addon/fold/foldgutter'; 
  import 'codemirror/addon/fold/foldgutter.css';
  import 'codemirror/addon/fold/brace-fold'; 
  import 'codemirror/addon/fold/xml-fold';
  import 'codemirror/addon/fold/comment-fold'; 
  import 'codemirror/addon/fold/markdown-fold'; 
  import 'codemirror/addon/fold/indent-fold'; 
  import 'codemirror/addon/edit/closebrackets'; 
  import 'codemirror/addon/edit/closetag';
  import 'codemirror/addon/edit/matchtags'; 
  import 'codemirror/addon/edit/matchbrackets';
  import 'codemirror/addon/selection/active-line';
  import "codemirror/addon/scroll/annotatescrollbar";
  import 'codemirror/addon/dialog/dialog';
  import 'codemirror/addon/dialog/dialog.css';
  import 'codemirror/addon/display/autorefresh'; 
  import 'codemirror/addon/display/placeholder'; 
  import 'codemirror/addon/selection/mark-selection';

  import { MODE } from './../typing';
  import { isString } from '/@/utils/is';
  import { useAppStore } from '/@/store/modules/app';
  import { useWindowSizeFn } from '/@/hooks/event/useWindowSizeFn';
  
  const codeEditor = ref();
  const appStore = useAppStore();
  let editor: Nullable<CodeMirror.Editor>;

  const props = defineProps({
    mode: {
      type: String as PropType<MODE>,
      default: MODE.JSON,
      validator(value: any) {
        // 这个值必须匹配下列字符串中的一个
        return Object.values(MODE).includes(value);
      },
    },
    modelValue: {
      type: [Object, String] as PropType<Record<string, any> | string>,
      default: '',
    },
    readonly: {
      type: Boolean,
      default: false,
    },
  });
  const emits = defineEmits(['update:modelValue', 'format-error']);

  const getValue = computed(() => {
    const { mode, modelValue } = props;
    if (mode !== MODE.JSON) {
      return modelValue as string;
    }
    let result = modelValue;
    if (isString(result)) {
      try {
        result = JSON.parse(result);
      } catch (e) {
        emits('format-error', modelValue);
        return modelValue as string;
      }
    }
    return JSON.stringify(result, null, 2);
  });

  watch(
    () => getValue.value,
    async (value) => {
      await nextTick();
      const oldValue = editor?.getValue();
      if (value !== oldValue) {
        setTimeout(() => {
          editor?.setValue(value);
        }, 100);
      }
    },
    { flush: 'post' },
  );

  watch(
    () => appStore.getDarkMode,
    async () => {
      setTheme();
    },
    {
      immediate: true,
    },
  );

  function setTheme() {
    unref(editor)?.setOption(
      'theme',
      appStore.getDarkMode === 'light' ? 'idea' : 'dracula',
    );
  }

  onMounted(() => {
    editor = CodeMirror.fromTextArea(codeEditor.value, {
      value: '',
      autoRefresh: true,
      mode: props.mode,
      indentWithTabs: false, // 在缩进时，是否需要把 n*tab宽度个空格替换成n个tab字符，默认为false
      smartIndent: true, // 自动缩进，设置是否根据上下文自动缩进（和上一行相同的缩进量）。默认为true
      lineNumbers: true, // 是否在编辑器左侧显示行号
      matchBrackets: true, // 括号匹配
      readOnly: props.readonly,
      // 启用代码折叠相关功能:开始
      foldGutter: true,
      lineWrapping: true,
      // 启用代码折叠相关功能:结束
      styleActiveLine: true, // 光标行高亮
    });
    // 监听编辑器的change事件
    editor.on("change", () => {
      // 触发v-model的双向绑定
      emits('update:modelValue', editor!.getValue());
    });
    useWindowSizeFn(editor.refresh);
  });

</script>
