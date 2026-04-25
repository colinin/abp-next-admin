<script setup lang="ts">
import type { EventDataNode, Key } from 'ant-design-vue/es/vc-tree/interface';
import type { TreeProps } from 'ant-design-vue/es/vc-tree/props';

import type { BlobDto } from '../../types';
import type { BlobContainerDto } from '../../types/containers';

import { defineAsyncComponent, nextTick, onMounted, ref, watch } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Button, Card, DirectoryTree, Empty, Select } from 'ant-design-vue';

import { useBlobContainersApi } from '../../api/useBlobContainersApi';
import { useBlobsApi } from '../../api/useBlobsApi';

const emits = defineEmits<{
  (event: 'containerChange', id: string): void;
  (event: 'folderChange', id?: string): void;
}>();

interface Folder {
  children?: Folder[];
  id: string;
  isLeaf?: boolean;
  key: string;
  name: string;
  path: string;
  title: string;
}

const { getPagedListApi: getContainersApi } = useBlobContainersApi();
const { getFolderPagedListApi: getFoldersApi } = useBlobsApi();

const rootFolder: Folder = {
  id: '',
  isLeaf: false,
  key: './',
  name: './',
  path: '/',
  title: $t('BlobManagement.Blobs:RootFolder'),
  children: [],
};

const containerId = ref<string>('');
const loadedFolders = ref<string[]>([]);
const expandedFolders = ref<string[]>([]);
const selectedFolders = ref<string[]>([]);
const containers = ref<BlobContainerDto[]>([]);
const folders = ref<Folder[]>([
  {
    ...rootFolder,
  },
]);

const isLoadingContainers = ref(false);
const isLoadingFolders = ref(false);

const [BlobFolderModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./BlobFolderModal.vue'),
  ),
});

const clearTreeState = () => {
  folders.value = [
    {
      ...rootFolder,
    },
  ];
  loadedFolders.value = [];
  expandedFolders.value = [];
  selectedFolders.value = [];
  isLoadingFolders.value = false;
};

const onLoadChildFolders: TreeProps['loadData'] = async (treeNode) => {
  if (isLoadingContainers.value || !containerId.value) {
    return;
  }

  const nodeKey = treeNode.key?.toString();
  if (!nodeKey) return;

  if (loadedFolders.value.includes(nodeKey)) {
    return;
  }

  try {
    isLoadingFolders.value = true;
    const children = await getFolders(containerId.value, treeNode.dataRef?.id);

    if (treeNode.dataRef && containerId.value) {
      treeNode.dataRef.children = children;
      folders.value = [...folders.value];
      loadedFolders.value = [...loadedFolders.value, nodeKey];
    }
  } catch (error) {
    console.error('Failed to load folders:', error);
    if (treeNode.dataRef) {
      treeNode.dataRef.children = [];
    }
  } finally {
    isLoadingFolders.value = false;
  }
};

async function onInit() {
  try {
    isLoadingContainers.value = true;
    const { items } = await getContainersApi({
      maxResultCount: 1000,
    });
    containers.value = items;
  } catch (error) {
    console.error('Failed to load containers:', error);
  } finally {
    isLoadingContainers.value = false;
  }
}

async function getFolders(containerId: string, parentId?: string) {
  if (!containerId) {
    return [];
  }

  try {
    const { items } = await getFoldersApi({
      containerId,
      parentId,
    });
    return items.map((folder) => {
      return {
        isLeaf: false,
        id: folder.id,
        key: folder.id,
        name: folder.name,
        path: folder.path,
        title: folder.name,
        children: [],
      };
    });
  } catch (error) {
    console.error('Failed to get folders:', error);
    return [];
  }
}

function onFolderExpand(
  _: Key[],
  info: {
    expanded: boolean;
    node: EventDataNode;
  },
) {
  if (!info.expanded) {
    const keys = [...loadedFolders.value];
    const findIndex = keys.lastIndexOf(info.node?.key?.toString());
    if (findIndex !== -1) {
      keys.splice(findIndex, 1);
      loadedFolders.value = keys;
    }
  }
}

function onFolderChange(folderId?: string) {
  emits('folderChange', folderId);
}

async function onContainerChange(newContainerId: string) {
  clearTreeState();
  emits('containerChange', newContainerId);
  await nextTick();
}

function onCreate() {
  if (!containerId.value) {
    return;
  }
  modalApi.setData({
    containerId: containerId.value,
    parentId: selectedFolders.value[0],
  });
  modalApi.open();
}

function onFolderCreated(blob: BlobDto) {
  const parentId = selectedFolders.value[0];
  if (parentId) {
    const index = loadedFolders.value.indexOf(parentId);
    if (index !== -1) {
      loadedFolders.value.splice(index, 1);
    }

    if (expandedFolders.value.includes(parentId)) {
      const parentNode = findNodeById(folders.value, parentId);
      if (parentNode && parentNode.children) {
        parentNode.children = [];
        folders.value = [...folders.value];
      }
    }
  }

  emits('folderChange', blob.id);
}

function findNodeById(nodes: Folder[], id: string): Folder | null {
  for (const node of nodes) {
    if (node.id === id) {
      return node;
    }
    if (node.children) {
      const found = findNodeById(node.children, id);
      if (found) return found;
    }
  }
  return null;
}

watch(containerId, (newVal, oldVal) => {
  if (newVal !== oldVal && newVal) {
    onContainerChange(newVal);
  }
});

onMounted(onInit);
</script>

<template>
  <Card :title="$t('BlobManagement.Blobs:Folder')" class="h-full">
    <div class="flex h-full flex-col gap-2">
      <Select
        :placeholder="$t('BlobManagement.Blobs:SelectContainer')"
        :options="containers"
        :field-names="{ label: 'name', value: 'id' }"
        :loading="isLoadingContainers"
        v-model:value="containerId"
        @change="(e) => onContainerChange(e!.toString())"
      />
      <Button
        v-if="containerId"
        block
        type="primary"
        ghost
        :disabled="isLoadingFolders"
        @click="onCreate"
      >
        {{ $t('BlobManagement.Blobs:CreateFolder') }}
      </Button>
      <div class="h-[calc(100%-100px)] overflow-auto">
        <!-- 添加 key 属性强制重新渲染树组件 -->
        <DirectoryTree
          v-if="containerId"
          :key="containerId"
          block-node
          :expand-action="false"
          v-model:expanded-keys="expandedFolders"
          v-model:selected-keys="selectedFolders"
          :loaded-keys="loadedFolders"
          :tree-data="folders"
          :load-data="onLoadChildFolders"
          @select="(_, info) => onFolderChange(info.node?.dataRef?.id)"
          @expand="onFolderExpand"
        />
        <Empty v-else />
      </div>
    </div>
  </Card>
  <BlobFolderModal @change="onFolderCreated" />
</template>

<style scoped lang="scss">
:deep(.ant-tree) {
  .ant-tree-title {
    white-space: normal;
  }
}

:deep(.ant-card-body) {
  height: calc(100% - 56px);
}
</style>
