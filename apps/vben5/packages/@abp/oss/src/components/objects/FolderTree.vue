<script setup lang="ts">
import type { EventDataNode, Key } from 'ant-design-vue/es/vc-tree/interface';
import type { TreeProps } from 'ant-design-vue/es/vc-tree/props';

import type { OssContainerDto } from '../../types';
import type { OssObjectDto } from '../../types/objects';

import { defineAsyncComponent, onMounted, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Button, Card, DirectoryTree, Empty, Select } from 'ant-design-vue';

import { useContainesApi } from '../../api';

const emits = defineEmits<{
  (event: 'bucketChange', data: string): void;
  (event: 'folderChange', data: string): void;
}>();

interface Folder {
  children?: Folder[];
  isLeaf?: boolean;
  key: string;
  name: string;
  path?: string;
  title: string;
}

const { getListApi: getContainersApi, getObjectsApi } = useContainesApi();

const [FolderModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./FolderModal.vue')),
});

const rootFolder: Folder = {
  isLeaf: false,
  key: './',
  name: './',
  path: '',
  title: $t('AbpOssManagement.Objects:Root'),
  children: [],
};

const bucket = ref<string>('');
const loadedFolders = ref<string[]>([]);
const expandedFolders = ref<string[]>([]);
const selectedFolders = ref<string[]>([]);
const containers = ref<OssContainerDto[]>([]);
const folders = ref<Folder[]>([
  {
    ...rootFolder,
  },
]);

const onLoadChildFolders: TreeProps['loadData'] = async (treeNode) => {
  let path = '';
  if (treeNode.dataRef?.path) {
    path = path + treeNode.dataRef?.path;
  }
  if (treeNode.dataRef?.name) {
    path = path + treeNode.dataRef?.name;
  }
  try {
    treeNode.dataRef!.children = await getFolders(bucket.value!, path);
  } catch {
    treeNode.dataRef!.children = [];
  }
  folders.value = [...folders.value];
  loadedFolders.value = [...loadedFolders.value, treeNode.key.toString()];
};

async function onInit() {
  const getContainersRes = await getContainersApi({
    maxResultCount: 1000,
  });
  containers.value = getContainersRes.containers;
}

async function getFolders(bucket: string, path?: string) {
  const { objects } = await getObjectsApi({
    bucket,
    delimiter: '/',
    maxResultCount: 1000,
    prefix: path ?? '',
  });
  return objects
    .filter((f) => f.isFolder)
    .map((folder) => {
      return {
        isLeaf: false,
        key: `${folder.path ?? ''}${folder.name}`,
        name: folder.name,
        path: folder.path,
        title: folder.name,
        children: [],
      };
    });
}

function onFolderExpand(
  _: Key[],
  info: {
    expanded: boolean;
    node: EventDataNode;
  },
) {
  if (!info.expanded) {
    const keys = loadedFolders.value;
    const findIndex = keys.lastIndexOf(info.node.key.toString());
    findIndex !== -1 && keys.splice(findIndex);
    loadedFolders.value = keys;
  }
}

function onFolderChange(selectedKeys: Key[]) {
  if (selectedKeys.length === 1) {
    emits('folderChange', selectedKeys[0]!.toString());
  }
}

async function onBucketChange(bucket: string) {
  emits('bucketChange', bucket);
  expandedFolders.value = [];
  loadedFolders.value = [];
  folders.value = [
    {
      ...rootFolder,
    },
  ];
}

function onCreate() {
  modalApi.setData({
    bucket: bucket.value,
    path: selectedFolders.value[0],
  });
  modalApi.open();
}

function onFolderCreated(ossObject: OssObjectDto) {
  const keys = expandedFolders.value;
  const findIndex = keys.lastIndexOf(ossObject.path);
  if (findIndex !== -1) {
    keys.splice(findIndex);
    expandedFolders.value = keys;
    loadedFolders.value = [];
  }
}

onMounted(onInit);
</script>

<template>
  <Card :title="$t('AbpOssManagement.Containers')">
    <div class="flex flex-col gap-2">
      <Select
        :placeholder="$t('AbpOssManagement.Containers:Select')"
        :options="containers"
        :field-names="{ label: 'name', value: 'name' }"
        v-model:value="bucket"
        @change="(e) => onBucketChange(e!.toString())"
      />
      <Button v-if="bucket" block type="primary" ghost @click="onCreate">
        {{ $t('AbpOssManagement.Objects:CreateFolder') }}
      </Button>
      <DirectoryTree
        v-if="bucket"
        block-node
        v-model:expanded-keys="expandedFolders"
        v-model:selected-keys="selectedFolders"
        :loaded-keys="loadedFolders"
        :tree-data="folders"
        :load-data="onLoadChildFolders"
        @select="onFolderChange"
        @expand="onFolderExpand"
      />
      <Empty v-else />
    </div>
  </Card>
  <FolderModal @change="onFolderCreated" />
</template>

<style scoped lang="scss">
:deep(.ant-tree) {
  .ant-tree-title {
    // word-break: break-word;
    white-space: normal;
  }
}
</style>
