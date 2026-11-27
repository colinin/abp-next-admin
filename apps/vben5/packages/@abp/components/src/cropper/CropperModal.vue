<script setup lang="ts">
import type { PropType } from 'vue';

import { computed, nextTick, reactive, ref, watch } from 'vue';

import { useVbenModal, VCropper } from '@vben/common-ui';
import { useNamespace } from '@vben/hooks';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';
import { isFunction } from '@vben/utils';

import { VbenAvatar, VbenButton, VbenTooltip } from '@vben-core/shadcn-ui';

import { dataURLtoBlob } from '@abp/core';
import { useDebounceFn } from '@vueuse/core';

type ApiFunParams = { file: Blob; fileName: string; name: string };

const props = defineProps({
  circled: { type: Boolean, default: true },
  uploadApi: {
    type: Function as PropType<(params: ApiFunParams) => Promise<any>>,
    default: undefined,
  },
  src: { type: String, default: '' },
});

const emits = defineEmits<{
  (event: 'uploadSuccess', url: string): void;
}>();

const UploadIcon = createIconifyIcon('ant-design:upload-outlined');
const ResetIcon = createIconifyIcon('ant-design:reload-outlined');
const RotateLeftIcon = createIconifyIcon('ant-design:rotate-left-outlined');
const RotateRightIcon = createIconifyIcon('ant-design:rotate-right-outlined');
const ScaleXIcon = createIconifyIcon('vaadin:arrows-long-h');
const ScaleYIcon = createIconifyIcon('vaadin:arrows-long-v');
const ZoomInIcon = createIconifyIcon('ant-design:zoom-in-outlined');
const ZoomOutIcon = createIconifyIcon('ant-design:zoom-out-outlined');

let fileName = '';
const originalSrc = ref(props.src || '');
const src = ref(props.src || '');
const previewSource = ref('');
const fileInputRef = ref<HTMLInputElement>();
const cropperRef = ref<InstanceType<typeof VCropper>>();
const transformState = reactive({
  rotation: 0,
  scaleX: 1,
  scaleY: 1,
  zoom: 1,
});
const { b, e } = useNamespace('cropper-am');

const cropperAspectRatio = computed(() => (props.circled ? '1:1' : undefined));

const [Modal, modalApi] = useVbenModal({
  class: 'w-[800px]',
  confirmText: $t('cropper.confirmText'),
  fullscreen: false,
  fullscreenButton: false,
  onConfirm: handleOk,
  async onOpened() {
    await nextTick();
    schedulePreview();
  },
  title: $t('cropper.title'),
});

function handleFileChange(event: Event) {
  const input = event.target as HTMLInputElement;
  const file = input.files?.[0];
  if (!file) {
    return;
  }
  const reader = new FileReader();
  reader.readAsDataURL(file);
  src.value = '';
  previewSource.value = '';
  reader.addEventListener('load', (e) => {
    originalSrc.value = (e.target?.result as string) ?? '';
    src.value = originalSrc.value;
    fileName = file.name;
    resetTransformState();
  });
  input.value = '';
}

function handleToolbar(action: string, arg?: number) {
  if (!src.value) {
    return;
  }
  switch (action) {
    case 'reset': {
      resetTransformState();
      src.value = originalSrc.value;
      break;
    }
    case 'rotate': {
      transformState.rotation = (transformState.rotation + (arg ?? 0)) % 360;
      void applyTransforms();
      break;
    }
    case 'scaleX': {
      transformState.scaleX *= -1;
      void applyTransforms();
      break;
    }
    case 'scaleY': {
      transformState.scaleY *= -1;
      void applyTransforms();
      break;
    }
    case 'zoom': {
      const nextZoom = transformState.zoom + (arg ?? 0);
      transformState.zoom = Math.min(3, Math.max(0.2, nextZoom));
      void applyTransforms();
      break;
    }
  }
  previewSource.value = '';
  schedulePreview();
}

function resetTransformState() {
  transformState.rotation = 0;
  transformState.scaleX = 1;
  transformState.scaleY = 1;
  transformState.zoom = 1;
}

async function applyTransforms() {
  if (!originalSrc.value) {
    src.value = '';
    return;
  }
  const image = await loadImage(originalSrc.value);
  const radians = (transformState.rotation * Math.PI) / 180;
  const width = image.naturalWidth * transformState.zoom;
  const height = image.naturalHeight * transformState.zoom;
  const canvas = document.createElement('canvas');
  canvas.width = Math.ceil(
    Math.abs(width * Math.cos(radians)) + Math.abs(height * Math.sin(radians)),
  );
  canvas.height = Math.ceil(
    Math.abs(width * Math.sin(radians)) + Math.abs(height * Math.cos(radians)),
  );
  const context = canvas.getContext('2d');
  if (!context) {
    return;
  }
  context.translate(canvas.width / 2, canvas.height / 2);
  context.rotate(radians);
  context.scale(transformState.scaleX, transformState.scaleY);
  context.drawImage(image, -width / 2, -height / 2, width, height);
  src.value = canvas.toDataURL('image/png');
}

function loadImage(source: string): Promise<HTMLImageElement> {
  return new Promise((resolve, reject) => {
    const image = new Image();
    if (/^https?:\/\//.test(source)) {
      image.crossOrigin = 'anonymous';
    }
    image.addEventListener('load', () => resolve(image));
    image.addEventListener('error', () =>
      reject(new Error(`Failed to load image: ${source}`)),
    );
    image.src = source;
  });
}

async function cropImage(): Promise<string | undefined> {
  if (!cropperRef.value) {
    return undefined;
  }
  const result = await cropperRef.value.getCropImage('image/png', 1, 'base64');
  if (typeof result !== 'string') {
    return undefined;
  }
  return props.circled ? await makeCircledImage(result) : result;
}

async function updatePreview() {
  try {
    const result = await cropImage();
    previewSource.value = result ?? '';
  } catch {
    previewSource.value = '';
  }
}

function handleCropperImageLoad(event: Event) {
  const target = event.target;
  if (target instanceof HTMLImageElement) {
    target.draggable = false;
  }
  schedulePreview();
}

const schedulePreview = useDebounceFn(updatePreview, 120);

async function makeCircledImage(source: string): Promise<string> {
  const image = await loadImage(source);
  const size = Math.min(image.naturalWidth, image.naturalHeight);
  const canvas = document.createElement('canvas');
  canvas.width = size;
  canvas.height = size;
  const context = canvas.getContext('2d');
  if (!context) {
    return source;
  }
  context.drawImage(
    image,
    (image.naturalWidth - size) / 2,
    (image.naturalHeight - size) / 2,
    size,
    size,
    0,
    0,
    size,
    size,
  );
  context.globalCompositeOperation = 'destination-in';
  context.beginPath();
  context.arc(size / 2, size / 2, size / 2, 0, 2 * Math.PI);
  context.fill();
  return canvas.toDataURL('image/png');
}

async function handleOk() {
  const uploadApi = props.uploadApi;
  if (!uploadApi || !isFunction(uploadApi)) {
    return;
  }
  await updatePreview();
  if (!previewSource.value) {
    return;
  }
  const blob = dataURLtoBlob(previewSource.value);
  try {
    modalApi.setState({ submitting: true });
    await uploadApi({ fileName, file: blob, name: 'file' });
    emits('uploadSuccess', previewSource.value);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}

function selectImage() {
  fileInputRef.value?.click();
}

watch(
  () => props.src,
  (value) => {
    if (value === originalSrc.value) {
      return;
    }
    originalSrc.value = value || '';
    src.value = value || '';
    previewSource.value = '';
    resetTransformState();
  },
);

watch(
  () => props.circled,
  () => {
    previewSource.value = '';
    schedulePreview();
  },
);
</script>

<template>
  <Modal>
    <div :class="b()">
      <div :class="e('left')">
        <div
          :class="e('cropper')"
          @dragstart.prevent
          @load.capture="handleCropperImageLoad"
          @mouseup="schedulePreview"
          @selectstart.prevent
          @touchend="schedulePreview"
        >
          <VCropper
            v-if="src"
            ref="cropperRef"
            :aspect-ratio="cropperAspectRatio"
            :height="300"
            :img="src"
            :width="440"
          />
        </div>
        <div :class="e('toolbar')">
          <input
            ref="fileInputRef"
            accept="image/*"
            class="hidden"
            type="file"
            @change="handleFileChange"
          />
          <VbenTooltip side="bottom">
            <template #trigger>
              <VbenButton size="sm" @click="selectImage">
                <UploadIcon class="size-4" />
              </VbenButton>
            </template>
            {{ $t('cropper.selectImage') }}
          </VbenTooltip>
          <div class="flex items-center gap-2">
            <VbenTooltip side="bottom">
              <template #trigger>
                <VbenButton
                  :disabled="!src"
                  size="sm"
                  @click="handleToolbar('reset')"
                >
                  <ResetIcon class="size-4" />
                </VbenButton>
              </template>
              {{ $t('cropper.btn_reset') }}
            </VbenTooltip>
            <VbenTooltip side="bottom">
              <template #trigger>
                <VbenButton
                  :disabled="!src"
                  size="sm"
                  @click="handleToolbar('rotate', -45)"
                >
                  <RotateLeftIcon class="size-4" />
                </VbenButton>
              </template>
              {{ $t('cropper.btn_rotate_left') }}
            </VbenTooltip>
            <VbenTooltip side="bottom">
              <template #trigger>
                <VbenButton
                  :disabled="!src"
                  size="sm"
                  @click="handleToolbar('rotate', 45)"
                >
                  <RotateRightIcon class="size-4" />
                </VbenButton>
              </template>
              {{ $t('cropper.btn_rotate_right') }}
            </VbenTooltip>
            <VbenTooltip side="bottom">
              <template #trigger>
                <VbenButton
                  :disabled="!src"
                  size="sm"
                  @click="handleToolbar('scaleX')"
                >
                  <ScaleXIcon class="size-4" />
                </VbenButton>
              </template>
              {{ $t('cropper.btn_scale_x') }}
            </VbenTooltip>
            <VbenTooltip side="bottom">
              <template #trigger>
                <VbenButton
                  :disabled="!src"
                  size="sm"
                  @click="handleToolbar('scaleY')"
                >
                  <ScaleYIcon class="size-4" />
                </VbenButton>
              </template>
              {{ $t('cropper.btn_scale_y') }}
            </VbenTooltip>
            <VbenTooltip side="bottom">
              <template #trigger>
                <VbenButton
                  :disabled="!src"
                  size="sm"
                  @click="handleToolbar('zoom', 0.1)"
                >
                  <ZoomInIcon class="size-4" />
                </VbenButton>
              </template>
              {{ $t('cropper.btn_zoom_in') }}
            </VbenTooltip>
            <VbenTooltip side="bottom">
              <template #trigger>
                <VbenButton
                  :disabled="!src"
                  size="sm"
                  @click="handleToolbar('zoom', -0.1)"
                >
                  <ZoomOutIcon class="size-4" />
                </VbenButton>
              </template>
              {{ $t('cropper.btn_zoom_out') }}
            </VbenTooltip>
          </div>
        </div>
      </div>
      <div :class="e('right')">
        <div :class="e('preview')">
          <img
            v-if="previewSource"
            :alt="$t('cropper.preview')"
            :src="previewSource"
          />
        </div>
        <template v-if="previewSource">
          <div :class="e('group')">
            <VbenAvatar
              v-for="size in [40, 48, 64, 80]"
              :key="size"
              :size="size"
              :src="previewSource"
            />
          </div>
        </template>
      </div>
    </div>
  </Modal>
</template>

<style scoped lang="scss">
$namespace: vben;

.#{$namespace}-cropper-am {
  display: flex;

  &__left,
  &__right {
    height: 340px;
  }

  &__left {
    width: 55%;
  }

  &__right {
    width: 45%;
  }

  &__cropper {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 300px;
    overflow: hidden;
    user-select: none;

    :deep(img) {
      user-select: none;
      -webkit-user-drag: none;
    }
  }

  &__toolbar {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-top: 10px;
  }

  &__preview {
    width: 220px;
    height: 220px;
    margin: 0 auto;
    overflow: hidden;
    border: var(--border);
    border-radius: 50%;

    img {
      width: 100%;
      height: 100%;
    }
  }

  &__group {
    display: flex;
    align-items: center;
    justify-content: space-around;
    padding-top: 8px;
    margin-top: 8px;
    border-top: var(--border);
  }
}
</style>
