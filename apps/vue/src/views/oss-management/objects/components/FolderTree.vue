<template>
  <div class="folder-tree-wrap">
    <Row>
      <Col :span="24">
        <Button
          v-if="enabledNewFolder"
          style="width: 100%; margin-bottom: 20px"
          type="primary"
          ghost
          @click="handleNewFolder">{{ L('Objects:CreateFolder') }}</Button>
      </Col>
      <Col :span="24">
        <DirectoryTree
          v-if="enabledNewFolder"
          v-model:expandedKeys="expandedKeys"
          v-model:selectedKeys="selectedKeys"
          :tree-data="folders"
          :load-data="fetchChildren"
          @select="handleSelectChange"
        />
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
  import { getObjects } from '/@/api/oss-management/oss';
  import { Folder } from '../datas/typing';
  import OssFolderModal from './OssFolderModal.vue';

  const DirectoryTree = Tree.DirectoryTree;

  const emits = defineEmits(['select']);
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
  const expandedKeys = ref<string[]>([]);
  const selectedKeys = ref<string[]>([]);
  const folders = ref<Folder[]>([
    {
      title: L('Objects:Root'),
      key: './',
      path: '',
      name: './',
      children: [],
    },
  ]);
  watch(
    () => props.bucket,
    (bucket) => {
      if (bucket) {
        fetchFolders(bucket).then((fs) => {
          folders.value[0].children = fs;
        });
      }
    },
    {
      immediate: true,
    },
  )
  const fetchChildren: TreeProps['loadData'] = treeNode => {
    return new Promise((resolve) => {
      if (treeNode.dataRef!.children!.length > 0) {
        resolve();
        return;
      }
      let path = '';
      if (treeNode.dataRef?.path) {
        path = path + treeNode.dataRef?.path;
      }
      if (treeNode.dataRef?.name) {
        path = path + treeNode.dataRef?.name;
      }
      fetchFolders(props.bucket, path).then((fs) => {
        treeNode.dataRef!.children = fs;
        folders.value = [...folders.value];
        resolve();
      }).catch(() => {
        resolve();
      });
    });
  };
  
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
      }).then((res) => {
        const fs = res.objects
          .filter((item) => item.isFolder)
          .map((item): Folder => {
            return {
              key: `${item.path ?? ''}${item.name}`,
              name: item.name,
              title: item.name,
              path: item.path,
              children: [],
            };
          });
        return resolve(fs);
      }).catch(() => {
        return resolve([]);
      })
    })
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

  function handleFolderChange() {
    console.log(folders.value);
    // TODO: 刷新目录
  }
</script>

<style lang="less" scoped>
  .folder-tree-wrap {
    height: 100%;
  }
</style>
