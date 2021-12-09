<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleAddNew">{{ L('Layout:AddNew') }}</a-button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'Platform.Layout.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'Platform.Layout.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <LayoutModal @change="reloadTable" @register="registerLayoutModal" :layout-id="layoutId" />
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';

  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Modal } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { getList, deleteById } from '/@/api/platform/layout';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import LayoutModal from './LayoutModal.vue';
  export default defineComponent({
    name: 'LayoutTable',
    components: {
      BasicTable,
      TableAction,
      LayoutModal,
    },
    setup() {
      const { L } = useLocalization('AppPlatform', 'AbpUi');
      const layoutId = ref('');
      const [registerTable, { reload: reloadTable }] = useTable({
        rowKey: 'id',
        title: L('DisplayName:Layout'),
        columns: getDataColumns(),
        api: getList,
        beforeFetch: formatPagedRequest,
        bordered: true,
        canResize: true,
        showTableSetting: true,
        useSearchForm: true,
        formConfig: getSearchFormSchemas(),
        rowSelection: { type: 'checkbox' },
        actionColumn: {
          width: 160,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });
      const [registerLayoutModal, { openModal: openLayoutModal }] = useModal();

      return {
        L,
        layoutId,
        reloadTable,
        registerTable,
        openLayoutModal,
        registerLayoutModal,
      };
    },
    methods: {
      handleAddNew() {
        this.openLayoutModal(true, {}, true);
      },
      handleEdit(record: Recordable) {
        this.openLayoutModal(true, record, true);
      },
      handleDelete(record: Recordable) {
        Modal.warning({
          title: this.L('AreYouSure'),
          content: this.L('ItemWillBeDeletedMessageWithFormat', [record.displayName] as Recordable),
          okCancel: true,
          onOk: () => {
            deleteById(record.id).then(() => {
              this.reloadTable();
            });
          },
        });
      },
    },
  });
</script>
