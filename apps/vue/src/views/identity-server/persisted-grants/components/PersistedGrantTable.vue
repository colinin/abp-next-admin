<template>
  <div>
    <BasicTable @register="registerTable">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'actions'">
          <TableAction
            :actions="[
              {
                color: 'success',
                icon: 'ant-design:search-outlined',
                label: L('Edit'),
                onClick: handleShow.bind(null, record),
              },
              {
                auth: 'AbpIdentityServer.Grants.Delete',
                color: 'error',
                icon: 'ant-design:delete-outlined',
                label: L('Grants:Delete'),
                onClick: handleDelete.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <PersistedGrantModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { deleteById, getList } from '/@/api/identity-server/persistedGrants';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import PersistedGrantModal from './PersistedGrantModal.vue';

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization('AbpIdentityServer');
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('DisplayName:PersistedGrants'),
    columns: getDataColumns(),
    api: getList,
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: true,
    immediate: true,
    canColDrag: true,
    formConfig: getSearchFormSchemas(),
    actionColumn: {
      width: 160,
      title: L('Actions'),
      dataIndex: 'actions',
    },
  });

  function handleShow(record) {
    openModal(true, record);
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
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
