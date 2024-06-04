<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleAddNew">{{ L('Layout:AddNew') }}</a-button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
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
      </template>
    </BasicTable>
    <LayoutModal @change="reload" @register="registerLayoutModal" />
  </div>
</template>

<script lang="ts" setup>
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { getList, deleteById } from '/@/api/platform/layouts';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import LayoutModal from './LayoutModal.vue';

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization(['AppPlatform', 'AbpUi']);
  const [registerTable, { reload }] = useTable({
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
    },
  });
  const [registerLayoutModal, { openModal: openLayoutModal }] = useModal();

  function handleAddNew() {
    openLayoutModal(true, {});
  }

  function handleEdit(record: Recordable) {
    openLayoutModal(true, record);
  }

  function handleDelete(record: Recordable) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', [record.displayName]),
      okCancel: true,
      onOk: () => {
        return deleteById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
</script>
