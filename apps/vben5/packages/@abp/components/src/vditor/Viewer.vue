<script lang="ts" setup>
import {
  computed,
  nextTick,
  onBeforeUnmount,
  onDeactivated,
  ref,
  unref,
  watch,
} from 'vue';

import { preferences } from '@vben/preferences';

import VditorPreview from 'vditor';

const props = defineProps<{
  class?: string;
  value: string;
}>();
const viewerRef = ref<HTMLDivElement>();
const vditorPreviewRef = ref<VditorPreview>();

const skinName = computed(() => {
  return preferences.theme.mode === 'light' ? 'light' : 'dark';
});

function init() {
  const viewerEl = unref(viewerRef) as HTMLDivElement;
  const isDark = skinName.value === 'dark';
  VditorPreview.preview(viewerEl, props.value, {
    hljs: {
      style: isDark ? 'dracula' : 'github',
    },
    mode: isDark ? 'dark' : 'light',
    theme: {
      current: isDark ? 'dark' : 'light',
    },
  });
}

watch(
  () => skinName.value,
  (val) => {
    const isDark = val === 'dark';
    VditorPreview.setContentTheme(isDark ? 'dark' : 'light', '');
    VditorPreview.setCodeTheme(isDark ? 'dracula' : 'github');
    init();
  },
);

watch(
  () => props.value,
  (v, oldValue) => {
    v !== oldValue && nextTick(init);
  },
  {
    immediate: true,
  },
);

function destroy() {
  const vditorInstance = unref(vditorPreviewRef);
  if (!vditorInstance) return;
  try {
    vditorInstance?.destroy?.();
  } catch {}
  vditorPreviewRef.value = undefined;
}

onBeforeUnmount(destroy);
onDeactivated(destroy);
</script>

<template>
  <div ref="viewerRef" id="markdownViewer" :class="$props.class"></div>
</template>

<style scoped>
.markdown-viewer {
  width: 100%;
}
</style>
