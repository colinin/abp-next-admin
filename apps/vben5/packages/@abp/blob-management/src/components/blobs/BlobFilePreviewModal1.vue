<script setup lang="ts">
import type { BlobDto } from '../../types/blobs';

import { defineAsyncComponent, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { downloadFileFromUrl } from '@vben/utils';

import { Button, Spin } from 'ant-design-vue';

import { useBlobsApi } from '../../api/useBlobsApi';

const VueFilesPreview = defineAsyncComponent({
  loader: async () => {
    const res = await import('vue-files-preview');
    import('vue-files-preview/lib/style.css');
    return res.VueFilesPreview;
  },
  loadingComponent: Spin,
  delay: 200,
});

const fileName = ref<string>();
const previewUrl = ref<string>();
const errorMsg = ref<string>();

const { generatePreviewUrlApi } = useBlobsApi();

const [Modal, modalApi] = useVbenModal({
  showCancelButton: false,
  showConfirmButton: false,
  fullscreen: true,
  fullscreenButton: false,
  onOpenChange: async (isOpen) => {
    isOpen && onInit();
  },
  onClosed() {
    previewUrl.value = undefined;
    fileName.value = undefined;
    errorMsg.value = undefined;
  },
});

async function onInit() {
  try {
    modalApi.lock();
    const blob = modalApi.getData<BlobDto>();
    fileName.value = blob.name;
    previewUrl.value = await generatePreviewUrlApi(blob.id);
  } finally {
    modalApi.unlock();
  }
}

async function handleDownload() {
  downloadFileFromUrl({
    fileName: fileName.value!,
    source: previewUrl.value!,
  });
}

function onError(error: Error) {
  errorMsg.value = error.message;
}
</script>

<template>
  <Modal :title="fileName">
    <div class="preview-container">
      <VueFilesPreview
        v-if="previewUrl"
        :url="previewUrl"
        :name="fileName"
        @error="onError"
      />

      <!-- 下载按钮 -->
      <div v-if="errorMsg && previewUrl" class="download-button">
        <Button type="primary" @click="handleDownload">
          {{ $t('BlobManagement.Blobs:Download') }}
        </Button>
      </div>
    </div>
  </Modal>
</template>

<style scoped lang="scss">
.preview-container {
  position: relative;
  height: 100%;

  .error-message {
    padding: 40px 20px;
    text-align: center;
  }

  .download-button {
    position: sticky;
    right: 20px;
    bottom: 20px;
    z-index: 10;
    float: right;
    margin-top: 20px;
  }
}
</style>
