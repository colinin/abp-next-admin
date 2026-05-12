<script setup lang="ts">
import { reactive, ref } from 'vue';

import { ColPage } from '@vben/common-ui';

import BlobFileTable from './BlobFileTable.vue';
import BlobFolderTree from './BlobFolderTree.vue';

const containerId = ref<string>('');
const folderId = ref<string>();

const props = reactive({
  leftCollapsible: false,
  leftMaxWidth: 30,
  leftMinWidth: 20,
  leftWidth: 30,
  resizable: true,
  rightWidth: 70,
  splitHandle: true,
  splitLine: true,
});

function onContainerChange(val: string) {
  containerId.value = val;
  folderId.value = undefined;
}

function onFolderChange(val?: string) {
  folderId.value = val;
}
</script>

<template>
  <ColPage auto-content-height v-bind="props">
    <template #left>
      <BlobFolderTree
        @container-change="onContainerChange"
        @folder-change="onFolderChange"
      />
    </template>
    <BlobFileTable :container-id="containerId" :folder-id="folderId" />
  </ColPage>
</template>

<style scoped></style>
