<script setup lang="ts">
import type { UploadProps } from 'ant-design-vue';

import type { PropType } from 'vue';

import type { CropendResult, Cropper } from './types';

import { h, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { useNamespace } from '@vben/hooks';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';
import { isFunction } from '@vben/utils';

import { dataURLtoBlob } from '@abp/core';
import { Avatar, Button, Space, Tooltip, Upload } from 'ant-design-vue';

import CropperImage from './Cropper.vue';

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
const src = ref(props.src || '');
const previewSource = ref('');
const fileList = ref<UploadProps['fileList']>([]);
const cropper = ref<Cropper>();
let scaleX = 1;
let scaleY = 1;

const { b, e } = useNamespace('cropper-am');
const [Modal, modalApi] = useVbenModal({
  class: 'w-[800px]',
  fullscreen: false,
  fullscreenButton: false,
  confirmText: $t('cropper.confirmText'),
  onConfirm: handleOk,
  title: $t('cropper.title'),
});
function handleBeforeUpload(file: File) {
  const reader = new FileReader();
  reader.readAsDataURL(file);
  src.value = '';
  previewSource.value = '';
  reader.addEventListener('load', (e) => {
    src.value = (e.target?.result as string) ?? '';
    fileName = file.name;
  });
  return false;
}
function handleCropend({ imgBase64 }: CropendResult) {
  previewSource.value = imgBase64;
}

function handleReady(cropperInstance: Cropper) {
  cropper.value = cropperInstance;
}
function handlerToolbar(event: string, arg?: number) {
  if (!cropper.value) {
    return;
  }
  if (event === 'scaleX') {
    scaleX = arg = scaleX === -1 ? 1 : -1;
  }
  if (event === 'scaleY') {
    scaleY = arg = scaleY === -1 ? 1 : -1;
  }
  switch (event) {
    case 'reset': {
      return cropper.value.reset();
    }
    case 'rotate': {
      return cropper.value.rotate(arg!);
    }
    case 'scaleX': {
      return cropper.value.scaleX(scaleX);
    }
    case 'scaleY': {
      return cropper.value.scaleY(scaleY);
    }
    case 'zoom': {
      return cropper.value.zoom(arg!);
    }
  }
}
async function handleOk() {
  const uploadApi = props.uploadApi;
  if (uploadApi && isFunction(uploadApi)) {
    const blob = dataURLtoBlob(previewSource.value);
    try {
      modalApi.setState({ submitting: true });
      await uploadApi({ name: 'file', file: blob, fileName });
      emits('uploadSuccess', previewSource.value);
      modalApi.close();
    } finally {
      modalApi.setState({ submitting: false });
    }
  }
}
</script>

<template>
  <Modal>
    <div :class="b()">
      <div :class="e('left')">
        <div :class="e('cropper')">
          <CropperImage
            v-if="src"
            :src="src"
            height="300px"
            :circled="circled"
            @cropend="handleCropend"
            @ready="handleReady"
          />
        </div>
        <div :class="e('toolbar')">
          <Upload
            :file-list="fileList"
            accept="image/*"
            :before-upload="handleBeforeUpload"
          >
            <Tooltip :title="$t('cropper.selectImage')" placement="bottom">
              <Button size="small" :icon="h(UploadIcon)" type="primary" />
            </Tooltip>
          </Upload>
          <Space>
            <Tooltip :title="$t('cropper.btn_reset')" placement="bottom">
              <Button
                type="primary"
                :icon="h(ResetIcon)"
                size="small"
                :disabled="!src"
                @click="handlerToolbar('reset')"
              />
            </Tooltip>
            <Tooltip :title="$t('cropper.btn_rotate_left')" placement="bottom">
              <Button
                type="primary"
                :icon="h(RotateLeftIcon)"
                size="small"
                :disabled="!src"
                @click="handlerToolbar('rotate', -45)"
              />
            </Tooltip>
            <Tooltip :title="$t('cropper.btn_rotate_right')" placement="bottom">
              <Button
                type="primary"
                :icon="h(RotateRightIcon)"
                size="small"
                :disabled="!src"
                @click="handlerToolbar('rotate', 45)"
              />
            </Tooltip>
            <Tooltip :title="$t('cropper.btn_scale_x')" placement="bottom">
              <Button
                type="primary"
                :icon="h(ScaleXIcon)"
                size="small"
                :disabled="!src"
                @click="handlerToolbar('scaleX')"
              />
            </Tooltip>
            <Tooltip :title="$t('cropper.btn_scale_y')" placement="bottom">
              <Button
                type="primary"
                :icon="h(ScaleYIcon)"
                size="small"
                :disabled="!src"
                @click="handlerToolbar('scaleY')"
              />
            </Tooltip>
            <Tooltip :title="$t('cropper.btn_zoom_in')" placement="bottom">
              <Button
                type="primary"
                :icon="h(ZoomInIcon)"
                size="small"
                :disabled="!src"
                @click="handlerToolbar('zoom', 0.1)"
              />
            </Tooltip>
            <Tooltip :title="$t('cropper.btn_zoom_out')" placement="bottom">
              <Button
                type="primary"
                :icon="h(ZoomOutIcon)"
                size="small"
                :disabled="!src"
                @click="handlerToolbar('zoom', -0.1)"
              />
            </Tooltip>
          </Space>
        </div>
      </div>
      <div :class="e('right')">
        <div :class="e(`preview`)">
          <img
            :src="previewSource"
            v-if="previewSource"
            :alt="$t('cropper.preview')"
          />
        </div>
        <template v-if="previewSource">
          <div :class="e(`group`)">
            <Avatar :src="previewSource" size="large" />
            <Avatar :src="previewSource" :size="48" />
            <Avatar :src="previewSource" :size="64" />
            <Avatar :src="previewSource" :size="80" />
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
    height: 300px;
    background: #eee;
    background-image:
      linear-gradient(
        45deg,
        rgb(0 0 0 / 25%) 25%,
        transparent 0,
        transparent 75%,
        rgb(0 0 0 / 25%) 0
      ),
      linear-gradient(
        45deg,
        rgb(0 0 0 / 25%) 25%,
        transparent 0,
        transparent 75%,
        rgb(0 0 0 / 25%) 0
      );
    background-position:
      0 0,
      12px 12px;
    background-size: 24px 24px;
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
