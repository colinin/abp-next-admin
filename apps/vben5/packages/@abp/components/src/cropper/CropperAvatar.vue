<script setup lang="ts">
import type { CSSProperties, PropType } from 'vue';

import type { VbenButtonProps } from '@vben-core/shadcn-ui';

import { computed, ref, unref, watch, watchEffect } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { useNamespace } from '@vben/hooks';
import { createIconifyIcon } from '@vben/icons';
import { useI18n } from '@vben/locales';

import { VbenButton } from '@vben-core/shadcn-ui';

import CropperModal from './CropperModal.vue';

interface File {
  file: Blob;
  fileName?: string;
  name: string;
}

const props = defineProps({
  width: { type: [String, Number], default: '200px' },
  value: { type: String, default: '' },
  showBtn: { type: Boolean, default: true },
  btnProps: {
    type: Object as PropType<VbenButtonProps>,
    default: undefined,
  },
  btnText: { type: String, default: '' },
  uploadApi: {
    type: Function as PropType<(file: File) => Promise<void>>,
    default: undefined,
  },
});

const emits = defineEmits(['update:value', 'change']);

const UploadIcon = createIconifyIcon('ant-design:cloud-upload-outlined');

const sourceValue = ref(props.value || '');
const { b, e } = useNamespace('cropper-avatar');
const [Modal, modalApi] = useVbenModal({
  connectedComponent: CropperModal,
});
const { t } = useI18n();

const getWidth = computed(() => `${`${props.width}`.replace(/px/, '')}px`);

const getIconWidth = computed(
  () => `${Number.parseInt(`${props.width}`.replace(/px/, '')) / 2}px`,
);

const getStyle = computed((): CSSProperties => ({ width: unref(getWidth) }));

const getImageWrapperStyle = computed((): CSSProperties => ({
  height: unref(getWidth),
  width: unref(getWidth),
}));

watchEffect(() => {
  sourceValue.value = props.value || '';
});

watch(
  () => sourceValue.value,
  (v: string) => {
    emits('update:value', v);
  },
);

function handleUploadSuccess(url: string) {
  sourceValue.value = url;
  emits('change', url);
}
function openModal() {
  modalApi.open();
}
function closeModal() {
  modalApi.close();
}

defineExpose({ closeModal, openModal });
</script>

<template>
  <div :class="b()" :style="getStyle">
    <div
      :class="e(`image-wrapper`)"
      :style="getImageWrapperStyle"
      @click="openModal"
    >
      <div :class="e(`image-mask`)" :style="getImageWrapperStyle">
        <UploadIcon
          :style="getImageWrapperStyle"
          :width="getIconWidth"
          color="#d6d6d6"
        />
      </div>
      <img v-if="sourceValue" :src="sourceValue" alt="avatar" />
    </div>
    <VbenButton
      v-if="showBtn"
      v-bind="btnProps"
      :class="e(`upload-btn`)"
      @click="openModal"
    >
      {{ btnText ? btnText : t('cropper.selectImage') }}
    </VbenButton>

    <Modal
      :src="sourceValue"
      :upload-api="uploadApi"
      @upload-success="handleUploadSuccess"
    />
  </div>
</template>

<style scoped lang="scss">
$namespace: vben;

.#{$namespace}-cropper-avatar {
  display: inline-block;
  text-align: center;

  &__image-wrapper {
    overflow: hidden;
    cursor: pointer;
    border: var(--border);
    border-radius: 50%;

    img {
      width: 100%;
      // height: 100%;
    }
  }

  &__image-mask {
    position: absolute;
    width: inherit;
    height: inherit;
    cursor: pointer;
    background: rgb(0 0 0 / 40%);
    border: inherit;
    border-radius: inherit;
    opacity: 0;
    transition: opacity 0.4s;

    ::v-deep(svg) {
      margin: auto;
    }
  }

  &__image-mask:hover {
    opacity: 40;
  }

  &__upload-btn {
    margin: 10px auto;
  }
}
</style>
