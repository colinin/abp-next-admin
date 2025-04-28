<script setup lang="ts">
import { defineAsyncComponent, ref } from 'vue';

const FolderTree = defineAsyncComponent(() => import('./FolderTree.vue'));
const FileList = defineAsyncComponent(() => import('./FileList.vue'));

const bucket = ref('');
const path = ref('');

function onBucketChange(val: string) {
  bucket.value = val;
  path.value = '';
}

function onFolderChange(val: string) {
  path.value = val;
}
</script>

<template>
  <div class="flex flex-row gap-2">
    <div style="width: 30%">
      <FolderTree
        @bucket-change="onBucketChange"
        @folder-change="onFolderChange"
      />
    </div>
    <div style="width: 70%">
      <FileList :bucket="bucket" :path="path" />
    </div>
  </div>
</template>

<style scoped></style>
