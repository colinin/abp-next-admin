<script setup lang="ts">
import type { VxeComponentStyleType } from 'vxe-table';

import { h, ref, useTemplateRef, watch } from 'vue';
import uploader from 'vue-simple-uploader';
import 'vue-simple-uploader/dist/style.css';

import { useVbenModal } from '@vben/common-ui';
import { useRefresh } from '@vben/hooks';
import { $t } from '@vben/locales';
import { useAccessStore } from '@vben/stores';

import { isNullOrWhiteSpace } from '@abp/core';
import {
  CaretRightOutlined,
  DeleteOutlined,
  PauseOutlined,
} from '@ant-design/icons-vue';
import { Button, Tag, Tooltip } from 'ant-design-vue';
import { VxeColumn, VxeTable } from 'vxe-table';

const emits = defineEmits<{
  (event: 'fileUploaded', file: any): void;
}>();
const Uploader = uploader.Uploader;
const UploaderDrop = uploader.UploaderDrop;
const UploaderList = uploader.UploaderList;
const UploaderUnsupport = uploader.UploaderUnsupport;

interface ModalState {
  bucket: string;
  path: string;
}

const selectBtn = useTemplateRef<any>('selectBtn');
const uploaderWrap = useTemplateRef<any>('uploaderWrap');

const { refresh } = useRefresh();
const accessStore = useAccessStore();

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  closeOnClickModal: false,
  closeOnPressEscape: false,
  draggable: true,
  footer: false,
  onCancel: () => {
    uploaderWrap.value?.uploader.cancel();
  },
  onOpenChange: (isOpen) => {
    if (isOpen) {
      onInit();
    }
  },
});

const options = ref({
  chunkRetryInterval: null,
  headers: {},
  initialPaused: true,
  maxChunkRetries: 3,
  permanentErrors: [400, 401, 403, 404, 415, 500, 501],
  processParams: (params: any) => params,
  processResponse: (response: any, cb: any) => {
    if (!isNullOrWhiteSpace(response)) {
      const error = JSON.parse(response);
      if (error.code !== '0') {
        cb(true, error.message);
        return;
      }
    }
    cb(null, response);
  },
  successStatuses: [200, 201, 202, 204, 205],
  target: '/api/oss-management/objects/upload',
  testChunks: false,
});

function onInit() {
  const state = modalApi.getData<ModalState>();
  options.value = {
    ...options.value,
    headers: {
      Authorization: accessStore.accessToken,
    },
    processParams: (params: any) => {
      params.bucket = state.bucket;
      params.path = state.path;
      return params;
    },
  };
}

function onSelectFiles() {
  selectBtn.value?.click();
}

function onResume(file: any) {
  if (file.error) {
    file.errorMsg = '';
    file.retry();
  } else {
    file.resume();
  }
}

function onPause(file: any) {
  file.pause();
}

function onDelete(file: any) {
  file.cancel();
}

function onFileSubmitted(_: any, files: any[]) {
  files.forEach((f) => {
    f.paused = true;
    f.completed = false;
    f.progress = 0;
    f.progressText = '0 %';
  });
}

function onUploadProgress(_: any, file: any) {
  if (file._prevUploadedSize) {
    const progress = Math.floor((file._prevUploadedSize / file.size) * 100);
    file.progress = progress;
    file.progressText = `${progress} %`;
    file.completed = progress === 100;
  }
}

function onUploadError(_rootFile: any, file: any, message: any, chunk: any) {
  if (chunk?.xhr?.status === 401) {
    // 401 错误代码刷新页面, 由axios拦截器刷新token
    refresh();
  } else {
    file.errorMsg = message;
  }
}

function onUploadSuccess(_rootFile: any, file: any) {
  emits('fileUploaded', file);
}

function formatSize(size: number) {
  if (size < 1024) {
    return `${size.toFixed(0)} bytes`;
  } else if (size < 1024 * 1024) {
    return `${(size / 1024).toFixed(0)} KB`;
  } else if (size < 1024 * 1024 * 1024) {
    return `${(size / 1024 / 1024).toFixed(1)} MB`;
  } else {
    return `${(size / 1024 / 1024 / 1024).toFixed(1)} GB`;
  }
}

function rowStyle(params: any): VxeComponentStyleType {
  // console.log('上传状态 --->', params.row.status);
  if (params.row.error) {
    return {
      background: '#ffe0e0',
    };
  }
  const startColor = 'rgba(255, 200, 200, 0.7)';
  const endColor = 'rgba(200, 255, 200, 0.7)';
  return {
    background: `linear-gradient(
      to right,
      ${startColor} 0%,
      ${endColor} ${params.row.progress}%,
      transparent ${params.row.progress}%,
      transparent 100%
    )`,
    backgroundSize: '100% 100%',
    transition: 'background 0.5s ease',
  };
}

watch(
  () => [selectBtn.value, uploaderWrap.value],
  ([button, wrap]) => {
    if (button && wrap) {
      wrap.uploader.assignBrowse(button);
    }
  },
);
</script>

<template>
  <Modal :title="$t('AbpOssManagement.Objects:UploadFile')">
    <Uploader
      :options="options"
      ref="uploaderWrap"
      @file-error="onUploadError"
      @file-progress="onUploadProgress"
      @file-success="onUploadSuccess"
      @files-submitted="onFileSubmitted"
    >
      <UploaderUnsupport />
      <UploaderDrop>
        <div class="flex flex-row gap-2">
          <input ref="selectBtn" style="display: none" />
          <Button type="primary" @click="onSelectFiles">
            {{ $t('AbpOssManagement.Upload:SelectFile') }}
          </Button>
        </div>
      </UploaderDrop>
      <UploaderList>
        <template #default="{ fileList }">
          <VxeTable :data="fileList" :row-style="rowStyle">
            <VxeColumn type="seq" width="70" />
            <VxeColumn
              field="name"
              :title="$t('AbpOssManagement.DisplayName:Name')"
            />
            <VxeColumn
              field="size"
              :title="$t('AbpOssManagement.DisplayName:Size')"
              width="100"
            >
              <template #default="{ row }">
                <span>{{ formatSize(row.size) }}</span>
              </template>
            </VxeColumn>
            <VxeColumn
              field="status"
              :title="$t('AbpOssManagement.DisplayName:Status')"
              width="180"
            >
              <template #default="{ row }">
                <Tag v-if="row.completed" color="green">
                  {{ $t('AbpOssManagement.Upload:Completed') }}
                </Tag>
                <Tooltip v-else-if="row.error" :title="row.errorMsg">
                  <Tag color="red">
                    {{ $t('AbpOssManagement.Upload:Error') }}
                  </Tag>
                </Tooltip>
                <Tag v-else-if="row.paused" color="orange">
                  {{ $t('AbpOssManagement.Upload:Pause') }}
                </Tag>
                <span v-else>{{
                  `${row.progressText} ${formatSize(row.averageSpeed)}/s`
                }}</span>
              </template>
            </VxeColumn>
            <VxeColumn
              fixed="right"
              field="action"
              :title="$t('AbpUi.Actions')"
              width="100"
            >
              <template #default="{ row }">
                <div class="flex flex-row">
                  <div v-if="!row.completed">
                    <Button
                      v-if="row.paused || row.error"
                      :icon="h(CaretRightOutlined)"
                      @click="onResume(row)"
                      type="link"
                    />
                    <Button
                      v-else
                      :icon="h(PauseOutlined)"
                      @click="onPause(row)"
                      type="link"
                    />
                  </div>
                  <Button
                    :icon="h(DeleteOutlined)"
                    @click="onDelete(row)"
                    type="link"
                    danger
                  />
                </div>
              </template>
            </VxeColumn>
          </VxeTable>
        </template>
      </UploaderList>
    </Uploader>
  </Modal>
</template>

<style scoped></style>
