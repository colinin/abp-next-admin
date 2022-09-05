<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button type="primary" @click="handleAddNew">{{ L('AddNew') }}</Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'enabled'">
          <CheckOutlined v-if="record.enabled" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'actions'">
          <TableAction
            :actions="[
              {
                auth: 'AbpIdentityServer.Clients.Update',
                icon: 'ant-design:edit-outlined',
                label: L('Edit'),
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpIdentityServer.Clients.Delete',
                color: 'error',
                icon: 'ant-design:delete-outlined',
                label: L('Delete'),
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'AbpIdentityServer.Clients.ManagePermissions',
                label: L('Permissions'),
                onClick: handlePermission.bind(null, record),
              },
              {
                auth: 'AbpIdentityServer.Clients.Clone',
                label: L('Client:Clone'),
                onClick: handleClone.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <ClientModal @register="registerModal" @change="handleChange" />
    <PermissionModal @register="registerPermissionModal" />
    <ClientClone @register="registerCloneModal" @change="handleChange" />
  </div>
</template>

<script lang="ts" setup>
  import { Button } from 'ant-design-vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { deleteById, getList } from '/@/api/identity-server/clients';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { PermissionModal } from '/@/components/Permission';
  import ClientModal from './ClientModal.vue';
  import ClientClone from './ClientClone.vue';

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization('AbpIdentityServer');
  const [registerModal, { openModal, closeModal }] = useModal();
  const [registerCloneModal, { openModal: openCloneModal }] = useModal();
  const [registerPermissionModal, { openModal: openPermissionModal }] = useModal();
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('DisplayName:ApiScopes'),
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
      width: 200,
      title: L('Actions'),
      dataIndex: 'actions',
    },
  });

  function handleAddNew() {
    openModal(true, {});
  }

  function handleEdit(record) {
    openModal(true, record);
  }

  function handleChange() {
    closeModal();
    reload();
  }

  function handleClone(record) {
    openCloneModal(true, record);
  }

  function handlePermission(record) {
    const props = {
      providerName: 'C',
      providerKey: record.clientId,
    };
    openPermissionModal(true, props, true);
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
