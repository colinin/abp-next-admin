<script lang="ts" setup>
import type { CSSProperties, PropType } from 'vue';

import type { Nullable } from '@vben/types';

import {
  computed,
  onMounted,
  onUnmounted,
  ref,
  unref,
  useAttrs,
  useTemplateRef,
} from 'vue';

import { useNamespace } from '@vben/hooks';

import { useDebounceFn } from '@vueuse/core';
import Cropper from 'cropperjs';

import 'cropperjs/dist/cropper.css';

type Options = Cropper.Options;
type ElRef<T extends HTMLElement = HTMLDivElement> = Nullable<T>;

const props = defineProps({
  src: { type: String, required: true },
  alt: { type: String, default: '' },
  circled: { type: Boolean, default: false },
  realTimePreview: { type: Boolean, default: true },
  height: { type: [String, Number], default: '360px' },
  crossorigin: {
    type: String as PropType<'' | 'anonymous' | 'use-credentials' | undefined>,
    default: undefined,
  },
  imageStyle: { type: Object as PropType<CSSProperties>, default: () => ({}) },
  options: { type: Object as PropType<Options>, default: () => ({}) },
});
const emits = defineEmits(['cropend', 'ready', 'cropendError']);
const defaultOptions: Options = {
  aspectRatio: 1,
  zoomable: true,
  zoomOnTouch: true,
  zoomOnWheel: true,
  cropBoxMovable: true,
  cropBoxResizable: true,
  toggleDragModeOnDblclick: true,
  autoCrop: true,
  background: true,
  highlight: true,
  center: true,
  responsive: true,
  restore: true,
  checkCrossOrigin: true,
  checkOrientation: true,
  scalable: true,
  modal: true,
  guides: true,
  movable: true,
  rotatable: true,
};

const attrs = useAttrs();

const imgElRef = useTemplateRef<ElRef<HTMLImageElement>>('imgElRef');
const cropper = ref<Nullable<Cropper>>();
const isReady = ref(false);

const { b, is } = useNamespace('cropper-image');
const debounceRealTimeCroppered = useDebounceFn(realTimeCroppered, 80);

const getImageStyle = computed((): CSSProperties => {
  return {
    height: props.height,
    maxWidth: '100%',
    ...props.imageStyle,
  };
});

const getClass = computed(() => {
  return [b(), attrs.class, is('circled', props.circled)];
});

const getWrapperStyle = computed((): CSSProperties => {
  return { height: `${`${props.height}`.replace(/px/, '')}px` };
});

onMounted(init);

onUnmounted(() => {
  cropper.value?.destroy();
});

async function init() {
  const imgEl = unref(imgElRef);
  if (!imgEl) {
    return;
  }
  cropper.value = new Cropper(imgEl, {
    ...defaultOptions,
    ready: () => {
      isReady.value = true;
      realTimeCroppered();
      emits('ready', cropper.value);
    },
    crop() {
      debounceRealTimeCroppered();
    },
    zoom() {
      debounceRealTimeCroppered();
    },
    cropmove() {
      debounceRealTimeCroppered();
    },
    ...props.options,
  });
}

// Real-time display preview
function realTimeCroppered() {
  props.realTimePreview && croppered();
}

// event: return base64 and width and height information after cropping
function croppered() {
  if (!cropper.value) {
    return;
  }
  const imgInfo = cropper.value.getData();
  const canvas = props.circled
    ? getRoundedCanvas()
    : cropper.value.getCroppedCanvas();
  canvas.toBlob((blob) => {
    if (!blob) {
      return;
    }
    const fileReader: FileReader = new FileReader();
    fileReader.readAsDataURL(blob);
    fileReader.onloadend = (e) => {
      emits('cropend', {
        imgBase64: e.target?.result ?? '',
        imgInfo,
      });
    };
    // eslint-disable-next-line unicorn/prefer-add-event-listener
    fileReader.onerror = () => {
      emits('cropendError');
    };
  }, 'image/png');
}

// Get a circular picture canvas
function getRoundedCanvas() {
  const sourceCanvas = cropper.value!.getCroppedCanvas();
  const canvas = document.createElement('canvas');
  const context = canvas.getContext('2d')!;
  const width = sourceCanvas.width;
  const height = sourceCanvas.height;
  canvas.width = width;
  canvas.height = height;
  context.imageSmoothingEnabled = true;
  context.drawImage(sourceCanvas, 0, 0, width, height);
  context.globalCompositeOperation = 'destination-in';
  context.beginPath();
  context.arc(
    width / 2,
    height / 2,
    Math.min(width, height) / 2,
    0,
    2 * Math.PI,
    true,
  );
  context.fill();
  return canvas;
}
</script>
<template>
  <div :class="getClass" :style="getWrapperStyle">
    <img
      v-show="isReady"
      ref="imgElRef"
      :src="src"
      :alt="alt"
      :crossorigin="crossorigin"
      :style="getImageStyle"
    />
  </div>
</template>
<style scoped lang="scss">
$namespace: vben;

.#{$namespace}-cropper-image {
  &.is-circled {
    .cropper-view-box,
    .cropper-face {
      border-radius: 50%;
    }
  }
}
</style>
