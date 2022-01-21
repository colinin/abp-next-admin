<template>
  <Card :title="title" size="small">
    <template #extra>
      <a-button type="primary" @click="handleNewData(null)">{{ L('Data:AddNew') }}</a-button>
    </template>

    <BasicTree
      :tree-data="treeData"
      :replace-fields="replaceFields"
      defaultExpandLevel="1"
      :before-right-click="getContentMenus"
      @select="handleNodeChange"
    />
    <BasicModalForm
      @register="registerDataModal"
      :save-changes="handleSaveChanges"
      :form-items="formItems"
      :title="formTitle"
    />

    <DataItemModal @register="registerItemModal" :data-id="getDataId" />
  </Card>
</template>

<script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  import { Modal, Card } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { BasicTree, ContextMenuItem } from '/@/components/Tree/index';
  import { listToTree } from '/@/utils/helper/treeHelper';

  import { getAll, get, update, create, remove } from '/@/api/platform/dataDic';
  import { Data } from '/@/api/platform/model/dataModel';
  import { getDateFormSchemas } from './ModalData';

  import DataItemModal from './DataItemModal.vue';

  export default defineComponent({
    name: 'DataTree',
    components: {
      Card,
      BasicTree,
      BasicModalForm,
      DataItemModal,
    },
    emits: ['change', 'append-item'],
    setup(_, { emit }) {
      const { L } = useLocalization('AppPlatform');
      const title = L('DisplayName:DataDictionary');
      const treeData = ref<Data[]>();
      const replaceFields = ref({
        title: 'displayName',
        key: 'id',
      });
      const [registerDataModal, { openModal: openDataModal, closeModal: closeDataModal }] =
        useModal();
      const [registerItemModal, { openModal: openItemModal }] = useModal();
      const formItems = getDateFormSchemas();
      const formTitle = ref('');
      const getDataId = ref('');

      onMounted(onLoadAllDataDic);

      function getContentMenus(node: any): ContextMenuItem[] {
        return [
          {
            label: L('Data:Edit'),
            handler: () => {
              get(node.eventKey).then((res) => {
                formTitle.value = L('Data:Edit');
                openDataModal(true, res, true);
              });
            },
            icon: 'ant-design:edit-outlined',
          },
          {
            label: L('Data:AddNew'),
            handler: () => {
              handleNewData(node.eventKey);
            },
            icon: 'ant-design:plus-outlined',
          },
          {
            label: L('Data:AppendItem'),
            handler: () => {
              getDataId.value = node.eventKey;
              openItemModal(true, {}, true);
            },
            icon: 'ant-design:plus-square-outlined',
          },
          {
            label: L('Data:Delete'),
            handler: () => {
              Modal.warning({
                title: L('AbpUi.AreYouSure'),
                content: L('AbpUi.ItemWillBeDeletedMessage'),
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

      function handleNewData(parentId: string | null) {
        formTitle.value = L('Data:AddNew');
        openDataModal(
          true,
          {
            parentId: parentId,
          } as Data,
          true,
        );
      }
      
      function handleSaveChanges(val) {
        const api: Promise<Data> = val.id
          ? update(val.id, {
              name: val.name,
              displayName: val.displayName,
              description: val.description,
            })
          : create({
              name: val.name,
              displayName: val.displayName,
              description: val.description,
              parentId: val.parentId,
            });
        return api.then(() => {
          onLoadAllDataDic();
        });
      }

      return {
        L,
        formItems,
        formTitle,
        title,
        getDataId,
        treeData,
        replaceFields,
        registerDataModal,
        openDataModal,
        closeDataModal,
        registerItemModal,
        openItemModal,
        getContentMenus,
        handleNewData,
        handleNodeChange,
        handleSaveChanges,
      };
    },
  });
</script>
