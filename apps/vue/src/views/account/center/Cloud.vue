<template>
  <div class="content">
    <Card>
      <CardGrid style="width: 25%">
        <CardMeta :title="L('Cloud')">
          <template #description>
            <DirectoryTree
              :tree-data="folderTree"
              :expandedKeys="expandedKeys"
              @expand="fetchFolders"
              @select="handleSelectFolder"
            />
          </template>
        </CardMeta>
      </CardGrid>
      <CardGrid style="width: 75%">
        <component
          :is="switchComponent.name"
          :select-group="switchComponent.group"
          :select-path="switchComponent.path"
          :delete-enabled="deleteEnabled"
          @append:folder="handleAppendFolder"
        />
      </CardGrid>
    </Card>
  </div>
</template>

<script lang="ts">
  import { computed, defineComponent, ref } from 'vue';
  import { Card, Tree } from 'ant-design-vue';
  import { TreeDataItem } from 'ant-design-vue/es/tree/Tree';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { OssObject } from '/@/api/oss-management/model/ossModel';
  import FileList from './FileList.vue';
  import ShareList from './ShareList.vue';

  interface IComponent {
    name: string;
    group: string;
    path: string;
    dataRef: any;
  }

  export default defineComponent({
    components: {
      Card,
      CardGrid: Card.Grid,
      CardMeta: Card.Meta,
      DirectoryTree: Tree.DirectoryTree,
      FileList,
      ShareList,
    },
    setup() {
      const { hasPermission } = usePermission();
      const { L } = useLocalization('AbpOssManagement', 'AbpUi');
      const folderTreeRef = ref<{ [key: string]: TreeDataItem }>({
        private: {
          key: 'private',
          group: 'private',
          title: L('MyDocument'),
          path: '/',
          children: [],
        },
        public: {
          key: 'public',
          group: 'public',
          title: L('PublicDocument'),
          path: '/',
          children: [],
        },
        share: {
          key: 'share',
          group: 'share',
          title: L('MyShare'),
          path: '/',
          children: [],
        },
      });
      const folderTree = computed(() => {
        return Object.keys(folderTreeRef.value).map((key) => folderTreeRef.value[key]);
      });
      const switchComponent = ref<IComponent>({
        name: 'FileList',
        group: '',
        path: '/',
        dataRef: {},
      });
      const deleteEnabled = computed(() => {
        switch (switchComponent.value.group) {
          case 'private':
          case 'share':
            return true;
          case 'public':
            return hasPermission('AbpOssManagement.OssObject.Delete');
          default:
            return false;
        }
      });
      const expandedKeys = ref<string[]>([]);

      function fetchFolders(keys) {
        expandedKeys.value = keys;
      }

      function handleSelectFolder(_, e) {
        switch (e.node.dataRef.group) {
          case 'private':
          case 'public':
            switchComponent.value = {
              name: 'FileList',
              group: e.node.dataRef.group,
              path: e.node.dataRef.path,
              dataRef: e.node.dataRef,
            };
            break;
          case 'share':
            switchComponent.value = {
              name: 'ShareList',
              group: e.node.dataRef.group,
              path: e.node.dataRef.path,
              dataRef: e.node.dataRef,
            };
            break;
        }
      }

      function handleAppendFolder(folders: OssObject[]) {
        switchComponent.value.dataRef.children = folders.map((obj) => {
          return {
            key: obj.name,
            group: switchComponent.value.group,
            title: obj.name,
            path: obj.name,
            children: [],
          };
        });
      }

      return {
        L,
        switchComponent,
        folderTree,
        deleteEnabled,
        expandedKeys,
        fetchFolders,
        handleSelectFolder,
        handleAppendFolder,
      };
    },
  });
</script>
