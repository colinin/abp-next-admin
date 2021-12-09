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
  import { defineComponent, ref } from 'vue';
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
    setup() {
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
      };
    },
    mounted() {
      this.onLoadAllDataDic();
    },
    methods: {
      getContentMenus(node: any): ContextMenuItem[] {
        return [
          {
            label: this.L('Data:Edit'),
            handler: () => {
              get(node.eventKey).then((res) => {
                this.formTitle = this.L('Data:Edit');
                this.openDataModal(true, res, true);
              });
            },
            icon: 'ant-design:edit-outlined',
          },
          {
            label: this.L('Data:AddNew'),
            handler: () => {
              this.handleNewData(node.eventKey);
            },
            icon: 'ant-design:plus-outlined',
          },
          {
            label: this.L('Data:AppendItem'),
            handler: () => {
              this.getDataId = node.eventKey;
              this.openItemModal(true, {}, true);
            },
            icon: 'ant-design:plus-square-outlined',
          },
          {
            label: this.L('Data:Delete'),
            handler: () => {
              Modal.warning({
                title: this.t('AbpUi.AreYouSure'),
                content: this.t('AbpUi.ItemWillBeDeletedMessage'),
                okCancel: true,
                onOk: () => {
                  remove(node.eventKey).then(() => {
                    this.onLoadAllDataDic();
                  });
                },
              });
            },
            icon: 'ant-design:delete-outlined',
          },
        ];
      },
      onLoadAllDataDic() {
        getAll().then((res) => {
          this.treeData = listToTree(res.items, {
            pid: 'parentId',
          });
        });
      },
      handleNodeChange(selectKeys: String[]) {
        if (selectKeys.length > 0) {
          this.$emit('change', selectKeys[0]);
        }
      },
      handleNewData(parentId: string | null) {
        this.formTitle = this.L('Data:AddNew');
        this.openDataModal(
          true,
          {
            parentId: parentId,
          } as Data,
          true,
        );
      },
      handleSaveChanges(val) {
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
          this.onLoadAllDataDic();
        });
      },
    },
  });
</script>
