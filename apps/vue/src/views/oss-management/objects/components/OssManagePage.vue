<template>
  <div class="content">
    <Card :title="L('Objects:FileSystem')">
      <CardGrid style="width: 25%; max-height: 800px; overflow: auto;">
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
            <FolderTree :bucket="currentBucket" @select="handlePathChange" />
          </template>
        </CardMeta>
      </CardGrid>
      <CardGrid style="width: 75%;">
        <FileList :bucket="currentBucket" :path="currentPath" />
      </CardGrid>
    </Card>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { Card, Select } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { getContainers } from '/@/api/oss-management/oss';
  import { OssContainer } from '/@/api/oss-management/model/ossModel';
  import FolderTree from './FolderTree.vue';
  import FileList from './FileList.vue';

  export default defineComponent({
    name: 'OssTable',
    components: {
      Card,
      CardGrid: Card.Grid,
      CardMeta: Card.Meta,
      Select,
      Option: Select.Option,
      FolderTree,
      FileList,
    },
    setup() {
      const { L } = useLocalization(['AbpOssManagement', 'AbpUi']);
      const { hasPermission } = usePermission();
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

      function handleBucketChange(bucket) {
        currentBucket.value = bucket;
      }

      function handlePathChange(path) {
        currentPath.value = path;
      }

      return {
        L,
        bucketList,
        currentBucket,
        currentPath,
        handleBucketChange,
        handlePathChange,
        hasPermission,
      };
    },
  });
</script>
