<template>
  <Card :title="title" size="small">
    <template #extra>
      <a-button type="primary" @click="openDataModal(true, {})">{{ L('Data:AddNew') }}</a-button>
    </template>

    <BasicTree
      :tree-data="treeData"
      :field-names="replaceFields"
      defaultExpandLevel="1"
      :before-right-click="getContentMenus"
      @select="handleNodeChange"
    />

    <DataModal @register="registerDataModal" @change="onLoadAllDataDic" />
    <DataItemModal @register="registerItemModal" @change="(dataId) => handleNodeChange([dataId])" />
  </Card>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  import { Modal, Card } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicTree, ContextMenuItem } from '/@/components/Tree/index';
  import { listToTree } from '/@/utils/helper/treeHelper';

  import { getAll, remove } from '/@/api/platform/dataDic';
  import { Data } from '/@/api/platform/model/dataModel';
  import DataModal from './DataModal.vue';
  import DataItemModal from './DataItemModal.vue';

  export default defineComponent({
    name: 'DataTree',
    components: {
      Card,
      BasicTree,
      DataModal,
      DataItemModal,
    },
    emits: ['change', 'append-item'],
    setup(_, { emit }) {
      const { L } = useLocalization(['AppPlatform', 'AbpUi']);
      const title = L('DisplayName:DataDictionary');
      const treeData = ref<Data[]>([]);
      const replaceFields = ref({
        title: 'displayName',
        key: 'id',
        value: 'id',
      });
      const [registerItemModal, { openModal: openItemModal }] = useModal();
      const [registerDataModal, { openModal: openDataModal }] = useModal();

      onMounted(onLoadAllDataDic);

      function getContentMenus(node: any): ContextMenuItem[] {
        return [
          {
            label: L('Data:Edit'),
            handler: () => {
              openDataModal(true, { id: node.eventKey });
            },
            icon: 'ant-design:edit-outlined',
          },
          {
            label: L('Data:AddNew'),
            handler: () => {
              openDataModal(true, { parentId: node.eventKey });
            },
            icon: 'ant-design:plus-outlined',
          },
          {
            label: L('Data:AppendItem'),
            handler: () => {
              openItemModal(true, { dataId: node.eventKey }, true);
            },
            icon: 'ant-design:plus-square-outlined',
          },
          {
            label: L('Data:Delete'),
            handler: () => {
              Modal.warning({
                title: L('AreYouSure'),
                content: L('ItemWillBeDeletedMessage'),
                okCancel: true,
                onOk: () => {
                  remove(node.eventKey).then(() => {
                    onLoadAllDataDic();
                  });
                },
              });
            },
            icon: 'ant-design:delete-outlined',
          },
        ];
      }

      function onLoadAllDataDic() {
        getAll().then((res) => {
          treeData.value = listToTree(res.items, {
            pid: 'parentId',
          });
        });
      }

      function handleNodeChange(selectKeys: String[]) {
        if (selectKeys.length > 0) {
          emit('change', selectKeys[0]);
        }
      }

      return {
        L,
        title,
        treeData,
        replaceFields,
        registerDataModal,
        openDataModal,
        registerItemModal,
        openItemModal,
        getContentMenus,
        handleNodeChange,
        onLoadAllDataDic,
      };
    },
  });
</script>
