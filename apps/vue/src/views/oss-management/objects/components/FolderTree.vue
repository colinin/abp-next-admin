<template>
  <div class="folder-tree-wrap">
    <Row>
      <Col :span="24">
        <Button
          v-if="enabledNewFolder"
          style="width: 100%; margin-bottom: 20px"
          type="primary"
          ghost
          @click="handleNewFolder"
          >{{ L('Objects:CreateFolder') }}</Button
        >
      </Col>
      <Col :span="24">
        <div class="folder-tree">
          <DirectoryTree
            v-if="enabledNewFolder"
            ref="folderTreeRef"
            v-model:expandedKeys="expandedKeys"
            v-model:selectedKeys="selectedKeys"
            :loadedKeys="loadedKeys"
            :tree-data="folders"
            :load-data="fetchChildren"
            @select="handleSelectChange"
            @expand="handleFolderExpand"
          />
        </div>
      </Col>
    </Row>
    <OssFolderModal @register="registerFolderModal" @change="handleFolderChange" />
  </div>
</template>

<script lang="ts" setup>
  import type { TreeProps } from 'ant-design-vue';
  import { computed, ref, watch } from 'vue';
  import { useModal } from '/@/components/Modal';
  import { Button, Tree, Row, Col } from 'ant-design-vue';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getObjects } from '/@/api/oss-management/objects';
  import { Folder } from '../datas/typing';
  import OssFolderModal from './OssFolderModal.vue';

  const DirectoryTree = Tree.DirectoryTree;

  const emits = defineEmits(['select', 'folder:created']);
  const props = defineProps({
    bucket: {
      type: String,
      default: '',
    },
  });
  const { hasPermission } = usePermission();
  const { L } = useLocalization(['AbpOssManagement', 'AbpUi']);
  const [registerFolderModal, { openModal: openFolderModal }] = useModal();
  const enabledNewFolder = computed(() => {
    return hasPermission('AbpOssManagement.OssObject.Create') && (props.bucket ? true : false);
  });
  const folderTreeRef = ref<any>();
  const loadedKeys = ref<string[]>([]);
  const expandedKeys = ref<string[]>([]);
  const selectedKeys = ref<string[]>([]);
  const folders = ref<Folder[]>([
    {
      title: L('Objects:Root'),
      key: './',
      path: '',
      name: './',
      isLeaf: false,
      children: [],
    },
  ]);
  watch(
    () => props.bucket,
    (bucket) => {
      if (bucket) {
        expandedKeys.value = [];
        selectedKeys.value = [];
        fetchFolders(bucket).then((fs) => {
          var foldersRoot: Folder[] = [
            {
              title: L('Objects:Root'),
              key: './',
              path: '',
              name: './',
              isLeaf: false,
              children: fs,
            },
          ];
          folders.value = foldersRoot;
        });
      }
    },
    {
      immediate: true,
    },
  );
  const fetchChildren: TreeProps['loadData'] = (treeNode) => {
    return new Promise((resolve) => {
      // if (treeNode.dataRef!.children!.length > 0) {
      //   resolve();
      //   return;
      // }
      let path = '';
      if (treeNode.dataRef?.path) {
        path = path + treeNode.dataRef?.path;
      }
      if (treeNode.dataRef?.name) {
        path = path + treeNode.dataRef?.name;
      }
      fetchFolders(props.bucket, path)
        .then((fs) => {
          treeNode.dataRef!.children = fs;
          folders.value = [...folders.value];
          loadedKeys.value = [...loadedKeys.value, treeNode.key.toString()];
          resolve();
        })
        .catch(() => {
          resolve();
        });
    });
  };

  function handleFolderExpand(_, e: any) {
    if (!e.expanded) {
      const keys = loadedKeys.value;
      const findIndex = keys.findLastIndex((key) => key === e.node.key);
      findIndex >= 0 && keys.splice(findIndex);
      loadedKeys.value = keys;
    }
  }

  function fetchFolders(bucket: string, path?: string): Promise<Folder[]> {
    return new Promise((resolve) => {
      getObjects({
        bucket: bucket,
        prefix: path ?? '',
        delimiter: '/',
        marker: '',
        encodingType: '',
        sorting: '',
        skipCount: 0,
        maxResultCount: 1000,
      })
        .then((res) => {
          const fs = res.objects
            .filter((item) => item.isFolder)
            .map((item): Folder => {
              return {
                key: `${item.path ?? ''}${item.name}`,
                name: item.name,
                title: item.name,
                path: item.path,
                children: [],
                isLeaf: false,
              };
            });
          return resolve(fs);
        })
        .catch(() => {
          return resolve([]);
        });
    });
  }

  function handleSelectChange(selectedKeys: string[]) {
    if (selectedKeys.length === 1) {
      emits('select', selectedKeys[0]);
    }
  }

  function handleNewFolder() {
    openFolderModal(true, {
      bucket: props.bucket,
      path: selectedKeys.value[0],
    });
  }

  function handleFolderChange(bucket: string, path: string, name: string) {
    refresh(path);
    emits('folder:created', bucket, path, name);
  }

  function refresh(path: string) {
    // 刷新目录的方式为收起目录，并设置为未加载状态
    // 当用户手动展开目录时重载目录结果
    const keys = expandedKeys.value;
    const findIndex = keys.findLastIndex((key) => key === path);
    if (findIndex >= 0) {
      keys.splice(findIndex);
      expandedKeys.value = keys;
      loadedKeys.value = [];
    }
  }

  defineExpose({
    refresh,
  });
</script>

<style lang="less" scoped>
  .folder-tree-wrap {
    height: 100%;
  }

  .folder-tree {
    overflow-x: auto;
  }
</style>
