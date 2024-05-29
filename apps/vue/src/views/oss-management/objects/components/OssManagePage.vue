<template>
  <div class="content">
    <Card :title="L('Objects:FileSystem')">
      <CardGrid style="width: 25%; max-height: 800px; overflow: auto">
        <CardMeta>
          <template #title>
            <Select
              style="width: 100%"
              :placeholder="L('Containers:Select')"
              :options="bucketList"
              :field-names="{
                label: 'name',
                value: 'name',
              }"
              @change="handleBucketChange"
            />
          </template>
          <template #description>
            <FolderTree
              ref="folderTreeRef"
              :bucket="currentBucket"
              @select="handlePathChange"
              @folder:created="handlePathCreated"
            />
          </template>
        </CardMeta>
      </CardGrid>
      <CardGrid style="width: 75%">
        <FileList
          ref="fileListRef"
          :bucket="currentBucket"
          :path="currentPath"
          @folder:delete="handlePathDeleted"
        />
      </CardGrid>
    </Card>
  </div>
</template>

<script lang="ts" setup>
  import { ref, unref, onMounted } from 'vue';
  import { Card, Select } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getContainers } from '/@/api/oss-management/containers';
  import { OssContainer } from '/@/api/oss-management/model/ossModel';
  import FolderTree from './FolderTree.vue';
  import FileList from './FileList.vue';

  const CardGrid = Card.Grid;
  const CardMeta = Card.Meta;

  const { L } = useLocalization(['AbpOssManagement', 'AbpUi']);
  const fileListRef = ref<any>();
  const folderTreeRef = ref<any>();
  const currentPath = ref('');
  const currentBucket = ref('');
  const bucketList = ref<OssContainer[]>([]);

  onMounted(fetchBuckets);

  function fetchBuckets() {
    getContainers({
      prefix: '',
      marker: '',
      sorting: '',
      skipCount: 0,
      maxResultCount: 1000,
    }).then((res) => {
      bucketList.value = res.containers;
    });
  }

  function handleBucketChange(bucket: string) {
    currentBucket.value = bucket;
  }

  function handlePathChange(path: string) {
    currentPath.value = path;
  }

  function handlePathCreated() {
    const fileList = unref(fileListRef);
    fileList?.refresh();
  }

  function handlePathDeleted(_bucket: string, path: string) {
    console.log(_bucket);
    console.log(path);
    console.log(name);
    const folderTree = unref(folderTreeRef);
    folderTree?.refresh(path);
  }
</script>
